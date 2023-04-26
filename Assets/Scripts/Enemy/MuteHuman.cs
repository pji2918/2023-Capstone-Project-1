using System.Collections;
using UnityEngine;

public class MuteHuman : MonsterController
{
    [SerializeField] private float thisSpeed = 2.0f;
    [SerializeField] private int thisAttack = 10;
    [SerializeField] private int thisMaxHp = 50;
    [SerializeField] private float thisAttackCoolTime = 3f;

    // 스탯 설정
    protected override void Start()
    {
        base.Start();
        speed = thisSpeed;
        attack = thisAttack;
        maxHp = thisMaxHp;
        attackCoolTime = thisAttackCoolTime;
    }

    // 공격
    protected override void Update()
    {
        base.Update();
        if (_agent.remainingDistance < 2)
        {
            _agent.speed = 0;
            if (attackCurrentTime >= attackCoolTime)
            {
                Attack();
                attackCurrentTime = 0;
            }
        }
        else
        {
            _agent.speed = speed;
        }
    }

    public void Attack()
    {
        PlayerController.Instance._playerHp -= attack;
    }
}