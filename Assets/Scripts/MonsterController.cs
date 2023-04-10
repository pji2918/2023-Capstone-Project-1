using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterController : MonoBehaviour
{
    [Tooltip("몬스터가 추적할 플레이어의 위치입니다.")] public Vector2 _playerPosition;
    NavMeshAgent _agent;

    SpriteRenderer monsterRenderer;

    public float speed;

    // NavMesh 에이전트의 회전과 Z축을 고정시킵니다.
    void Start()
    {
        monsterRenderer = this.GetComponent<SpriteRenderer>();
        _agent = this.GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    // 플레이어의 위치를 변수에 저장한 다음, 에이전트의 목적지를 플레이어의 위치로 설정합니다.
    void Update()
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
    }

    public abstract void Attack();
}

public class SnakeFish : MonsterController
{
    //speed = 5f;

    public override void Attack()
    {

    }
}

public class Octopus : MonsterController
{
    public override void Attack()
    {

    }
}

public class MuteBear : MonsterController
{
    public override void Attack()
    {

    }
}

public class MuteHuman : MonsterController
{
    public override void Attack()
    {

    }
}

public class LightFish : MonsterController
{
    public override void Attack()
    {

    }
}