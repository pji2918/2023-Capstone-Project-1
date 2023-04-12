using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject weaponWindow;
    [SerializeField]
    private GameObject upgradeWindow;
    [SerializeField]
    private GameObject bookWindow;

    #region 버튼 함수
    public void OnClickWeaponButton()
    {
        WindowPopUp(weaponWindow);
    }

    public void OnClickUpgradeButton()
    {
        WindowPopUp(upgradeWindow);
    }

    public void OnClickTableButton()
    {
        WindowPopUp(bookWindow);
    }

    public void OnClickWeaponBackground()
    {
        WindowDisappear(weaponWindow);
    }

    public void OnClickUpgradeBackground()
    {
        WindowDisappear(upgradeWindow);
    }

    public void OnClickTableBackground()
    {
        WindowDisappear(bookWindow);
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
}
