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

    // 플레이어 컨트롤러에 필요한 컴포넌트를 선언합니다.
    private NavMeshAgent _playerAgent;
    private Rigidbody2D _playerRigidbody;
    private Animator _playerAnimator;

    // 플레이어 컨트롤러에 필요한 컴포넌트를 가져오는 프로퍼티를 선언합니다.
    public NavMeshAgent _PlayerAgent { get => _playerAgent; }
    public Rigidbody2D _PlayerRigidbody { get => _playerRigidbody; }
    public Animator _PlayerAnimator { get => _playerAnimator; }

    // 플레이어의 이동 속도 및 돌진 속도, 쿨타임을 저장하는 변수입니다.
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _dashSpeed;
    public float _dashCoolDown;
    public bool _isDash = false;

    [Range(0, 100)] public int _playerHp = 100;

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
