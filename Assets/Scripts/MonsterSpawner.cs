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
        Debug.Log(Resources.Load<TextAsset>("MonsterSpawnCap").text);
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
                MaxSpawn = new int[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,8,9,10,12,12,15,15,15,18,18,22,25,30,33,36,40,45,50}
            },
            new MonsterSpawnCap.MonsterList()
            {
                Name = "MuteBear",
                MaxSpawn = new int[]{0,0,0,0,0,0,0,0,0,0,0,0,1,1,2,2,2,3,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,11,12,14,14,14,16,16,16,18,18,20,20,20,22,23,25,27}
            },
            new MonsterSpawnCap.MonsterList()
            {
                Name = "SnakeFish",
                MaxSpawn = new int[]{ 0,0,0,0,0,0,3,3,5,0,5,6,7,8,8,9,9,10,10,12,12,12,14,14,14,16,16,18,18,21,21,23,23,25,25,27,27,27,29,29,30,30,33,33,33,35,37,40,45}
            },
            new MonsterSpawnCap.MonsterList()
            {
                Name = "LightFish",
                MaxSpawn = new int[]{ 0,0,3,5,7,9,12,15,18,21,24,27,30,33,35,35,38,38,41,41,43,43,45,45,46,47,48,50,50,52,55,65,70,75,80,85,90,95,97,100,100,103,103,108,115,120,125 }
            }
        }
    };

    void SpawnMonster()
    {
    SetMonster:
        int monsterIndex = Random.Range(0, 5);
        if (monsterIndex == 4)
        {
            goto SetPosition;
        }
        if (_monsterSpawnCap.Monsters[monsterIndex].MaxSpawn[DataManager.instance._data.day - 1] <= 0)
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
