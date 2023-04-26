using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFish : MonsterController
{
    private float stopCurrentTime;
    private bool isStop = false;

    [SerializeField] private float thisSpeed = 8.0f;
    [SerializeField] private int thisAttack = 1;
    [SerializeField] private int thisMaxHp = 50;
    [SerializeField] private float thisAttackCoolTime = 5f;

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
        StartCoroutine(DOTDamage());
    }

    IEnumerator DOTDamage()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerController.Instance._playerHp -= attack;
            yield return new WaitForSeconds(1f);
        }
    }
}
