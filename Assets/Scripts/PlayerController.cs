using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    NavMeshAgent _playerAgent;
    Rigidbody2D _playerRigidbody;
    Animator _playerAnimator;

    [SerializeField] private float _moveSpeed = 5f;

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
        _playerAgent.speed = _moveSpeed;

        // 오른쪽 마우스 클릭을 하면 클릭한 위치로 이동합니다.
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _playerAgent.SetDestination(mousePosition);
        }

        if (_playerAgent.velocity == Vector3.zero)
        {
            _playerAnimator.SetBool("isWalking", false);
        }
        else
        {
            _playerAnimator.SetBool("isWalking", true);
        }

        if (_playerAgent.velocity.x > 0)
        {
            this.transform.localScale = new Vector3(-3, 3, 1);
        }
        else if (_playerAgent.velocity.x < 0)
        {
            this.transform.localScale = new Vector3(3, 3, 1);
        }
    }
}
