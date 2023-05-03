using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFish : MonsterController
{
    [SerializeField] private float stopTime = 2.5f;
    private float stopCurrentTime;
    private bool isStop = false;

    [Space(10f)]

    [SerializeField] private float thisSpeed = 8.0f;
    [SerializeField] private int thisAttack = 10;
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

        if (isStop)
        {
            stopCurrentTime += Time.deltaTime;
        }

        if (stopCurrentTime > stopTime && isStop)
        {
            isStop = false;
            PlayerController.instance._moveSpeed = 5f;
        }
    }

    public void Attack()
    {
        if (isStop)
        {
            stopCurrentTime = 0;
            Debug.Log("경직 시간 초기화");
        }
        else
        {
            StartCoroutine(StopPlayer());
        }
    }

    IEnumerator StopPlayer()
    {
        stopCurrentTime = 0;
        isStop = true;
        PlayerController.instance._moveSpeed = 0f;
        yield return null;
    }
}
