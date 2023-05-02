using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [Tooltip("몬스터가 추적할 플레이어의 위치입니다.")] public Vector2 _playerPosition;
    protected NavMeshAgent _agent;

    SpriteRenderer monsterRenderer;
    ItemSpawn itemSpawn;

    protected float speed;

    protected int attack;

    protected float attackCurrentTime = 0;
    protected float attackCoolTime;

    protected int currentHp;
    protected int maxHp;

    public int Hp { get { return currentHp; } set { currentHp = value; } }

    // NavMesh 에이전트의 회전과 Z축을 고정시킵니다.
    protected virtual void Start()
    {
        currentHp = maxHp;
        monsterRenderer = this.GetComponent<SpriteRenderer>();
        _agent = this.GetComponent<NavMeshAgent>();
        itemSpawn = this.GetComponent<ItemSpawn>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    // 플레이어의 위치를 변수에 저장한 다음, 에이전트의 목적지를 플레이어의 위치로 설정합니다.
    protected virtual void Update()
    {
        _playerPosition = GameObject.Find("Player").transform.position;
        _agent.SetDestination(_playerPosition);

        if (transform.position.x > _playerPosition.x)
        {
            monsterRenderer.flipX = false;
        }
        else if (transform.position.x < _playerPosition.x)
        {
            monsterRenderer.flipX = true;
        }

        attackCurrentTime += Time.deltaTime;

        if (currentHp <= 0)
        {
            itemSpawn.RandomItem();
            Destroy(gameObject);
            return;
        }
    }
}