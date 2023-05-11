using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    public struct Resource
    {
        public ItemNum _type;
        public int _amount;
        public int _objectiveAmount;
    }

    public Image _healthBar;
    public Image[] _skillCoolDownBar;
    private bool[] _isHovering = new bool[4] { false, false, false, false };
    public GameObject[] _skillTooltip;
    public TextMeshProUGUI[] _questText = new TextMeshProUGUI[3];

    public Resource[] _quest = new Resource[3];

    public static InGameUI instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _quest[0]._type = ItemNum.iron;
        _quest[0]._amount = 0;
        _quest[0]._objectiveAmount = 10;

        _quest[1]._type = ItemNum.concrete;
        _quest[1]._amount = 0;
        _quest[1]._objectiveAmount = 10;

        _quest[2]._type = ItemNum.bolt;
        _quest[2]._amount = 0;
        _quest[2]._objectiveAmount = 10;
    }

    public string[] _resourcename = new string[5] { "철근", "콘크리트", "코어", "볼트와 너트", "식량" };

    // Update is called once per frame
    void Update()
    {
        _healthBar.fillAmount = PlayerController.instance._playerHp / 100f;

        #region 스킬 쿨타임 표시
        _skillCoolDownBar[0].fillAmount = 1 - (PlayerController.instance._dashCoolDown / 10f);
        _skillCoolDownBar[1].fillAmount = 1 - (PlayerController.instance._harpoonCoolDown / 8f);
        _skillCoolDownBar[2].fillAmount = 1 - (PlayerController.instance._bubbleCoolDown / 15f);
        _skillCoolDownBar[3].fillAmount = 1 - (PlayerController.instance._jangpungCoolDown / 13f);
        _skillCoolDownBar[4].fillAmount = 1 - (PlayerController.instance._mineCoolDown / 30f);
        #endregion

        #region 퀘스트 표시
        for (int i = 0; i < 3; i++)
        {
            _questText[i].text = string.Format("{0} ({1}/{2})", _resourcename[(int)_quest[i]._type], _quest[i]._amount, _quest[i]._objectiveAmount);
            if (_quest[i]._amount >= _quest[i]._objectiveAmount)
            {
                _questText[i].color = new Color32(0, 255, 0, 255);
            }
        }
        #endregion
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
