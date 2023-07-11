using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class MonsterController : MonoBehaviour
{
    [Tooltip("몬스터가 추적할 플레이어의 위치입니다.")] public Vector2 _playerPosition;
    protected NavMeshAgent _agent;

    SpriteRenderer monsterRenderer;
    protected ItemSpawn itemSpawn;

    public GameObject _dieEffect;

    protected float speed;

    protected float _damagecool;

    protected int attack;
    protected float attackCurrentTime = 0;
    protected float attackCoolTime;

    protected int currentHp;
    protected int maxHp;

    protected bool doRotate = true;
    [SerializeField] protected double _attackRange = 1;

    public int Hp { get { return currentHp; } set { currentHp = value; } }
    protected bool _isKnockback = false;

    // NavMesh 에이전트의 회전과 Z축을 고정시킵니다.
    protected virtual void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;
        currentHp = maxHp;
        monsterRenderer = this.GetComponent<SpriteRenderer>();
        itemSpawn = this.GetComponent<ItemSpawn>();
    }
    // 플레이어의 위치를 변수에 저장한 다음, 에이전트의 목적지를 플레이어의 위치로 설정합니다.
    protected virtual void Update()
    {
        if (doRotate)
        {
            if (transform.position.x > _playerPosition.x)
            {
                monsterRenderer.flipX = false;
            }
            else if (transform.position.x < _playerPosition.x)
            {
                monsterRenderer.flipX = true;
            }
        }

        if (_damagecool > 0)
        {
            _damagecool -= Time.deltaTime;
        }

        attackCurrentTime += Time.deltaTime;

        if (currentHp <= 0)
        {
            DataManager.instance.EnemyCount++;
            Debug.Log("count : " + DataManager.instance.EnemyCount);
            itemSpawn.RandomItem();
            Instantiate(_dieEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }
        try
        {
            if (this.gameObject.GetComponent<NavMeshAgent>().enabled)
            {
                _playerPosition = GameObject.Find("Player").transform.position;
                _agent.SetDestination(_playerPosition);
            }

            if (!PlayerController.instance._jangpungOn)
            {
                _isKnockback = false;
                this.GetComponent<NavMeshAgent>().isStopped = false;
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        catch
        {

        }
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bubble") && _damagecool <= 0)
        {
            // 여기에 몬스터가 비눗방울에 닿았을 때 HP가 서서히 감소하는 코드를 작성합니다.
            Damage((int)(2 + (2 * PlayerController.instance._skillDamageMultiplier)));
            _damagecool = 0.1f;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Harpoon"))
        {
            // 여기에 몬스터가 작살에 닿았을 때 HP가 감소하는 코드를 작성합니다.
            Damage((int)(30 + (30 * PlayerController.instance._skillDamageMultiplier)));
        }
        if (other.name == "Attack_Effect")
        {
            // 여기에 기본 공격에 닿았을 때 HP가 감소하는 코드를 작성합니다.
            Damage(PlayerController.instance._playerAtk);
        }
        if (other.CompareTag("Mine_Explosion"))
        {
            // 여기에 지뢰에 닿았을 때 HP가 감소하는 코드를 작성합니다.
            Damage((int)(50 + (50 * PlayerController.instance._skillDamageMultiplier)));
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Jangpung"))
        {
            _isKnockback = true;
            this.GetComponent<NavMeshAgent>().isStopped = true;
            // 여기에 몬스터가 장풍에 닿았을 때 HP가 감소하는 코드를 작성합니다.
            Damage((int)(5 + (5 * PlayerController.instance._skillDamageMultiplier)));
        }
    }

    protected void Damage(int damage)
    {
        GameObject hitEffect = Instantiate(Resources.Load("HitEffect"), transform.position, Quaternion.identity) as GameObject;
        hitEffect.transform.SetParent(this.transform);
        if (PlayerController.instance._isPowerUp)
        {
            damage = int.MaxValue;
        }
        currentHp -= damage;
        if (DataManager.instance._data.displayDamage)
        {
            var text = Instantiate(Resources.Load("Damage"), transform.position, Quaternion.identity) as GameObject;
            text.GetComponent<TextMeshPro>().text = damage.ToString();
            StartCoroutine(DamageEffectCoroutine());
        }
    }

    public IEnumerator DamageEffectCoroutine()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 170, 170, 255);
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}