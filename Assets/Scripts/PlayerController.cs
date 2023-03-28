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
        _playerAgent = GetComponent<NavMeshAgent>();
        _playerAgent.updateRotation = false;
        _playerAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Mouse Clicked");
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _playerAgent.SetDestination(mousePosition);
        }
    }
}
