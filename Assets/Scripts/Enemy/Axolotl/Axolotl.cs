using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axolotl : MonsterController
{
    [SerializeField] private float thisSpeed = 1.0f;
    [SerializeField] private int thisAttack = 10;
    [SerializeField] private int thisMaxHp = 50;
    [SerializeField] private float thisAttackCoolTime = 3f;

    [SerializeField] private float dashCoolTime = 10f;
    [SerializeField] private float dashCurrentTime;
    [SerializeField] private float dashTime = 1.5f;

    [SerializeField] private float dashPower = 5.0f;

    private Rigidbody2D rb;

    // 스탯 설정
    protected override void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed = thisSpeed;
        attack = thisAttack;
        maxHp = thisMaxHp;
        attackCoolTime = thisAttackCoolTime;
        base.Start();

        StartCoroutine(Dash());
    }

    void Update()
    {
        if (_agent.enabled)
        {
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
        base.Update();
    }

    public void Attack()
    {
        PlayerController.instance.CallCoroutine();
        PlayerController.instance._playerHp -= attack;
    }

    public IEnumerator Dash()
    {
        GameObject player = PlayerController.instance.gameObject;
        speed = 0;

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(2.0f);
            Vector2 dir = (player.transform.position - transform.position).normalized;

            Debug.Log("dol");
            rb.AddForce(dir * dashPower, ForceMode2D.Impulse);
        }
    }
}