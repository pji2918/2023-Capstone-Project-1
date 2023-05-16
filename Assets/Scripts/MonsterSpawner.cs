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
        Instantiate(_monsterPrefab[Random.Range(0, _monsterPrefab.Length)], new Vector2(Random.Range(-17, 17), Random.Range(-17, 17)), Quaternion.identity);
    }
}