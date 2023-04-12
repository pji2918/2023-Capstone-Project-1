using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterNum
{
    snakeFish,
    octopus,
    MuteBear,
    muteHuman,
    lightFish,
}

public class MonsterController : MonoBehaviour
{
    [Tooltip("몬스터가 추적할 플레이어의 위치입니다.")] public Vector2 _playerPosition;
    NavMeshAgent _agent;

    SpriteRenderer monsterRenderer;

    public MonsterNum monsterNum;

    protected float speed;
    protected float attack;
    protected int currentHp;
    protected int maxHp;

    private MonsterController monster = null;

    // NavMesh 에이전트의 회전과 Z축을 고정시킵니다.
    void Start()
    {
        monsterRenderer = this.GetComponent<SpriteRenderer>();
        _agent = this.GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        switch (monsterNum)
        {
            case MonsterNum.lightFish:
                monster = new LightFish();
                break;
            case MonsterNum.MuteBear:
                monster = new MuteBear();
                break;
            case MonsterNum.muteHuman:
                monster = new MuteHuman();
                break;
            case MonsterNum.octopus:
                monster = new Octopus();
                break;
            case MonsterNum.snakeFish:
                monster = new SnakeFish();
                break;
        }
    }

    // 플레이어의 위치를 변수에 저장한 다음, 에이전트의 목적지를 플레이어의 위치로 설정합니다.
    void Update()
    {
        _agent.speed = monster.speed;

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
    }

    public virtual void Attack()
    {

    }
}

public class SnakeFish : MonsterController
{
    public SnakeFish()
    {
        speed = 8f;
        attack = 10f;
        maxHp = 50;
    }

    public override void Attack()
    {

    }
}

public class Octopus : MonsterController
{
    public Octopus()
    {
        speed = 5f;
        attack = 10f;
        maxHp = 80;
    }

    public override void Attack()
    {

    }
}

public class MuteBear : MonsterController
{
    public MuteBear()
    {
        speed = 2f;
        attack = 10f;
        maxHp = 100;
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
        attack = 3f;
        maxHp = 30;
    }
}

public class LightFish : MonsterController
{
    public LightFish()
    {
        speed = 5f;
        attack = 1f;
        maxHp = 50;
    }

    public override void Attack()
    {

    }
}