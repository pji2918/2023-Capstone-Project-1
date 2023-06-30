using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
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
    public Image[] _skillIcon, _skillCoolDownBar;
    public Image _foodCoolDownBar;
    public GameObject[] _statusPanel, _skillLock;
    private bool[] _isHovering = new bool[5] { false, false, false, false, false };
    public GameObject[] _skillTooltip, _statusTooltip;
    public GameObject _foodTooltip;
    public GameObject _pauseUI;
    public TextMeshProUGUI[] _questText = new TextMeshProUGUI[3];
    public TextMeshProUGUI _foodCountText, _timerText;
    public Resource[] _quest = new Resource[5];
    public bool _isPause = false;
    public Slider[] _volumeSlider = new Slider[2];
    public TextMeshProUGUI[] _volumeText = new TextMeshProUGUI[2];
    public static InGameUI instance;
    public bool _isBoss = false;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        #region 퀘스트 설정
        switch (DataManager.instance._data.day)
        {
            case 1:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 1;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 1;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 3;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 2:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 6;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 6;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 13;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 4;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 3:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 13;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 8;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 15;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 4:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 5;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 16;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 21;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 4;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 5:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 24;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 3;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 6;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 6:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 5;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 8;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 24;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 8;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 7:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 34;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 13;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 1;
                    break;
                }
            case 8:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 31;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 5;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 7;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 18;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 9:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 28;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 28;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 51;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 10:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 45;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 53;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 11:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 31;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 28;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 62;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 12:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 64;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 23;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 13:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 23;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 23;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 31;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 3;
                    break;
                }
            case 14:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 17;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 17;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 13;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 5;
                    break;
                }
            case 15:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 42;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 42;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 25;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 16:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 81;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 6;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 17:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 53;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 53;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 31;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 8;
                    break;
                }
            case 18:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 38;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 38;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 74;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 19:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 48;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 48;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 53;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 23;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 20:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 87;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 34;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 21:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 74;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 35;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 1;
                    break;
                }
            case 22:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 81;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 24;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 2;
                    break;
                }
            case 23:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 45;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 45;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 77;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 1;
                    break;
                }
            case 24:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 85;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 23;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 2;
                    break;
                }
            case 25:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 31;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 75;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 38;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 26:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 33;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 33;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 56;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 3;
                    break;
                }
            case 27:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 56;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 81;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 28:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 48;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 76;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 15;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 29:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 34;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 24;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 3;
                    break;
                }
            case 30:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 51;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 34;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 31:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 27;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 28;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 34;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 12;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 2;
                    break;
                }
            case 32:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 31;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 48;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 28;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 3;
                    break;
                }
            case 33:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 0;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 0;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 13;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 5;
                    break;
                }
            case 34:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 21;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 21;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 0;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 36;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 0;
                    break;
                }
            case 35:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 65;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 65;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 88;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 8;
                    break;
                }
            case 36:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 31;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 31;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 48;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 13;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 5;
                    break;
                }
            case 37:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 28;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 34;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 51;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 14;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 5;
                    break;
                }
            case 38:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 28;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 26;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 53;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 12;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 5;
                    break;
                }
            case 39:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 31;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 31;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 45;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 15;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 5;
                    break;
                }
            case 40:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 24;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 24;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 31;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 24;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 5;
                    break;
                }
            case 41:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 34;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 34;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 28;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 11;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 8;
                    break;
                }
            case 42:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 41;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 41;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 23;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 15;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 8;
                    break;
                }
            case 43:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 38;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 38;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 21;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 21;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 8;
                    break;
                }
            case 44:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 51;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 51;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 38;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 8;
                    break;
                }
            case 45:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 22;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 22;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 58;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 23;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 12;
                    break;
                }
            case 46:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 24;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 24;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 53;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 28;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 12;
                    break;
                }
            case 47:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 58;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 58;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 34;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 14;
                    break;
                }
            case 48:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 26;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 26;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 53;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 28;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 14;
                    break;
                }
            case 49:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 31;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 31;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 44;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 31;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 16;
                    break;
                }
            case 50:
                {
                    _quest[0]._type = ItemNum.iron;
                    _quest[0]._amount = 0;
                    _quest[0]._objectiveAmount = 60;

                    _quest[1]._type = ItemNum.concrete;
                    _quest[1]._amount = 0;
                    _quest[1]._objectiveAmount = 60;

                    _quest[2]._type = ItemNum.bolt;
                    _quest[2]._amount = 0;
                    _quest[2]._objectiveAmount = 88;

                    _quest[3]._type = ItemNum.ingredient;
                    _quest[3]._amount = 0;
                    _quest[3]._objectiveAmount = 0;

                    _quest[4]._type = ItemNum.core;
                    _quest[4]._amount = 0;
                    _quest[4]._objectiveAmount = 18;
                    break;
                }
        }
        #endregion

        _resourcename[0] = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "iron");
        _resourcename[1] = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "concrete");
        _resourcename[2] = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "core");
        _resourcename[3] = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "bolt");
        _resourcename[4] = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "ingredient");
    }

    public bool[] _isQuestComplete = new bool[5] { false, false, false, false, false };

    public string[] _resourcename = new string[6] { "철근", "콘크리트", "코어", "볼트와 너트", "식재료", "null" };

    // Update is called once per frame
    void Update()
    {
        _healthBar.fillAmount = (float)PlayerController.instance._playerHp / PlayerController.instance._playerMaxHp;

        #region 스킬 쿨타임 표시
        _skillCoolDownBar[0].fillAmount = (PlayerController.instance._dashCoolDown / 7f);
        _skillCoolDownBar[1].fillAmount = (PlayerController.instance._harpoonCoolDown / 8f);
        _skillCoolDownBar[2].fillAmount = (PlayerController.instance._bubbleCoolDown / 15f);
        _skillCoolDownBar[3].fillAmount = (PlayerController.instance._jangpungCoolDown / 13f);
        _skillCoolDownBar[4].fillAmount = (PlayerController.instance._mineCoolDown / 30f);
        _skillCoolDownBar[4].fillAmount = (PlayerController.instance._mineCoolDown / 30f);
        _foodCoolDownBar.fillAmount = PlayerController.instance._foodCoolDown / 10f;
        #endregion

        _foodCountText.text = string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "skill_foodcount"), DataManager.instance._data.resources["food"]);

        #region 스킬 활성화 여부 표시

        if (DataManager.instance._data.skillLevel >= 1)
        {
            _skillIcon[0].GetComponent<EventTrigger>().enabled = true;
            _skillLock[0].SetActive(false);
        }
        else
        {
            _skillIcon[0].GetComponent<EventTrigger>().enabled = false;
            _skillLock[0].SetActive(true);
        }

        if (DataManager.instance._data.skillLevel >= 3)
        {
            _skillIcon[1].GetComponent<EventTrigger>().enabled = true;
            _skillLock[1].SetActive(false);
        }
        else
        {
            _skillIcon[1].GetComponent<EventTrigger>().enabled = false;
            _skillLock[1].SetActive(true);
        }

        if (DataManager.instance._data.skillLevel >= 6)
        {
            _skillIcon[2].GetComponent<EventTrigger>().enabled = true;
            _skillLock[2].SetActive(false);
        }
        else
        {
            _skillIcon[2].GetComponent<EventTrigger>().enabled = false;
            _skillLock[2].SetActive(true);
        }

        if (DataManager.instance._data.skillLevel >= 9)
        {
            _skillIcon[3].GetComponent<EventTrigger>().enabled = true;
            _skillLock[3].SetActive(false);
        }
        else
        {
            _skillIcon[3].GetComponent<EventTrigger>().enabled = false;
            _skillLock[3].SetActive(true);
        }

        if (DataManager.instance._data.skillLevel >= 12)
        {
            _skillIcon[4].GetComponent<EventTrigger>().enabled = true;
            _skillLock[4].SetActive(false);
        }
        else
        {
            _skillIcon[4].GetComponent<EventTrigger>().enabled = false;
            _skillLock[4].SetActive(true);
        }

        if (DataManager.instance._data.resources["food"] <= 0)
        {
            _foodCoolDownBar.fillAmount = 1;
        }

        #endregion



        #region 퀘스트 표시
        for (int i = 0; i < 5; i++)
        {
            if (_quest[i]._objectiveAmount <= 0)
            {
                _questText[i].gameObject.SetActive(false);

            }
            else
            {
                _questText[i].text = string.Format("{0} ({1}/{2})", _resourcename[(int)_quest[i]._type], _quest[i]._amount, _quest[i]._objectiveAmount);
            }

            if (_quest[i]._amount >= _quest[i]._objectiveAmount || _quest[i]._objectiveAmount == 0)
            {
                _questText[i].color = new Color32(0, 255, 0, 255);
                _isQuestComplete[i] = true;
            }
        }
        #endregion

        _timerText.text = string.Format("{0:00}:{1:00}", (180 - (int)PlayerController.instance._timer) / 60 % 60, (180 - (int)PlayerController.instance._timer) % 60);

        #region 상태 표시
        if (PlayerController.instance.isSlow)
        {
            _statusPanel[0].SetActive(true);
            _statusPanel[0].transform.GetChild(1).GetComponent<Image>().fillAmount = 1 - (PlayerController.instance.slowCurrentTime / PlayerController.instance._slowTime);
        }
        else
        {
            _statusPanel[0].SetActive(false);
            _statusTooltip[0].SetActive(false);
        }

        if (PlayerController.instance._foodTimer > 0f)
        {
            _statusPanel[1].SetActive(true);
            _statusPanel[1].transform.GetChild(2).GetComponent<Image>().fillAmount = 1 - (PlayerController.instance._foodTimer / 4.5f);
        }
        else
        {
            _statusPanel[1].SetActive(false);
            _statusTooltip[1].SetActive(false);
        }

        if (PlayerController.instance._isDash)
        {
            _statusPanel[2].SetActive(true);
        }
        else
        {
            _statusPanel[2].SetActive(false);
            _statusTooltip[2].SetActive(false);
        }

        if (PlayerController.instance._isInvincible)
        {
            _statusPanel[3].SetActive(true);
        }
        else
        {
            _statusPanel[3].SetActive(false);
            _statusTooltip[3].SetActive(false);
        }

        if (PlayerController.instance._isPowerUp)
        {
            _statusPanel[4].SetActive(true);
        }
        else
        {
            _statusPanel[4].SetActive(false);
            _statusTooltip[4].SetActive(false);
        }
        #endregion

        #region 치트 기능
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F10))
        {
            PlayerController.instance._isInvincible = !PlayerController.instance._isInvincible;
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            PlayerController.instance._isPowerUp = !PlayerController.instance._isPowerUp;
        }
