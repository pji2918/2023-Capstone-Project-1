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
            if (Vector2.Distance(transform.position, _playerPosition) < 2 && !PlayerController.instance._isDash &&
            !PlayerController.instance._isFinishing && !PlayerController.instance._isInvincible)
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
        PlayerController.instance.OnDamage();
        PlayerController.instance.CallCoroutine();
        PlayerController.instance._playerHp -= attack;
    }
}
