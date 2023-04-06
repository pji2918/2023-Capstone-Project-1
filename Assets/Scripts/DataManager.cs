using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

// 이 클래스에 저장할 변수가 담길 것입니다.
class Data
{

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

    private Data _data = new Data();
    string _dataPath = Application.persistentDataPath + "/save.json";

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
    }
}
