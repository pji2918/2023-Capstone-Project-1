using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piranha : MonsterController
{
    [SerializeField] private float thisSpeed = 2.0f;
    [SerializeField] private int thisAttack = 10;
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
        if (this._agent.enabled)
        {
            if (Vector3.Distance(transform.position, _playerPosition) < _attackRange && !PlayerController.instance._isDash &&
            !PlayerController.instance._isFinishing && !PlayerController.instance._isInvincible)
            {
                this._agent.speed = 0;
                if (this.attackCurrentTime >= attackCoolTime)
                {
                    Attack();
                    this.attackCurrentTime = 0;
                }
            }
            else
            {
                this._agent.speed = speed;
            }
        }
        base.Update();
    }

    public void Attack()
    {
        PlayerController.instance.OnDamage();
        PlayerController.instance._playerHp -= attack;
    }

    // void OnCollisionStay2D(Collision2D other)
    // {
    //     if (other.gameObject.tag == "Player" && !PlayerController.instance._isDash && !PlayerController.instance._isFinishing)
    //     {
    //         if (attackCurrentTime >= attackCoolTime)
    //         {
    //             other.gameObject.GetComponent<PlayerController>()._playerHp -= attack;
    //             PlayerController.instance.CallCoroutine();
    //             attackCoolTime = thisAttackCoolTime;
    //             attackCurrentTime = 0;
    //         }
    //     }
    // }
}
