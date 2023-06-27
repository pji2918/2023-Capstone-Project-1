using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DataManager.instance._data.resources["iron"] = 99999;
            DataManager.instance._data.resources["concrete"] = 99999;
            DataManager.instance._data.resources["core"] = 99999;
            DataManager.instance._data.resources["bolt"] = 99999;
            DataManager.instance._data.resources["ingredient"] = 99999;
            DataManager.instance._data.resources["food"] = 99999;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            DataManager.instance._data.resources["iron"] = 0;
            DataManager.instance._data.resources["concrete"] = 0;
            DataManager.instance._data.resources["core"] = 0;
            DataManager.instance._data.resources["bolt"] = 0;
            DataManager.instance._data.resources["ingredient"] = 0;
            DataManager.instance._data.resources["food"] = 0;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            DataManager.instance._data.skillLevel = 0;
            DataManager.instance._data.buildLevel = 0;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log("스킬레벨 : " + DataManager.instance._data.skillLevel);
            Debug.Log("빌드레벨 : " + DataManager.instance._data.buildLevel);
        }
    }
}
