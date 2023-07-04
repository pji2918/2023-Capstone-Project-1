using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public enum Language
{
    Auto,
    Korean,
    English
}

// 이 클래스에 저장할 변수가 담길 것입니다.
public class Data
{
    public int skillLevel = 0;
    public int buildLevel = 0;
    public int day = 1;

    public int musicVolume = 100;
    public int effectVolume = 100;
    public FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;
    public Resolution resolution;
    public bool is3dAudio = true;
    public bool isScreenVibration = true, displayDamage = true;
    public Language language = Language.Auto;

    public Dictionary<string, int> resources = new Dictionary<string, int>()
    {
        {"iron", 10},
        {"concrete", 10},
        {"bolt", 10},
        {"core", 10},
        {"ingredient", 10},
        {"food", 10}
    };

    public Dictionary<string, int> weaponUpgrade = new Dictionary<string, int>()
    {
        {"iron", 1},
        {"concrete", 0},
        {"bolt", 0},
        {"core", 0}
    };

    public Dictionary<string, int> HouseUpgrade = new Dictionary<string, int>()
    {
        {"iron", 1},
        {"concrete", 0},
        {"bolt", 0},
        {"core", 0}
    };
}

public struct PlayerStat
{
    public List<int> atk, maxHp;
    public List<double> skillDamageMultiplier;
    public List<float> moveSpeed;

    public PlayerStat(int dummy)
    {
        atk = new List<int>()
        {
            10, 11, 12, 14, 15, 16, 18, 19, 20, 23, 25, 28, 30, 33, 36, 40
        };
        skillDamageMultiplier = new List<double>()
        {
            0, 0.2, 0.2, 0.3, 0.5, 0.5, 1, 1, 1, 1.5, 1.5, 1.7, 2, 2, 2, 2.5
        };
        maxHp = new List<int>()
        {
            100, 105, 115, 120, 125, 135, 140, 145, 155, 160, 180
        };
        moveSpeed = new List<float>()
        {
            5f, 5.2f, 5.5f, 5.7f, 5.9f, 6.3f, 6.5f, 6.7f, 7f, 7.2f, 7.5f
        };
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public PlayerStat _playerStat = new PlayerStat(0);
    public int EnemyCount = 0;

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
        EnemyCount = 0;
        _dataPath = Path.Combine(Application.persistentDataPath, "save.json");
        Load();
    }

    public Data _data = new Data();
    string _dataPath;

    // Data의 내용을 JSON으로 저장합니다.
    public void Save()
    {
        string json = JsonConvert.SerializeObject(_data, Formatting.Indented);
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
            if (_data.resolution.width < Screen.resolutions[0].width || _data.resolution.height < Screen.resolutions[0].height || _data.resolution.refreshRate < Screen.resolutions[0].refreshRate)
            {
                _data.resolution = Screen.resolutions[Screen.resolutions.Length - 1];
            }
            Save();
        }
    }

    public void Delete()
    {
        if (File.Exists(_dataPath))
        {
            File.Delete(_dataPath);
        }
    }

    public void NeedWeaponResourseChange(int iron, int concrete, int bolt, int core)
    {
        _data.weaponUpgrade["iron"] = iron;
        _data.weaponUpgrade["concrete"] = concrete;
        _data.weaponUpgrade["bolt"] = bolt;
        _data.weaponUpgrade["core"] = core;

        Save();
    }

    public void NeedHouseResourseChange(int iron, int concrete, int bolt, int core)
    {
        _data.HouseUpgrade["iron"] = iron;
        _data.HouseUpgrade["concrete"] = concrete;
        _data.HouseUpgrade["bolt"] = bolt;
        _data.HouseUpgrade["core"] = core;

        Save();
    }
}
