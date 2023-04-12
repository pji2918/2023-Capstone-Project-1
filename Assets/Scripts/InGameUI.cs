using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public Image _healthBar;

    public Image _dashCoolDownBar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _healthBar.fillAmount = PlayerController.instance._playerHp / 100f;
        _dashCoolDownBar.fillAmount = PlayerController.instance._dashCoolDown / 10f;
    }
}
