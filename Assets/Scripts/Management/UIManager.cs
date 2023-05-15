using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject upgradeWindow;
    [SerializeField]
    private GameObject weaponWindow;
    [SerializeField]
    private TextMeshProUGUI upgradeText;
    [SerializeField]
    private GameObject needWeaponResource;
    [SerializeField]
    private GameObject houseWindow;
    [SerializeField]
    private GameObject needHouseResource;
    [SerializeField]
    private TextMeshProUGUI buildText;
    [SerializeField]
    private GameObject bookWindow;
    [SerializeField]
    private GameObject Option;

    #region 버튼 함수
    public void OnClickUpgradeButton()
    {
        WindowPopUp(upgradeWindow);
    }

    public void OnClickWeaponButton()
    {
        PopUpWidowChange(weaponWindow, upgradeWindow);
    }

    public void OnClickweaponUpgradeButton()
    {
        PopUpWidowChange(upgradeText.gameObject, needWeaponResource);
        StartCoroutine(Typing(upgradeText, "강 화 중 . . .", 0.5f, upgradeText.gameObject, needWeaponResource));
        DataManager.instance._data.skillLevel++;
        DataManager.instance.Save();
    }

    public void OnClickHouseButton()
    {
        PopUpWidowChange(houseWindow, upgradeWindow);
    }

    public void OnClickHouseUpgradeButton()
    {
        PopUpWidowChange(buildText.gameObject, needHouseResource);
        StartCoroutine(Typing(buildText, "건 설 중 . . .", 0.5f, buildText.gameObject, needHouseResource));
    }

    public void OnClickFoodButton()
    {
        Debug.Log("식량제작버튼 클릭");
    }

    public void OnClickFoodRecallButton()
    {
        Debug.Log("식량회수버튼 클릭");
    }

    public void OnClickTableButton()
    {
        WindowPopUp(bookWindow);
    }

    public void OnClickOption()
    {
        WindowPopUp(Option);
    }

    public void OnClickUpgradeBackground()
    {
        WindowDisappear(upgradeWindow);
    }

    public void OnClickTableBackground()
    {
        WindowDisappear(bookWindow);
    }

    public void OnClickOptionBackground()
    {
        WindowDisappear(Option);
    }
    #endregion

    void PopUpWidowChange(GameObject popUp, GameObject window)
    {
        popUp.SetActive(!popUp.activeSelf);
        window.SetActive(!window.activeSelf);
    }

    void WindowDisappear(GameObject popUp)
    {
        popUp.SetActive(false);
    }

    void WindowPopUp(GameObject popUp)
    {
        popUp.SetActive(true);
    }

    public void HomeToFighting()
    {
        SceneManager.LoadScene("Fighting");
    }

    IEnumerator Typing(TextMeshProUGUI typingText, string message, float speed, GameObject popUp, GameObject window)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
        typingText.text = "완료";
        yield return new WaitForSeconds(1);
        PopUpWidowChange(popUp, window);
    }
}
