using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Whale : MonsterController
{
    [SerializeField] private float thisSpeed = 2.0f;
    [SerializeField] private int thisAttack = 10;
    [SerializeField] private int thisMaxHp = 50;
    [SerializeField] private float thisAttackCoolTime = 0.1f;

    // 스탯 설정
    protected override void Start()
    {
        speed = thisSpeed;
        attack = thisAttack;
        maxHp = thisMaxHp;
        attackCoolTime = thisAttackCoolTime;
        base.Start();
    }

    // 공격
    protected override void Update()
    {
        if (!_isKnockback)
        {
            _agent.speed = speed;
        }
        base.Update();
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {

    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (attackCurrentTime >= attackCoolTime)
            {
                other.gameObject.GetComponent<PlayerController>()._playerHp -= attack;
                PlayerController.instance.CallCoroutine();
                attackCoolTime = thisAttackCoolTime;
                attackCurrentTime = 0;
            }
        }
    }
}