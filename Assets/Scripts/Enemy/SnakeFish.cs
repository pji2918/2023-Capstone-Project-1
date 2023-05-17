using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFish : MonsterController
{
    [SerializeField] private float stopTime = 2.5f;


    [Space(10f)]

    [SerializeField] private float thisSpeed = 8.0f;
    [SerializeField] private int thisAttack = 10;
    [SerializeField] private int thisMaxHp = 50;
    [SerializeField] private float thisAttackCoolTime = 5f;

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
            if (_agent.remainingDistance < 2)
            {
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
        base.Update();
    }

    public void Attack()
    {
        if (PlayerController.instance.isStop)
        {
            PlayerController.instance.stopCurrentTime = 0;
            Debug.Log("경직 시간 초기화");
        }
        else
        {
            StartCoroutine(PlayerController.instance.StopPlayer(stopTime));
        }
    }


}
