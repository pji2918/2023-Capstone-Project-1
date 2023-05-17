using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEngine.EventSystems;

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
    public Image _foodCoolDownBar;
    public GameObject[] _statusPanel;
    private bool[] _isHovering = new bool[5] { false, false, false, false, false };
    public GameObject[] _skillTooltip, _statusTooltip;
    public GameObject _foodTooltip;
    public TextMeshProUGUI[] _questText = new TextMeshProUGUI[3];
    public TextMeshProUGUI _foodCountText;
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

    public bool[] _isQuestComplete = new bool[3] { false, false, false };

    public string[] _resourcename = new string[5] { "철근", "콘크리트", "코어", "볼트와 너트", "식량" };

    // Update is called once per frame
    void Update()
    {
        _healthBar.fillAmount = (float)PlayerController.instance._playerHp / PlayerController.instance._playerMaxHp;

        #region 스킬 쿨타임 표시
        _skillCoolDownBar[0].fillAmount = 1 - (PlayerController.instance._dashCoolDown / 7f);
        _skillCoolDownBar[1].fillAmount = 1 - (PlayerController.instance._harpoonCoolDown / 8f);
        _skillCoolDownBar[2].fillAmount = 1 - (PlayerController.instance._bubbleCoolDown / 15f);
        _skillCoolDownBar[3].fillAmount = 1 - (PlayerController.instance._jangpungCoolDown / 13f);
        _skillCoolDownBar[4].fillAmount = 1 - (PlayerController.instance._mineCoolDown / 30f);
        _skillCoolDownBar[4].fillAmount = 1 - (PlayerController.instance._mineCoolDown / 30f);
        _foodCoolDownBar.fillAmount = 1 - (PlayerController.instance._foodCoolDown / 10f);
        #endregion

        _foodCountText.text = string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "skill_foodcount"), DataManager.instance._data.resources["food"]);

        #region 스킬 활성화 여부 표시

        if (DataManager.instance._data.skillLevel >= 1)
        {
            _skillCoolDownBar[0].GetComponent<EventTrigger>().enabled = true;
            _skillCoolDownBar[0].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            _skillCoolDownBar[0].GetComponent<EventTrigger>().enabled = false;
            _skillCoolDownBar[0].GetComponent<Image>().color = new Color32(94, 94, 94, 255);
        }

        if (DataManager.instance._data.skillLevel >= 2)
        {
            _skillCoolDownBar[1].GetComponent<EventTrigger>().enabled = true;
            _skillCoolDownBar[1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            _skillCoolDownBar[1].GetComponent<EventTrigger>().enabled = false;
            _skillCoolDownBar[1].GetComponent<Image>().color = new Color32(94, 94, 94, 255);
        }

        if (DataManager.instance._data.skillLevel >= 3)
        {
            _skillCoolDownBar[2].GetComponent<EventTrigger>().enabled = true;
            _skillCoolDownBar[2].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            _skillCoolDownBar[2].GetComponent<EventTrigger>().enabled = false;
            _skillCoolDownBar[2].GetComponent<Image>().color = new Color32(94, 94, 94, 255);
        }

        if (DataManager.instance._data.skillLevel >= 4)
        {
            _skillCoolDownBar[3].GetComponent<EventTrigger>().enabled = true;
            _skillCoolDownBar[3].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            _skillCoolDownBar[3].GetComponent<EventTrigger>().enabled = false;
            _skillCoolDownBar[3].GetComponent<Image>().color = new Color32(94, 94, 94, 255);
        }

        if (DataManager.instance._data.skillLevel >= 5)
        {
            _skillCoolDownBar[4].GetComponent<EventTrigger>().enabled = true;
            _skillCoolDownBar[4].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            _skillCoolDownBar[4].GetComponent<EventTrigger>().enabled = false;
            _skillCoolDownBar[4].GetComponent<Image>().color = new Color32(94, 94, 94, 255);
        }

        if (DataManager.instance._data.resources["food"] >= 1)
        {
            _foodCoolDownBar.GetComponent<Image>().color = new Color32(255, 100, 100, 255);
        }
        else
        {
            _foodCoolDownBar.GetComponent<Image>().color = new Color32(94, 94, 94, 255);
        }

        #endregion

        #region 퀘스트 표시
        for (int i = 0; i < 3; i++)
        {
            _questText[i].text = string.Format("{0} ({1}/{2})", _resourcename[(int)_quest[i]._type], _quest[i]._amount, _quest[i]._objectiveAmount);
            if (_quest[i]._amount >= _quest[i]._objectiveAmount)
            {
                _questText[i].color = new Color32(0, 255, 0, 255);
                _isQuestComplete[i] = true;
            }
        }
        #endregion

        if (PlayerController.instance.isStop)
        {
            _statusPanel[0].SetActive(true);
            _statusPanel[0].transform.GetChild(0).GetComponent<Image>().fillAmount = 1 - (PlayerController.instance.stopCurrentTime / PlayerController.instance._stopTime);
        }
        else
        {
            _statusPanel[0].SetActive(false);
            _statusTooltip[0].SetActive(false);
        }
    }

    public void OnHoverStop()
    {
        _statusTooltip[0].SetActive(true);
    }

    public void OnNotHoverStop()
    {
        _statusTooltip[0].SetActive(false);
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

    public void OnHoverFood()
    {
        _foodTooltip.SetActive(true);
    }

    public void OnNotHoverFood()
    {
        _foodTooltip.SetActive(false);
    }
}
