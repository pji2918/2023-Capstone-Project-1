using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int foodCount = 0;
    public float _currentCookingTime = 0;
    public bool _isCooking = false;
    public int _healthReduce = 0;

    private void Awake()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("GameManager");
            if (go == null)
            {
                go = new GameObject { name = "GameManager" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<GameManager>();
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (_isCooking)
        {
            if (_currentCookingTime < 30)
            {
                _currentCookingTime += Time.deltaTime;
            }
            else
            {
                foodCount += 2;
                _isCooking = false;
                _currentCookingTime = 0;
            }
        }
    }

    public void IncreseResource(string resourceName, int amount)
    {
        DataManager.instance._data.resources[resourceName] += amount;
        DataManager.instance.Save();
    }

    public void ResourceReduction(string resourceName, int amount)
    {
        DataManager.instance._data.resources[resourceName] -= amount;
        DataManager.instance.Save();
    }
}
