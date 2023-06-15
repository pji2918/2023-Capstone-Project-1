using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KrakenUpLeg : MonsterController
{
    [SerializeField] private float thisSpeed = 0f;
    [SerializeField] private int thisAttack = 10;
    [SerializeField] private int thisMaxHp = 50;
    [SerializeField] private float thisAttackCoolTime = 3f;

    // 스탯 설정
    protected override void Start()
    {
        speed = thisSpeed;
        attack = thisAttack;
        maxHp = thisMaxHp;
        attackCoolTime = thisAttackCoolTime;

        doRotate = false;

        currentHp = maxHp;
    }

    // 공격
    protected override void Update()
    {
        attackCurrentTime += Time.deltaTime;

        if (currentHp <= 0)
        {
            Instantiate(_dieEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }
        if (PlayerController.instance._isAttackedByKraken && attackCurrentTime >= attackCoolTime)
        {
            Attack();
        }
    }

    public void Attack()
    {
        PlayerController.instance.CallCoroutine();
        PlayerController.instance._playerHp -= attack;
        attackCoolTime = thisAttackCoolTime;
        attackCurrentTime = 0;
    }
}
