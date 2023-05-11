using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [System.Serializable]
    public struct ItemPercent
    {
        public ItemNum itemNum;
        public float percent;
    }

    // 드랍될 아이템 종류 배열
    public GameObject[] _items;
    public ItemPercent[] _itemPercents;

    public void RandomItem()
    {
        float randomValue = Random.value * 100.0f;
        float culValue = 0;

        PlayerController.instance.sumCount++;

        for (int i = 0; i < _itemPercents.Length; i++)
        {
            culValue += _itemPercents[i].percent;
            if (randomValue <= culValue && _itemPercents[i].itemNum != ItemNum.nullNum)
            {
                //GameObject item = Instantiate(_items[(int)(_itemPercents[i].itemNum)]);
                PlayerController.instance.itemCounts[(int)(_itemPercents[i].itemNum)]++;
                InGameUI.instance._quest[System.Array.FindIndex(InGameUI.instance._quest, x => x._type == _itemPercents[i].itemNum)]._amount++;
                //item.transform.position = transform.position;
                break;
            }
            else if (_itemPercents[i].itemNum == ItemNum.nullNum)
            {
                PlayerController.instance.itemCounts[(int)(_itemPercents[5].itemNum)]++;
            }
        }
    }
}
