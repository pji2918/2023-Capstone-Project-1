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
        if (other.CompareTag("Player"))
        {
            Debug.Log(itemNum.ToString());
            // 변수 이름 (itemNum.ToString());
            Destroy(gameObject);
        }
    }
}
