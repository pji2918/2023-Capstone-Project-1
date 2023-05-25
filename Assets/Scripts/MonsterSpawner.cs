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
        Instantiate(_monsterPrefab[Random.Range(0, _monsterPrefab.Length)], new Vector2(Random.Range(-17, 17), Random.Range(-17, 17)), Quaternion.identity);
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
