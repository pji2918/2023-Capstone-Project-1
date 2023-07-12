using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    private float _spawnCoolDown;
    public GameObject[] _monsterPrefab;
    public GameObject[] _bossPrefab;
    bool _bossSpawned = false;


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
            _spawnCoolDown = 0.5f;
            if (!InGameUI.instance._isBoss)
            {
                SpawnMonster();
            }
        }
    }

    public struct MonsterSpawnCap
    {
        public struct MonsterList
        {
            public string Name;
            public int[] MaxSpawn;
        }
        public MonsterList[] Monsters;
    }

    public MonsterSpawnCap _monsterSpawnCap = new MonsterSpawnCap()
    {
        Monsters = new MonsterSpawnCap.MonsterList[]
        {
            new MonsterSpawnCap.MonsterList()
            {
                Name = "MuteHuman",
                MaxSpawn = new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,4,4,6,6,8,8,10,10,12,12,14,16,18,20,24,24,30,30,30,36,36,44,50,60,66,72,80,90,100}
            },
            new MonsterSpawnCap.MonsterList()
            {
                Name = "MuteBear",
                MaxSpawn = new int[]{0,0,0,0,0,0,0,0,0,0,0,0,2,2,4,4,4,6,6,6,8,8,10,10,12,12,14,14,16,18,18,18,20,22,24,28,28,28,32,32,32,32,32,40,40,40,44,46,50,54}
            },
            new MonsterSpawnCap.MonsterList()
            {
                Name = "SnakeFish",
                MaxSpawn = new int[]{ 0,0,0,0,0,0,12, 12, 20, 20, 24, 28, 32, 32, 36, 36, 40, 40, 48, 48, 48, 56, 56, 56, 64, 64, 72, 72, 84, 84, 92, 92, 100, 100, 108, 108, 108, 116, 116, 120, 120, 132, 132, 132, 140, 140, 148, 160, 180 }
            },
            new MonsterSpawnCap.MonsterList()
            {
                Name = "LightFish",
                MaxSpawn = new int[]{ 0,0,6, 10, 14, 18, 24, 30, 36, 42, 48, 54, 60, 66, 70, 70, 70, 76, 76, 82, 82, 86, 86, 90, 90, 92, 94, 96, 100, 100, 104, 110, 130, 140, 150, 160, 170, 180, 190, 194, 200, 200, 206, 206, 206, 216, 216, 230, 240, 250 }
            }
        }
    };

    void SpawnMonster()
    {
        if (!_bossSpawned)
        {
            if (DataManager.instance._data.day == 8)
            {
                Instantiate(_bossPrefab[0], new Vector3(8, 0, 0), Quaternion.identity);
                _bossSpawned = true;
            }
            else if (DataManager.instance._data.day == 23)
            {
                Instantiate(_bossPrefab[1], new Vector3(8, 0, 0), Quaternion.identity);
                _bossSpawned = true;
            }
            else if (DataManager.instance._data.day == 34)
            {
                Instantiate(_bossPrefab[2], new Vector3(8, 0, 0), Quaternion.identity);
                _bossSpawned = true;
            }
        }
    SetMonster:
        int monsterIndex = Random.Range(0, 5);
        if (monsterIndex == 4)
        {
            goto SetPosition;
        }
        else if (monsterIndex == 1 || monsterIndex == 2)
        {
            goto SetMonster;
        }
        else if (_monsterSpawnCap.Monsters[monsterIndex].MaxSpawn[DataManager.instance._data.day - 1] <= 0)
        {
            goto SetMonster;
        }
        else
        {
            _monsterSpawnCap.Monsters[monsterIndex].MaxSpawn[DataManager.instance._data.day - 1]--;
        }
    SetPosition:
        Vector2 spawnPos = new Vector2(Random.Range(-60, 60), Random.Range(-60, 60));

        // 만약 스폰 위치와 플레이어 간의 거리가 5 이하이면, 범위 재지정
        if (Vector2.Distance(spawnPos, PlayerController.instance.transform.position) <= 5)
        {
            goto SetPosition;
        }

        Instantiate(_monsterPrefab[monsterIndex], spawnPos, Quaternion.identity);
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
