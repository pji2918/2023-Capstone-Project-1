using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MuteBear : MonsterController
{
    [SerializeField] private float thisSpeed = 2.0f;
    [SerializeField] private int thisAttack = 10;
    [SerializeField] private int thisMaxHp = 50;
    [SerializeField] private float thisAttackCoolTime = 3f;

    [Space(10f)]

    [SerializeField] private GameObject stonePrefab;

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
            if (_agent.remainingDistance < 6)
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
        base.Update();
    }

    public void Attack()
    {
        GameObject stone = Instantiate(stonePrefab, transform.position, transform.rotation);

        float angle = Mathf.Atan2(PlayerController.instance.transform.position.y - stone.transform.position.y,
                            PlayerController.instance.transform.position.x - stone.transform.position.x)
              * Mathf.Rad2Deg;
        stone.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
