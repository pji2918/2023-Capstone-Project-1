using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Attack()
    {
        GameObject stone = Instantiate(stonePrefab, transform.position, transform.rotation);

        float angle = Mathf.Atan2(PlayerController.Instance.transform.position.y - stone.transform.position.y,
                            PlayerController.Instance.transform.position.x - stone.transform.position.x)
              * Mathf.Rad2Deg;
        stone.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
