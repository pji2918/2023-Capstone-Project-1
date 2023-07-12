using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

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
            var itemIndicator = Instantiate(Resources.Load("ItemIndicator"), transform.position, Quaternion.identity) as GameObject;
            itemIndicator.GetComponent<TextMeshPro>().text = StringUtil.KoreanParticle(string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "getItem"), InGameUI.instance._resourcename[(int)itemNum]));
            if (System.Array.FindIndex(InGameUI.instance._quest, x => x._type == itemNum) >= 0)
            {
                ++InGameUI.instance._quest[System.Array.FindIndex(InGameUI.instance._quest, x => x._type == itemNum)]._amount;
            }
            // 여기에 자원량을 추가하는 코드를 넣으십시오.
            SoundManager.instance.PlayPlayerEffects(SoundManager.instance.GetAudioClip(SoundManager.AudioClips.Get_Item));
            ++DataManager.instance._data.resources[itemNum.ToString()];

            PlayerController.instance.CallComplete();

            Destroy(gameObject);
        }
    }
}
