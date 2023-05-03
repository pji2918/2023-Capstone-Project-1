using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public int _playerMaxHp = 100;

    [Range(0, 100)] public int _playerHp;

    // 플레이어의 NavMeshAgent 컴포넌트를 가져옵니다.
    void Start()
    {
        // 플레이어의 NavMesh 에이전트 컴포넌트를 가져오고, 자동 회전을 끕니다.
        _playerAgent = GetComponent<NavMeshAgent>();
        _playerAgent.updateRotation = false;
        _playerAgent.updateUpAxis = false;

        // 플레이어의 Rigidbody2D 컴포넌트를 가져옵니다.
        _playerRigidbody = GetComponent<Rigidbody2D>();

        // 플레이어의 Animator 컴포넌트를 가져옵니다.
        _playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        #region 키 입력
        // 오른쪽 마우스 클릭을 하면 플레이어를 클릭한 위치로 이동시킵니다.
        if (Input.GetMouseButtonDown(1) && !PlayerController.instance._isDash)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlayerController.instance._playerAgent.SetDestination(mousePosition);
        }

        // 왼쪽 Shift 키를 누르면 플레이어를 돌진시키는 함수를 실행합니다.
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(PlayerController.instance.Dash());
        }

        if (Input.GetMouseButtonDown(0))
        {
            _playerAttackEffect.SetActive(true);
            _playerAttackEffect.GetComponent<Animator>().Play("Attack");
            // 애니메이션이 끝나면, 플레이어의 공격 이펙트를 비활성화합니다.
            StartCoroutine(AttackEffect());
        }

        if (Input.GetKeyDown(KeyCode.Q))
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

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_bubbleCoolDown <= 0)
            {
                _bubbleCoolDown = 15f;
                GameObject bub = Instantiate(_bubblePrefab, transform.position, Quaternion.identity);
                Destroy(bub, 2f);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_mineCoolDown <= 0)
            {
                _mineCoolDown = 30f;
                GameObject mine = Instantiate(_minePrefab, transform.position, Quaternion.identity);
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
        if (_playerAgent.velocity.x > 0 || _playerRigidbody.velocity.x > 0)
        {
            this.transform.localScale = new Vector3(-2, 2, 1);
        }
        else if (_playerAgent.velocity.x < 0 || _playerRigidbody.velocity.x < 0)
        {
            this.transform.localScale = new Vector3(2, 2, 1);
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

    IEnumerator AttackEffect()
    {
        yield return new WaitForSeconds(0.5f);
        _playerAttackEffect.SetActive(false);
    }

    /// <summary>
    /// 플레이어를 돌진시키는 함수입니다.
    /// </summary>
    public IEnumerator Dash()
    {
        // 플레이어가 돌진할 수 있는 상태라면, 플레이어를 돌진시킵니다.
        if (_dashCoolDown <= 0)
        {
            _dashCoolDown = 10f;
            _isDash = true;

            // 마우스 방향으로 돌진합니다.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePosition - transform.position).normalized;
            _playerRigidbody.velocity = direction * _dashSpeed;

            yield return new WaitForSeconds(0.5f);

            _playerRigidbody.velocity = Vector2.zero;
            _isDash = false;
        }
    }
}
