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
    [SerializeField] private float thisAttackCoolTime = 1f;

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
        if (_agent.enabled)
        {
            if (_agent.remainingDistance < 2)
            {
                _agent.speed = 0;
                if (!_isAttack)
                {
                    StartCoroutine(DOTDamage());
                }
            }
            else
            {
                _agent.speed = speed;
            }
        }
        base.Update();
    }

    private bool _isAttack = false;

    IEnumerator DOTDamage()
    {
        _isAttack = true;
        PlayerController.instance._playerHp -= attack;
        PlayerController.instance.CallCoroutine();
        yield return new WaitForSeconds(0.8f);
        _isAttack = false;
    }
}
