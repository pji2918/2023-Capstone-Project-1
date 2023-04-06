using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    NavMeshAgent _playerAgent;
    [SerializeField] private float _moveSpeed = 5f;

    // 플레이어의 NavMeshAgent 컴포넌트를 가져옵니다.
    void Start()
    {
        // 플레이어의 NavMesh 에이전트 컴포넌트를 가져오고, 자동 회전을 끕니다.
        _playerAgent = GetComponent<NavMeshAgent>();
        _playerAgent.updateRotation = false;
        _playerAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        _playerAgent.speed = _moveSpeed;

        // 오른쪽 마우스 클릭을 하면 클릭한 위치로 이동합니다.
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse Clicked");
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _playerAgent.SetDestination(mousePosition);
        }
    }
}
