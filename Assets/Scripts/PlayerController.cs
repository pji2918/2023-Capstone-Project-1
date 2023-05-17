using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region 싱글톤 패턴 선언
    public static PlayerController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public int[] itemCounts;
    public int sumCount;

    // 플레이어 컨트롤러에 필요한 컴포넌트를 선언합니다.
    public NavMeshAgent _playerAgent;
    public Rigidbody2D _playerRigidbody;
    public Animator _playerAnimator;
    public GameObject _playerAttackEffect;
    public GameObject _dieFade, _gameOverUI;

    // 플레이어의 이동 속도 및 돌진 속도, 쿨타임을 저장하는 변수입니다.
    [SerializeField] public float _moveSpeed = 5f;
    [SerializeField] private float _dashSpeed;
    public float _dashCoolDown;
    public bool _isDash = false;

    public GameObject _bubblePrefab;
    public float _bubbleCoolDown;

    public GameObject _harpoonPrefab;
    public float _harpoonCoolDown;

    public GameObject _jangpungPrefab;
    public float _jangpungCoolDown;
    public bool _jangpungOn = false;

    public GameObject _minePrefab;
    public float _mineCoolDown;

    public float _foodCoolDown;

    bool _isAttacking;
    public bool _isDying = false;
    public int _playerMaxHp = 100;
    public int _playerHp;

    private float _attackCoolDown = 0f;

    // 플레이어의 NavMeshAgent 컴포넌트를 가져옵니다.
    void Start()
    {
        Debug.Log(_playerMaxHp);
        // 플레이어의 NavMesh 에이전트 컴포넌트를 가져오고, 자동 회전을 끕니다.
        _playerAgent = GetComponent<NavMeshAgent>();
        _playerAgent.updateRotation = false;
        _playerAgent.updateUpAxis = false;

        // 플레이어의 Rigidbody2D 컴포넌트를 가져옵니다.
        _playerRigidbody = GetComponent<Rigidbody2D>();

        // 플레이어의 Animator 컴포넌트를 가져옵니다.
        _playerAnimator = GetComponent<Animator>();

        if (GameManager.instance._healthReduce > 0)
        {
            _playerHp = _playerMaxHp - (_playerMaxHp * (GameManager.instance._healthReduce / 100));
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region 키 입력
        // 오른쪽 마우스 클릭을 하면 플레이어를 클릭한 위치로 이동시킵니다.
        if (Input.GetMouseButtonDown(1) && !_isDash && !_isDying)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlayerController.instance._playerAgent.SetDestination(mousePosition);
        }

        // 왼쪽 Shift 키를 누르면 플레이어를 돌진시키는 함수를 실행합니다.
        if (Input.GetKeyDown(KeyCode.LeftShift) && DataManager.instance._data.skillLevel >= 1)
        {
            StartCoroutine(PlayerController.instance.Dash());
        }



        if (Input.GetMouseButtonDown(0))
        {
            if (!_isDying && _attackCoolDown <= 0f)
            {
                _attackCoolDown = 0.5f;
                _isAttacking = true;

                StartCoroutine(Attack());

                _playerAnimator.SetTrigger("Attack");

                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // mousePosition이 플레이어의 왼쪽이면 플레이어를 왼쪽으로 바라보게 합니다.
                if (mousePosition.x < transform.position.x)
                {
                    transform.localScale = new Vector3(2, 2, 1);
                }
                else if (mousePosition.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-2, 2, 1);
                }

                // 애니메이션이 끝나면, 플레이어의 공격 이펙트를 비활성화합니다.
                StartCoroutine(AttackEffect());
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && DataManager.instance._data.skillLevel >= 2)
        {
            if (_harpoonCoolDown <= 0)
            {
                _harpoonCoolDown = 8f;
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
                GameObject harpoon = Instantiate(_harpoonPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
                Destroy(harpoon, 5f);
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && DataManager.instance._data.skillLevel >= 3)
        {
            if (_bubbleCoolDown <= 0)
            {
                _bubbleCoolDown = 15f;
                GameObject bub = Instantiate(_bubblePrefab, transform.position, Quaternion.identity);
                Destroy(bub, 2f);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && DataManager.instance._data.skillLevel >= 4)
        {
            if (_jangpungCoolDown <= 0)
            {
                _jangpungOn = true;
                _jangpungCoolDown = 13f;
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
                GameObject jang = Instantiate(_jangpungPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
                Destroy(jang, 1f);
                _jangpungOn = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && DataManager.instance._data.skillLevel >= 5)
        {
            if (_mineCoolDown <= 0)
            {
                _mineCoolDown = 30f;
                GameObject mine = Instantiate(_minePrefab, transform.position, Quaternion.identity);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (DataManager.instance._data.resources["food"] >= 1 && _foodCoolDown <= 0 && !_isDying)
            {
                _foodCoolDown = 10f;
                StartCoroutine(Eat());
            }
        }

        #endregion

        // 플레이어의 이동 속도를 설정합니다.
        _playerAgent.speed = _moveSpeed;

        // 플레이어가 움직이지 않으면, Idle 애니메이션을 재생합니다.
        if (_playerAgent.velocity == Vector3.zero)
        {
            _playerAnimator.SetBool("isWalking", false);
        }
        else
        {
            _playerAnimator.SetBool("isWalking", true);
        }

        // 플레이어가 움직이는 방향에 따라, 플레이어를 좌우 반전합니다.
        if (!_isAttacking)
        {
            if (_playerAgent.velocity.x > 0 || _playerRigidbody.velocity.x > 0)
            {
                this.transform.localScale = new Vector3(-2, 2, 1);
            }
            else if (_playerAgent.velocity.x < 0 || _playerRigidbody.velocity.x < 0)
            {
                this.transform.localScale = new Vector3(2, 2, 1);
            }
        }

        // 플레이어의 쿨타임을 감소시킵니다.
        if (_dashCoolDown > 0)
        {
            _dashCoolDown -= Time.deltaTime;
        }
        if (_bubbleCoolDown > 0)
        {
            _bubbleCoolDown -= Time.deltaTime;
        }
        if (_harpoonCoolDown > 0)
        {
            _harpoonCoolDown -= Time.deltaTime;
        }
        if (_jangpungCoolDown > 0)
        {
            _jangpungCoolDown -= Time.deltaTime;
        }
        if (_mineCoolDown > 0)
        {
            _mineCoolDown -= Time.deltaTime;
        }
        if (_foodCoolDown > 0)
        {
            _foodCoolDown -= Time.deltaTime;
        }
        if (_attackCoolDown > 0)
        {
            _attackCoolDown -= Time.deltaTime;
        }

        // 플레이어가 돌진할 수 있는 상태라면, 플레이어의 투명도를 낮추고, 몬스터와의 충돌을 무시합니다.
        if (_isDash)
        {
            Physics2D.IgnoreLayerCollision(6, 7);
            this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 150);
            _playerAgent.enabled = false;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            _playerAgent.enabled = true;
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.25f);
        _playerAttackEffect.SetActive(true);
        _playerAttackEffect.GetComponent<Animator>().Play("Attack");
        Debug.Log("Attack");
    }

    IEnumerator AttackEffect()
    {
        yield return new WaitForSeconds(0.5f);
        _playerAttackEffect.SetActive(false);
        _isAttacking = false;
    }

    public void CallCoroutine()
    {
        StartCoroutine(CheckisDie());
    }

    public void CallComplete()
    {
        StartCoroutine(CheckisComplete());
    }

    public GameObject _completeFade;

    IEnumerator CheckisComplete()
    {
        if (InGameUI.instance._isQuestComplete[0] && InGameUI.instance._isQuestComplete[1] && InGameUI.instance._isQuestComplete[2])
        {
            _playerAgent.isStopped = true;
            _playerRigidbody.bodyType = RigidbodyType2D.Static;
            _completeFade.SetActive(true);
            for (int i = 0; i <= 255; i++)
            {
                _completeFade.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)i);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1f);
            DataManager.instance._data.day += 1;
            DataManager.instance.Save();
            SceneManager.LoadScene("Home");
        }
    }

    IEnumerator Eat()
    {
        DataManager.instance._data.resources["food"] -= 1;
        DataManager.instance.Save();
        for (int i = 0; i < 15; i++)
        {
            if (_playerHp < 100)
            {
                if (_playerHp + 1 >= 100)
                {
                    _playerHp = 100;
                }
                else
                {
                    _playerHp += 1;
                }
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                break;
            }
        }
    }

    public IEnumerator CheckisDie()
    {
        if (_playerHp <= 0)
        {
            _isDying = true;
            _playerAgent.isStopped = true;
            _playerRigidbody.bodyType = RigidbodyType2D.Static;
            _dieFade.SetActive(true);
            for (int i = 0; i <= 255; i++)
            {
                _dieFade.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)i);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSecondsRealtime(2f);
            _gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// 플레이어를 돌진시키는 함수입니다.
    /// </summary>
    public IEnumerator Dash()
    {
        // 플레이어가 돌진할 수 있는 상태라면, 플레이어를 돌진시킵니다.
        if (_dashCoolDown <= 0)
        {
            _dashCoolDown = 7f;
            _isDash = true;

            // 마우스 방향으로 돌진합니다.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePosition - transform.position).normalized;
            _playerRigidbody.velocity = direction * _dashSpeed;

            yield return new WaitForSeconds(0.5f);

            _playerRigidbody.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.3f);
            _isDash = false;
        }
    }
}
