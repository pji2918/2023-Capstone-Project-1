using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _playerRigidbody;
    [SerializeField] private float _moveSpeed = 5f;

    // 플레이엉의 리지드바디를 가져옵니다.
    void Start()
    {
        _playerRigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // X축과 Y축의 입력을 받아서, 플레이어의 리지드바디의 속도를 설정합니다.
    void FixedUpdate()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        _playerRigidbody.velocity = new Vector2(xInput, yInput) * _moveSpeed;
    }
}
