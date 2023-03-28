using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public Vector2 _playerPosition;
    NavMeshAgent _agent;

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
        _playerPosition = GameObject.Find("Player").transform.position;
        _agent.SetDestination(_playerPosition);
    }
}