#endif
        #endregion

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPause)
            {
                if (!PlayerController.instance._isDying || !PlayerController.instance._isFinishing)
                {
                    Time.timeScale = 0;
                    _pauseUI.SetActive(true);
                    _isPause = true;
                }
            }
            else
            {
                Time.timeScale = 1;
                _pauseUI.SetActive(false);
                _isPause = false;
            }
        }

        _volumeText[0].text = string.Format("{0}%", (int)_volumeSlider[0].value);
        _volumeText[1].text = string.Format("{0}%", (int)_volumeSlider[1].value);
    }

    public void OnHoverStop()
    {
        _statusTooltip[0].SetActive(true);
    }

    public void OnNotHoverStop()
    {
        _statusTooltip[0].SetActive(false);
    }

    public void OnHoverFoodBuff()
    {
        _statusTooltip[1].SetActive(true);
    }

    public void OnNotHoverFoodBuff()
    {
        _statusTooltip[1].SetActive(false);
    }

    public void OnHoverDash()
    {
        _statusTooltip[2].SetActive(true);
    }

    public void OnNotHoverDash()
    {
        _statusTooltip[2].SetActive(false);
    }

    public void OnHoverInvincible()
    {
        _statusTooltip[3].SetActive(true);
    }

    public void OnNotHoverInvincible()
    {
        _statusTooltip[3].SetActive(false);
    }

    public void OnHoverPower()
    {
        _statusTooltip[4].SetActive(true);
    }

    public void OnNotHoverPower()
    {
        _statusTooltip[4].SetActive(false);
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

    public TextMeshProUGUI _deathReportText;
    public GameObject _dieFade, _gameOverUI, _dieReportUI;

    public IEnumerator CheckisDie()
    {
        if (PlayerController.instance._playerHp <= 0)
        {
            DataManager.instance.Delete();
            PlayerController.instance._isDying = true;
            PlayerController.instance._playerAgent.isStopped = true;
            PlayerController.instance._playerRigidbody.bodyType = RigidbodyType2D.Static;
            _dieFade.SetActive(true);
            for (int i = 0; i <= 255; i++)
            {
                _dieFade.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)i);
                yield return new WaitForSeconds(0.001f);
            }
            yield return new WaitForSecondsRealtime(2f);
            _gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void DeathReport()
    {
        _gameOverUI.SetActive(false);
        _dieReportUI.SetActive(true);
        _deathReportText.text = string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "deathReport_day"), DataManager.instance._data.day) + "\n" + string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "deathReport_weapon"), DataManager.instance._data.skillLevel) + "\n" + string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "deathReport_shelter"), DataManager.instance._data.buildLevel);
    }

    public void PauseMenu_Resume()
    {
        Time.timeScale = 1;
        _pauseUI.SetActive(false);
        _isPause = false;
    }

    public void PauseMenu_Options()
    {
        _pauseUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        _pauseUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        _volumeSlider[0].value = DataManager.instance._data.musicVolume;
        _volumeSlider[1].value = DataManager.instance._data.effectVolume;
        _isActivated = true;
    }

    public void PauseMenu_Quit()
    {
        Time.timeScale = 1;
        StartCoroutine(ReturnToHomeFade());
    }

    bool _isActivated = false;

    public void OnSliderValueChanged()
    {
        if (_isActivated)
        {
            DataManager.instance._data.musicVolume = (int)_volumeSlider[0].value;
            DataManager.instance._data.effectVolume = (int)_volumeSlider[1].value;
            DataManager.instance.Save();
        }
    }

    public void PauseMenu_Options_Back()
    {
        _pauseUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        _pauseUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    }

    public IEnumerator ReturnToHomeFade()
    {
        PlayerController.instance._completeFade.SetActive(true);
        for (int i = 0; i <= 255; i++)
        {
            PlayerController.instance._completeFade.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)i);
            yield return new WaitForSeconds(0.001f);
        }
        SceneManager.LoadScene("Loading_ToHome");
    }
}
