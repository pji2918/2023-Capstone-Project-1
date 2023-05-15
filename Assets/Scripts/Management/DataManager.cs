using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

// 이 클래스에 저장할 변수가 담길 것입니다.
public class Data
{
    public int skillLevel = 0;

    public Dictionary<string, int> resources = new Dictionary<string, int>()
    {
        {"iron", 10},
        {"concrete", 10},
        {"core", 10},
        {"bolt", 10},
        {"ingredient", 10},
        {"food", 10}
    };
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _dataPath = Path.Combine(Application.persistentDataPath, "save.json");
        Load();
    }

    public Data _data = new Data();
    string _dataPath;

    // Data의 내용을 JSON으로 저장합니다.
    public void Save()
    {
        string json = JsonConvert.SerializeObject(_data);
        File.WriteAllText(_dataPath, json);
    }

    // JSON을 읽어서 Data에 넣습니다.
    public void Load()
    {
        if (File.Exists(_dataPath))
        {
            string json = File.ReadAllText(_dataPath);
            _data = JsonConvert.DeserializeObject<Data>(json);
        }
        else
        {
            Save();
        }
    }
}
