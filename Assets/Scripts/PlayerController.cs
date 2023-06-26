using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

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

    public float slowCurrentTime;
    public bool isSlow = false;

    // 플레이어의 이동 속도 및 돌진 속도, 쿨타임을 저장하는 변수입니다.
    [SerializeField] public float _moveSpeed = 5f;
    [SerializeField] private float _dashSpeed;
    public float _dashCoolDown;
    public bool _isDash = false;

    public bool _isInvincible = false;
    public bool _isPowerUp = false;

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
    public int _playerMaxHp;
    public int _playerHp;
    public int _playerAtk;
    public double _skillDamageMultiplier = 0;

    public float _timer = 0f;

    private float _attackCoolDown = 0f;

    public GameObject _playerArrow;

    public CinemachineImpulseSource _impulseSource;

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

        switch (DataManager.instance._data.buildLevel) // 건물 레벨에 따라 플레이어의 최대 체력과 이동 속도를 설정합니다.
        {
            case 0:
                {
                    _playerMaxHp = 100;
                    _moveSpeed = 5f;
                    break;
                }
            case 1:
                {
                    _playerMaxHp = 105;
                    _moveSpeed = 5.2f;
                    break;
                }
            case 2:
                {
                    _playerMaxHp = 115;
                    _moveSpeed = 5.5f;
                    break;
                }
            case 3:
                {
                    _playerMaxHp = 120;
                    _moveSpeed = 5.7f;
                    break;
                }
            case 4:
                {
                    _playerMaxHp = 125;
                    _moveSpeed = 5.9f;
                    break;
                }
            case 5:
                {
                    _playerMaxHp = 135;
                    _moveSpeed = 6.3f;
                    break;
                }
            case 6:
                {
                    _playerMaxHp = 140;
                    _moveSpeed = 6.5f;
                    break;
                }
            case 7:
                {
                    _playerMaxHp = 145;
                    _moveSpeed = 6.7f;
                    break;
                }
            case 8:
                {
                    _playerMaxHp = 155;
                    _moveSpeed = 7f;
                    break;
                }
            case 9:
                {
                    _playerMaxHp = 160;
                    _moveSpeed = 7.2f;
                    break;
                }
            case 10:
                {
                    _playerMaxHp = 180;
                    _moveSpeed = 7.5f;
                    break;
                }
        }

        switch (DataManager.instance._data.skillLevel) // 스킬 레벨에 따라 플레이어의 공격력을 설정합니다.
        {
            case 0:
                {
                    _playerAtk = 10;
                    _skillDamageMultiplier = 0;
                    break;
                }
            case 1:
                {
                    _playerAtk = 11;
                    _skillDamageMultiplier = 0.2;
                    break;
                }
            case 2:
                {
                    _playerAtk = 12;
                    _skillDamageMultiplier = 0.2;
                    break;
                }
            case 3:
                {
                    _playerAtk = 14;
                    _skillDamageMultiplier = 0.3;
                    break;
                }
            case 4:
                {
                    _playerAtk = 15;
                    _skillDamageMultiplier = 0.5;
                    break;
                }
            case 5:
                {
                    _playerAtk = 16;
                    _skillDamageMultiplier = 0.5;
                    break;
                }
            case 6:
                {
                    _playerAtk = 18;
                    _skillDamageMultiplier = 1;
                    break;
                }
            case 7:
                {
                    _playerAtk = 19;
                    _skillDamageMultiplier = 1;
                    break;
                }
            case 8:
                {
                    _playerAtk = 20;
                    _skillDamageMultiplier = 1;
                    break;
                }
            case 9:
                {
                    _playerAtk = 23;
                    _skillDamageMultiplier = 1.5;
                    break;
                }
            case 10:
                {
                    _playerAtk = 25;
                    _skillDamageMultiplier = 1.5;
                    break;
                }
            case 11:
                {
                    _playerAtk = 28;
                    _skillDamageMultiplier = 1.7;
                    break;
                }
            case 12:
                {
                    _playerAtk = 30;
                    _skillDamageMultiplier = 2;
                    break;
                }
            case 13:
                {
                    _playerAtk = 33;
                    _skillDamageMultiplier = 2;
                    break;
                }
            case 14:
                {
                    _playerAtk = 36;
                    _skillDamageMultiplier = 2;
                    break;
                }
            case 15:
                {
                    _playerAtk = 40;
                    _skillDamageMultiplier = 2.5;
                    break;
                }
            default:
                {
                    _playerAtk = 0;
                    _skillDamageMultiplier = 0;
                    break;
                }
        }

        if (GameManager.instance is not null)
        {
            Debug.Log(GameManager.instance._healthReduce);
            if (GameManager.instance._healthReduce > 0)
            {
                // Player의 HP를 healthReduce의 백분율만큼 감소시킵니다.
                _playerHp = _playerMaxHp - (int)(_playerMaxHp * (GameManager.instance._healthReduce / 100f));
            }
            else
            {
                _playerHp = _playerMaxHp;
            }
        }
        else
        {
            _playerHp = _playerMaxHp;
        }

        StartCoroutine(Fade());

        SoundManager.instance.PlayMusic(SoundManager.instance.GetAudioClip(SoundManager.AudioClips.BeYourSelf));
    }

    public bool _isFinishing = false;
    public float _foodTimer = 0f;

    public GameObject _clickAnimation;

    // Update is called once per frame
    void Update()
    {
        #region 키 입력
        // 오른쪽 마우스 클릭을 하면 플레이어를 클릭한 위치로 이동시킵니다.
        if (Input.GetMouseButtonDown(1) && !_isDash && !_isDying && !_isFinishing)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(_clickAnimation, mousePosition, Quaternion.identity);
            PlayerController.instance._playerAgent.SetDestination(mousePosition);
        }

        // 왼쪽 Shift 키를 누르면 플레이어를 돌진시키는 함수를 실행합니다.
        if (Input.GetKeyDown(KeyCode.LeftShift) && DataManager.instance._data.skillLevel >= 1)
        {
            StartCoroutine(PlayerController.instance.Dash());
        }

        if (_playerAgent.velocity.magnitude > 0.1f)
        {
            _playerArrow.SetActive(true);
            _playerArrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, _playerAgent.velocity);
        }
        else
        {
            _playerArrow.SetActive(false);
        }



        if (!_isDash)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerAgent.isStopped = true;
                _playerAgent.velocity = Vector2.zero;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                _playerAgent.isStopped = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!_isDying && _attackCoolDown <= 0f)
            {
                _attackCoolDown = 0.7f;
                _isAttacking = true;

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
                GameObject harpoon = Instantiate(_harpoonPrefab, transform.position, Quaternion.AngleAxis(angle - 90, Vector3.forward));
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
                GameObject jang = Instantiate(_jangpungPrefab, transform.position, Quaternion.AngleAxis(angle - 90, Vector3.forward));
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
                _foodTimer = 4.5f;
                _foodCoolDown = 10f;
                StartCoroutine(Eat());
            }
        }

        #endregion

        if (_foodTimer > 0)
        {
            _foodTimer -= Time.deltaTime;
        }
        else
        {
            _foodTimer = 0;
        }

        // 플레이어의 이동 속도를 설정합니다.
        _playerAgent.speed = _moveSpeed;

        if (_timer >= 180 && !_isFinishing)
        {
            CallComplete();
        }
        else
        {
            _timer += Time.deltaTime;
        }

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
            if (_playerAgent.velocity.x > 1 || _playerRigidbody.velocity.x > 1)
            {
                this.transform.localScale = new Vector3(-2, 2, 1);
            }
            else if (_playerAgent.velocity.x < 1 || _playerRigidbody.velocity.x < 1)
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

        if (isSlow)
        {
            slowCurrentTime += Time.deltaTime;
        }

        if (slowCurrentTime > _slowTime && isSlow)
        {
            isSlow = false;
            PlayerController.instance._moveSpeed = 5f;
        }
    }

    public float _slowTime;

    public void OnDamage()
    {
        if (DataManager.instance._data.isScreenVibration)
        {
            StartCoroutine(CameraMovement());
        }
    }

    public IEnumerator CameraMovement()
    {
        _impulseSource.GenerateImpulse();
        yield return new WaitForSeconds(0.5f);
        CinemachineImpulseManager.Instance.Clear();
    }

    public IEnumerator SlowPlayer(float stopTime)
    {
        _slowTime = stopTime;
        slowCurrentTime = 0;
        isSlow = true;
        PlayerController.instance._moveSpeed = 2f;
        yield return null;
    }

    public void Attack()
    {
        _playerAttackEffect.SetActive(true);
        _playerAttackEffect.GetComponent<Animator>().Play("Attack");
    }

    IEnumerator AttackEffect()
    {
        yield return new WaitForSeconds(0.5f);
        _playerAttackEffect.SetActive(false);
        _isAttacking = false;
    }

    IEnumerator Fade()
    {
        _completeFade.SetActive(true);
        for (int i = 255; i >= 0; i--)
        {
            _completeFade.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)i);
            yield return new WaitForSeconds(0.000001f);
        }
        _completeFade.SetActive(false);
    }

    public void CallCoroutine()
    {
        if (!_isDying)
        {
            StartCoroutine(InGameUI.instance.CheckisDie());
        }
    }

    public void CallComplete()
    {
        StartCoroutine(CheckisComplete());
    }

    public GameObject _completeFade;

    IEnumerator CheckisComplete()
    {
        if (_timer >= 180
        || (InGameUI.instance._isQuestComplete[0]
        && InGameUI.instance._isQuestComplete[1]
        && InGameUI.instance._isQuestComplete[2]
        && InGameUI.instance._isQuestComplete[3]
        && InGameUI.instance._isQuestComplete[4]))
        {
            _isFinishing = true;
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
            SceneManager.LoadScene("Loading_ToHome");
        }
    }

    IEnumerator Eat()
    {
        DataManager.instance._data.resources["food"] -= 1;
        DataManager.instance.Save();
        for (int i = 0; i < 15; i++)
        {
            if (_playerHp < _playerMaxHp)
            {
                if (_playerHp + (int)(_playerMaxHp * 0.01) >= _playerMaxHp)
                {
                    _playerHp = _playerMaxHp;
                }
                else
                {
                    _playerHp += (int)(_playerMaxHp * 0.01);
                }
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                break;
            }
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

            Physics2D.IgnoreLayerCollision(6, 7);
            this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 150);
            _playerAgent.isStopped = true;

            // 마우스 방향으로 돌진합니다.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mousePosition - transform.position).normalized;
            _playerRigidbody.velocity = direction * _dashSpeed;

            yield return new WaitForSeconds(0.5f);

            _playerRigidbody.velocity = Vector2.zero;
            this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            _playerAgent.isStopped = false;
            _isDash = false;
        }
    }
}
