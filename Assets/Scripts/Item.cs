using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이거 다르면 고치면 됨
public enum ItemNum
{
    iron,
    concrete,
    core,
    bolt,
    ingredient,
    nullNum,
}

public class Item : MonoBehaviour
{
    public ItemNum itemNum;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && itemNum != ItemNum.nullNum)
        {
            Debug.Log(itemNum.ToString());
            if (System.Array.FindIndex(InGameUI.instance._quest, x => x._type == itemNum) >= 0)
            {
                InGameUI.instance._quest[System.Array.FindIndex(InGameUI.instance._quest, x => x._type == itemNum)]._amount++;
            }
            // 여기에 자원량을 추가하는 코드를 넣으십시오.
            DataManager.instance._data.resources[itemNum.ToString()]++;
            DataManager.instance.Save();
            PlayerController.instance.CallComplete();
            Destroy(gameObject);
        }
    }
}
