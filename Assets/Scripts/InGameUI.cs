using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public Image _healthBar;

    public Image _dashCoolDownBar;
    private bool[] _isHovering = new bool[4] { false, false, false, false };
    public GameObject[] _skillTooltip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _healthBar.fillAmount = PlayerController.Instance._playerHp / 100f;
        _dashCoolDownBar.fillAmount = PlayerController.Instance._dashCoolDown / 10f;

        if (_isHovering[0])
        {
            _skillTooltip[0].SetActive(true);
        }
        else
        {
            _skillTooltip[0].SetActive(false);
        }
    }

    public void OnHoverOne()
    {
        _isHovering[0] = true;
    }

    public void OnNotHoverOne()
    {
        _isHovering[0] = false;
    }
}
