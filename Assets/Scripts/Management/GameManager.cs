using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int foodCount = 0;

    public Dictionary<string, int> resources = new Dictionary<string, int>()
    {
        {"iron", 10},
        {"concrete", 10},
        {"bolt", 10},
        {"core", 10},
        {"food", 10},
        {"ingredient", 10}
    };

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
        
    }

    public void IncreseResource(string resourceName, int amount)
    {
        resources[resourceName] += amount;
    }

    public void ResourceReduction(string resourceName, int amount)
    {
        resources[resourceName] -= amount;
    }
}
