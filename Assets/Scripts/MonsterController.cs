using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [Tooltip("몬스터가 추적할 플레이어의 위치입니다.")] public Vector2 _playerPosition;
    protected NavMeshAgent _agent;

    SpriteRenderer monsterRenderer;

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
        monsterRenderer = this.GetComponent<SpriteRenderer>();
        _agent = this.GetComponent<NavMeshAgent>();
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
    }
}

/*
public class Octopus : MonsterController
{
    public Octopus()
    {
        speed = 5f;
        attack = 10;
        maxHp = 80;
    }
}

public class MuteBear : MonsterController
{
    public MuteBear()
    {
        speed = 2f;
        attack = 10;
        maxHp = 100;
        attackCoolTime = 4f;
    }

    public override void Attack()
    {

    }
}

public class MuteHuman : MonsterController
{
    public MuteHuman()
    {
        speed = 2f;
        attack = 3;
        maxHp = 30;
        attackCoolTime = 3f;
    }
}

public class LightFish : MonsterController
{
    public LightFish()
    {
        speed = 5f;
        attack = 1;
        maxHp = 50;
        attackCoolTime = 5f;
    }

    public override void Attack()
    {

    }
        IEnumerator StopPlayer()
        {
            stopCurrentTime = stopTime;
            while (stopCurrentTime < 0)
            {

            }
        }
    }
*/