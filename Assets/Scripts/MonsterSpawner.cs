using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    private float _spawnCoolDown;
    public GameObject[] _monsterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(SpawnMonsterInOrder());
    }

    // Update is called once per frame
    void Update()
    {
        _spawnCoolDown -= Time.deltaTime;

        if (_spawnCoolDown <= 0)
        {
            _spawnCoolDown = 5f;
            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
    SetPosition:
        Vector2 spawnPos = new Vector2(Random.Range(-60, 60), Random.Range(-60, 60));

        // 만약 스폰 위치와 플레이어 간의 거리가 5 이하이면, 범위 재지정
        if (Vector2.Distance(spawnPos, PlayerController.instance.transform.position) <= 5)
        {
            goto SetPosition;
        }

        Instantiate(_monsterPrefab[Random.Range(0, _monsterPrefab.Length)], spawnPos, Quaternion.identity);
    }

    // IEnumerator SpawnMonsterInOrder()
    // {
    //     for (int i = 0; i < _monsterPrefab.Length; i++)
    //     {
    //         Instantiate(_monsterPrefab[i], new Vector3(8, 0, 0), Quaternion.identity);
    //         yield return new WaitForSeconds(30f);
    //     }
    // }
}
