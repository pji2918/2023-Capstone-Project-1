using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using TMPro;

public class Whale : MonsterController
{
    [SerializeField] private float thisSpeed = 2.0f;
    [SerializeField] private int thisAttack = 10;
    [SerializeField] private int thisMaxHp = 1200;
    [SerializeField] private float thisAttackCoolTime = 0.1f;

    [SerializeField] private GameObject bulletPrefab;

    [Space(10)]
    [SerializeField] private float maxCoolTime;
    [SerializeField] private float minCoolTime;
    private float currentCoolTime;

    private Slider hpBar;
    private GameObject hpBarObj;

    private Animator whaleAnimator;

    private bool isAttack = false;

    [SerializeField] private GameObject point;

    // 스탯 설정
    protected override void Start()
    {
        whaleAnimator = gameObject.GetComponent<Animator>();

        hpBarObj = GameObject.Find("Canvas").transform.GetChild(4).GetChild(1).gameObject;
        InGameUI.instance._timerText.gameObject.SetActive(false);
        hpBarObj.SetActive(true);
        hpBar = hpBarObj.transform.GetChild(1).GetComponent<Slider>();

        hpBarObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("BOSS: {0}", LocalizationSettings.StringDatabase.GetLocalizedString("UI", "boss_whale"));

        InGameUI.instance._isBoss = true;

        speed = thisSpeed;
        attack = thisAttack;
        maxHp = thisMaxHp;
        attackCoolTime = thisAttackCoolTime;

        StartCoroutine(Shot());
        StartCoroutine(AttackOnOff());

        base.Start();
    }

    void OnDestroy()
    {
        hpBarObj.SetActive(false);
        InGameUI.instance._timerText.gameObject.SetActive(true);
        InGameUI.instance._isBoss = false;
    }

    // 공격
    protected override void Update()
    {
        if (hpBar is not null)
        {
            hpBar.value = (float)Hp / maxHp;
        }

        _agent.speed = speed;

        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {

    }

    IEnumerator Shot()
    {
        currentCoolTime = Random.Range(minCoolTime, maxCoolTime);

        yield return new WaitForSeconds(currentCoolTime);

        int shootType = Random.Range(1, 4);

        if (isAttack)
        {
            switch (shootType)
            {
                case 1:
                    StartCoroutine(BuchaeShot(3, 60, BulletType.normal));
                    break;
                case 2:
                    StartCoroutine(RotateShot(10, 3, BulletType.normal, 0));
                    break;
                case 3:
                    StartCoroutine(RotateShot(16, 0, BulletType.normal, 0));
                    break;
            }
        }

        StartCoroutine(Shot());
    }

    IEnumerator AttackOnOff()
    {
        yield return new WaitForSeconds(10f);
        whaleAnimator.SetBool("isAttack", false);
        isAttack = false;

        yield return new WaitForSeconds(10f);
        SoundManager.instance.PlayMonsterEffects(this.gameObject, SoundManager.instance.GetAudioClip(SoundManager.AudioClips.Whale_StartAttack));
        whaleAnimator.SetBool("isAttack", true);
        isAttack = true;

        StartCoroutine(AttackOnOff());
    }

    #region shoot
    IEnumerator SpinShot(int bulletCount, BulletType bulletType, int dir)
    {
        for (int i = 0; i < 360; i += (dir * 360) / bulletCount)
        {
            StartCoroutine(RotateShot(50, 5, bulletType, i));
        }

        yield return new WaitForSeconds(5f);
        StartCoroutine(SpinShot(bulletCount, bulletType, dir));
    }

    IEnumerator RotateShot(int bulletCount, float shotTime, BulletType bulletType, float startAngle)
    {
        // bulletCount = 총알 갯수
        // shotTime = 돌면서 발사하는 시간
        // bulletType = 총알 타입

        for (int i = 0; i < 360; i += 360 / bulletCount)
        {
            GameObject bullet = Instantiate(bulletPrefab, point.transform.position, Quaternion.Euler(0, 0, i + startAngle));

            switch (bulletType)
            {
                case BulletType.normal:
                    bullet.GetComponent<Bullet>().Shoot(BulletType.normal);
                    break;
                case BulletType.bomb:
                    bullet.GetComponent<Bullet>().Shoot(BulletType.bomb);
                    break;
            }

            if (shotTime > 0)
            {
                yield return new WaitForSeconds(shotTime / bulletCount);
            }
        }

        yield return new WaitForSeconds(3.0f);
        //StartCoroutine(RotateShot(bulletCount, shotTime, bulletType));
    }

    IEnumerator BuchaeShot(int bulletCount, float shotAngle, BulletType bulletType)
    {
        // bulletCount = 총알 갯수
        // shotAngle = 부채꼴 각도
        // bulletType = 총알 타입

        float angle = AngleFunc(transform.position, PlayerController.instance.transform.position);
        float currentAngle = 0;

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, point.transform.position,
                Quaternion.Euler(0, 0, angle - (currentAngle - (shotAngle / 2)) + 90));

            switch (bulletType)
            {
                case BulletType.normal:
                    bullet.GetComponent<Bullet>().Shoot(BulletType.normal);
                    break;
                case BulletType.bomb:
                    bullet.GetComponent<Bullet>().Shoot(BulletType.bomb);
                    break;
            }

            Debug.Log((currentAngle - (shotAngle / 2)));
            currentAngle += shotAngle / (bulletCount - 1);
        }

        //Debug.Log(angle);
        yield return new WaitForSeconds(7.0f);
        StartCoroutine(BuchaeShot(bulletCount, shotAngle, bulletType));
    }
    #endregion

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !PlayerController.instance._isDash &&
        !PlayerController.instance._isFinishing && !PlayerController.instance._isInvincible)
        {
            if (attackCurrentTime >= attackCoolTime)
            {
                PlayerController.instance.OnDamage();
                other.gameObject.GetComponent<PlayerController>()._playerHp -= attack;
                PlayerController.instance.CallCoroutine();
                attackCoolTime = thisAttackCoolTime;
                attackCurrentTime = 0;
            }
        }
    }

    // 플레이어를 바라보는 각도를 구하는 함수
    public float AngleFunc(Vector2 self, Vector2 target)
    {
        float angle;

        Vector2 dir = target - self;

        angle = Mathf.Atan2(dir.y, dir.x);
        angle = angle * Mathf.Rad2Deg;

        float baseAngle = 90.0f; // 왼쪽을 기준으로 합니다.
        float maxAngle = 360.0f; // 180도에서 끝나도록 합니다.

        // 입력된 각도를 변환합니다.
        float convertedAngle = (angle - baseAngle) % 360.0f;
        if (convertedAngle < 0)
        {
            convertedAngle += 360.0f;
        }
        if (convertedAngle > maxAngle)
        {
            convertedAngle = maxAngle;
        }

        return convertedAngle;
    }
}