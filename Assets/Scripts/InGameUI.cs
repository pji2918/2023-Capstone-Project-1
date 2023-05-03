using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public Image _healthBar;
    public Image[] _skillCoolDownBar;
    private bool[] _isHovering = new bool[4] { false, false, false, false };
    public GameObject[] _skillTooltip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _healthBar.fillAmount = PlayerController.instance._playerHp / 100f;
        _skillCoolDownBar[0].fillAmount = 1 - (PlayerController.instance._dashCoolDown / 10f);
        _skillCoolDownBar[1].fillAmount = 1 - (PlayerController.instance._harpoonCoolDown / 8f);
        _skillCoolDownBar[2].fillAmount = 1 - (PlayerController.instance._bubbleCoolDown / 15f);
        _skillCoolDownBar[3].fillAmount = 1 - (PlayerController.instance._jangpungCoolDown / 13f);
        _skillCoolDownBar[4].fillAmount = 1 - (PlayerController.instance._mineCoolDown / 30f);
    }

    public void OnHoverOne()
    {
        _skillTooltip[0].SetActive(true);
    }

    public void OnNotHoverOne()
    {
        _skillTooltip[0].SetActive(false);
    }

    public void OnHoverTwo()
    {
        _skillTooltip[1].SetActive(true);
    }

    public void OnNotHoverTwo()
    {
        _skillTooltip[1].SetActive(false);
    }

    public void OnHoverThree()
    {
        _skillTooltip[2].SetActive(true);
    }

    public void OnNotHoverThree()
    {
        _skillTooltip[2].SetActive(false);
    }

    public void OnHoverFour()
    {
        _skillTooltip[3].SetActive(true);
    }

    public void OnNotHoverFour()
    {
        _skillTooltip[3].SetActive(false);
    }

    public void OnHoverFive()
    {
        _skillTooltip[4].SetActive(true);
    }

    public void OnNotHoverFive()
    {
        _skillTooltip[4].SetActive(false);
    }
}
