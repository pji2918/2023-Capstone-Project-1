using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Axolotl : MonsterController
{
    [SerializeField] private float thisSpeed = 1.0f;
    [SerializeField] private int thisAttack = 10;
    [SerializeField] private int thisMaxHp = 50;
    [SerializeField] private float thisAttackCoolTime = 3f;

    [SerializeField] private float dashCoolTime = 10f;
    [SerializeField] private float dashTime = 1.5f;

    [SerializeField] private float dashPower = 5.0f;

    [SerializeField] private GameObject poisonPrefab;

    [SerializeField] private Ease ease;

    [SerializeField] private bool isDash;
    private Rigidbody2D rb;

    [SerializeField] private int dashAttack;

    private Collider2D axolotlCollider;
    private SpriteRenderer spriteRenderer;

    private Slider hpBar;
    private GameObject hpBarObj;

    // 스탯 설정
    protected override void Start()
    {
        InGameUI.instance._isBoss = true;

        hpBarObj = GameObject.Find("Canvas").transform.GetChild(4).GetChild(1).gameObject;
        InGameUI.instance._timerText.gameObject.SetActive(false);
        hpBarObj.SetActive(true);
        hpBar = hpBarObj.transform.GetChild(1).GetComponent<Slider>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        axolotlCollider = gameObject.GetComponent<Collider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed = thisSpeed;
        attack = thisAttack;
        maxHp = thisMaxHp;
        attackCoolTime = thisAttackCoolTime;
        base.Start();

        StartCoroutine(Dash());
        StartCoroutine(Poison());
    }

    void OnDestroy()
    {
        hpBarObj.SetActive(false);
        InGameUI.instance._timerText.gameObject.SetActive(true);
        InGameUI.instance._isBoss = false;
    }

    protected override void Update()
    {
        hpBar.value = (float)Hp / maxHp;

        if (_agent.enabled)
        {
            if (_agent.remainingDistance < _attackRange && !axolotlCollider.isTrigger)
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
            base.Update();
        }
    }

    public void Attack()
    {
        PlayerController.instance._playerHp -= attack;
    }

    public IEnumerator Poison()
    {
        if (!isDash)
        {
            GameObject poison = Instantiate(poisonPrefab, transform.position, transform.rotation);

        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(Poison());
    }

    public IEnumerator Dash()
    {
        GameObject player = PlayerController.instance.gameObject;
        speed = 0;

        isDash = true;
        axolotlCollider.isTrigger = true;
        _agent.enabled = false;
        spriteRenderer.color = new Color32(255, 255, 255, 150);

        for (int i = 0; i < 3; i++)
        {

            yield return new WaitForSeconds(dashTime);

            Vector2 dir = (player.transform.position - transform.position).normalized * dashPower;
            dir = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);

            if (dir.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (dir.x < 0)
            {
                spriteRenderer.flipX = false;
            }

            //transform.position = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
            rb.DOMove(dir, 1f).SetEase(ease);
            yield return new WaitForSeconds(0.5f);

            //DOTween.Init();
        }

        isDash = false;
        axolotlCollider.isTrigger = false;
        _agent.enabled = true;
        spriteRenderer.color = new Color32(255, 255, 255, 255);

        speed = thisSpeed;
        yield return new WaitForSeconds(dashCoolTime);

        StartCoroutine(Dash());
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.CallCoroutine();
            PlayerController.instance._playerHp -= dashAttack;
        }

        base.OnTriggerEnter2D(other);
    }
}