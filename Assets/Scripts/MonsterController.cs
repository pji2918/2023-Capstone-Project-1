using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [Tooltip("몬스터가 추적할 플레이어의 위치입니다.")] public Vector2 _playerPosition;
    NavMeshAgent _agent;
    private bool _isKnockback = false;

    // NavMesh 에이전트의 회전과 Z축을 고정시킵니다.
    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    // 플레이어의 위치를 변수에 저장한 다음, 에이전트의 목적지를 플레이어의 위치로 설정합니다.
    void Update()
    {
        if (!_isKnockback)
        {
            _playerPosition = GameObject.Find("Player").transform.position;
            _agent.SetDestination(_playerPosition);
        }
        if (!PlayerController.instance._jangpungOn)
        {
            _isKnockback = false;
            this.GetComponent<NavMeshAgent>().enabled = true;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bubble"))
        {
            // 여기에 몬스터가 비눗방울에 닿았을 때 HP가 서서히 감소하는 코드를 작성합니다.
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Harpoon"))
        {
            // 여기에 몬스터가 작살에 닿았을 때 HP가 감소하는 코드를 작성합니다.
        }
        if (other.name == "Attack_Effect")
        {
            // 여기에 기본 공격에 닿았을 때 HP가 감소하는 코드를 작성합니다.
        }
        if (other.CompareTag("Mine_Explosion"))
        {
            // 여기에 지뢰에 닿았을 때 HP가 감소하는 코드를 작성합니다.
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Jangpung"))
        {
            _isKnockback = true;
            this.GetComponent<NavMeshAgent>().enabled = false;
            // 여기에 몬스터가 장풍에 닿았을 때 HP가 감소하는 코드를 작성합니다.
        }
    }
}
