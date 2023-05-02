using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public int currentEnemyCount;

    public int multipleNumber;

    private void Update()
    {
        if (currentEnemyCount <= enemyCount)
        {
            for (int i = 0; i < multipleNumber; i++)
            {
                Instantiate(enemyPrefab);
                currentEnemyCount++;
            }
        }
    }
}
