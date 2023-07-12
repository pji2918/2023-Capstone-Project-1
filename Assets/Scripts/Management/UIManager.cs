using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using UnityEngine.Localization.Settings;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region 팝업창
    [SerializeField]//업그레이드창
    public GameObject upgradeWindow;
    [SerializeField]//무기 업그레이드창
    public GameObject weaponWindow;
    [SerializeField]//집 업그레이드창
    private GameObject houseWindow;
    [SerializeField]//스토리 창
    private GameObject bookWindow;
    [SerializeField]//설정창
    private GameObject Option;
    #endregion

    #region 텍스트
    [SerializeField]//강화중...타이핑 텍스트
    private TextMeshProUGUI upgradeText;
    [SerializeField]//건축중... 타이핑 텍스트
    private TextMeshProUGUI buildText;
    [SerializeField]//무기 강화 상태 확인 텍스트
    private TextMeshProUGUI upgradeSubText;
    [SerializeField]//집 강화 상태 확인 텍스트
    private TextMeshProUGUI buildSubText;
    [SerializeField]//음식 제작 상태확인 텍스트(남은 시간 확인용 텍스트)
    private TextMeshProUGUI foodText;
    [SerializeField]//음식 제작 상태확인 텍스트
    private TextMeshProUGUI foodSubText;
    [SerializeField]//제작 완료된 음식 개수 확인 텍스트
    private TextMeshProUGUI foodCountText;

    [SerializeField] private TextMeshProUGUI[] _hpText = new TextMeshProUGUI[2];
    [SerializeField] private TextMeshProUGUI[] _atkText = new TextMeshProUGUI[2];
    [SerializeField] private TextMeshProUGUI[] _speedText = new TextMeshProUGUI[2];

    //재료 개수 확인 텍스트 모음
    private TextMeshProUGUI[] resourceTexts = new TextMeshProUGUI[6];
    //무기 필요 재료 확인 텍스트 모음
    private TextMeshProUGUI[] needWeaponTexts = new TextMeshProUGUI[4];
    //집 필요 재료 확인 텍스트 모음
    private TextMeshProUGUI[] needBuildTexts = new TextMeshProUGUI[4];

    [SerializeField] private TextMeshProUGUI _shelterLevelText;
    [SerializeField] private TextMeshProUGUI _weaponLevelText;
    #endregion

    #region 기타
    [SerializeField]
    Tutorial tutorial;
    [SerializeField]//무기 강화시 필요한 재료 확인 텍스트 묶음(빈 오브젝트)
    private GameObject needWeaponResource;
    [SerializeField]//집 강화시 필요한 재료 확인 텍스트 묶음(빈 오브젝트)
    private GameObject needHouseResource;
    [SerializeField]//재료 개수 확인 텍스트의 부모 오브젝트(빈 오브젝트)
    private GameObject ResourceTextsParent;
    [SerializeField]//필요 재료 개수 확인 텍스트의 부모 오브젝트(빈 오브젝트, 무기업글)
    private GameObject needWeaponResourceTextsParent;
    [SerializeField]//필요 재료 개수 확인 텍스트의 부모 오브젝트(빈 오브젝트, 집 업글)
    private GameObject needHouseResourceTextsParent;
    [SerializeField]
    public GameObject _statWindow;

    [SerializeField]
    private RectTransform _bookFit;

    int _storyNum;

    // 책 텍스트
    public TextMeshProUGUI _title, _desc;
    public GameObject _letter;

    //음식 제작 게이지
    private Slider foodSlider;

    public Animator _cooker;

    //음식 제작 시간
    private float cookingTime = 30;

    private bool _lookingStory = false;
    NeedResourse _needResourse = new NeedResourse();

    public TextMeshProUGUI wLevel; //무기 강화 레벨 텍스트
    public TextMeshProUGUI bLevel; // 쉘터 강화 레벨

    [SerializeField]
    private Image weaponImage;
    [SerializeField]
    private Sprite[] weaponSprites = new Sprite[6];

    [SerializeField]
    private Image shelterImage;
    [SerializeField]
    private Sprite[] shelterSprites = new Sprite[4];

    private int _deleteCount = 0;

    [SerializeField] private Button[] _optionTabs;
    [SerializeField] private GameObject[] _optionPanels;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private List<Resolution> _resolutions = new List<Resolution>();
    [SerializeField] private TMP_Dropdown _fullScreenModeDropdown;
    [SerializeField] private Button[] _upgradeButtons;
    private bool _resolutionLocked = true;
    private bool _languageLocked = true;
    #endregion

    IEnumerator ChangeLang()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        if (DataManager.instance._data.language == Language.Auto)
        {
            var currentCulture = CultureInfo.InstalledUICulture;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(currentCulture.Name);
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)DataManager.instance._data.language - 1];
        }
    }

    private void Start()
    {
        StartCoroutine(ChangeLang());
        List<string> args = System.Environment.GetCommandLineArgs().ToList();

        if (args.Contains("--safemode"))
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }

        _storyNum = Random.Range(0, 5);
        for (int i = 0; i < 6; i++)
        {
            resourceTexts[i] = ResourceTextsParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            Debug.Log("재료 텍스트 : " + resourceTexts[i].name);

            if (i < 4)
            {
                needWeaponTexts[i] = needWeaponResourceTextsParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                needBuildTexts[i] = needHouseResourceTextsParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();

                Debug.Log("무기 강화 필요 재료 텍스트" + needWeaponTexts[i].name);
                Debug.Log("쉘터 강화 필요 재료 텍스트" + needBuildTexts[i].name);
            }
        }

        foodSlider = GameObject.Find("Slider").GetComponent<Slider>();

        foodText.text = "";
        foodSubText.text = "";
        wLevel.text = "무기 레벨 :" + DataManager.instance._data.skillLevel;
        bLevel.text = "쉘터 레벨 :" + DataManager.instance._data.buildLevel;

        switch (DataManager.instance._data.skillLevel)
        {
            case 0:
                weaponImage.sprite = weaponSprites[0];
                break;
            case 2:
                weaponImage.sprite = weaponSprites[1];
                break;
            case 5:
                weaponImage.sprite = weaponSprites[2];
                break;
            case 8:
                weaponImage.sprite = weaponSprites[3];
                break;
            case 11:
                weaponImage.sprite = weaponSprites[4];
                break;
            default:
                weaponImage.sprite = weaponSprites[5];
                break;
        }
        switch (DataManager.instance._data.buildLevel)
        {
            case 2:
            case 3:
            case 4:
                shelterImage.sprite = shelterSprites[0];
                break;
            case 5:
            case 6:
            case 7:
                shelterImage.sprite = shelterSprites[1];
                break;
            case 8:
            case 9:
                shelterImage.sprite = shelterSprites[2];
                break;
            case 10:
                shelterImage.sprite = shelterSprites[3];
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        ShowResource();
        if (GameManager.instance._isCooking)
        {
            foodSlider.value = GameManager.instance._currentCookingTime / cookingTime;

            foodText.text = "음식 제작중... 남은 시간: " + (int)(cookingTime - GameManager.instance._currentCookingTime) + "초";

            if (GameManager.instance._currentCookingTime >= cookingTime)
            {
                StartCoroutine(TextClear(foodText, "제작이 완료되었습니다"));

                foodSlider.value = 0;
            }
        }
        _cooker.SetBool("isCooking", GameManager.instance._isCooking);

        if (Input.GetKeyDown(KeyCode.F10))
        {
            _storyNum = Random.Range(0, 5);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            DataManager.instance._data.day = 1;
            DataManager.instance.Save();
        }
    }

    #region 버튼 함수

    //업그레이드 창 팝업
    public void OnClickUpgradeButton()
    {
        if (DataManager.instance._data.day == 2)
        {
            tutorial.StepUp2();
        }

        WindowPopUp(upgradeWindow);
    }

    //무기 업그레이드 창 팝업
    public void OnClickWeaponButton()
    {
        ShowNeedWeaponResourse();

        if (DataManager.instance._data.day == 2)
        {
            tutorial.StepUp2();
        }

        PopUpWidowChange(weaponWindow, upgradeWindow);
    }

    public void OnClickStatButton()
    {
        if (DataManager.instance._data.day == 2)
        {
            tutorial.StepUp2();
        }

        WindowPopUp(_statWindow);

        ChangeStatsText();
    }

    public void OnCloseStatButton()
    {
        _statWindow.SetActive(false);
    }

    [SerializeField] private GameObject[] _skillLocks;

    public void ChangeStatsText()
    {
        if (DataManager.instance._data.skillLevel >= 1)
        {
            _skillLocks[0].SetActive(false);
        }
        if (DataManager.instance._data.skillLevel >= 3)
        {
            _skillLocks[1].SetActive(false);
        }
        if (DataManager.instance._data.skillLevel >= 6)
        {
            _skillLocks[2].SetActive(false);
        }
        if (DataManager.instance._data.skillLevel >= 9)
        {
            _skillLocks[3].SetActive(false);
        }
        if (DataManager.instance._data.skillLevel >= 12)
        {
            _skillLocks[4].SetActive(false);
        }

        _shelterLevelText.text = string.Format("쉘터 레벨 : {0}", DataManager.instance._data.buildLevel);
        _weaponLevelText.text = string.Format("무기 레벨 : {0}", DataManager.instance._data.skillLevel);

        _hpText[0].text = DataManager.instance._playerStat.maxHp[DataManager.instance._data.buildLevel].ToString();
        _speedText[0].text = DataManager.instance._playerStat.moveSpeed[DataManager.instance._data.buildLevel].ToString();

        if (DataManager.instance._data.buildLevel >= 10)
        {
            _hpText[1].gameObject.SetActive(false);
            _speedText[1].gameObject.SetActive(false);
        }
        else
        {
            _hpText[1].text = string.Format("+{0}", DataManager.instance._playerStat.maxHp[DataManager.instance._data.buildLevel + 1] - DataManager.instance._playerStat.maxHp[DataManager.instance._data.buildLevel]);
            _speedText[1].text = string.Format("+{0}", DataManager.instance._playerStat.moveSpeed[DataManager.instance._data.buildLevel + 1] - DataManager.instance._playerStat.moveSpeed[DataManager.instance._data.buildLevel]);
        }

        _atkText[0].text = DataManager.instance._playerStat.atk[DataManager.instance._data.skillLevel].ToString();

        if (DataManager.instance._data.skillLevel >= 15)
        {
            _atkText[1].gameObject.SetActive(false);
        }
        else
        {
            _atkText[1].text = string.Format("+{0}", DataManager.instance._playerStat.atk[DataManager.instance._data.skillLevel + 1] - DataManager.instance._playerStat.atk[DataManager.instance._data.skillLevel]);
        }
    }

    //무기 강화 버튼 클릭
    public void OnClickweaponUpgradeButton()
    {
        if (DataManager.instance._data.day == 2)
        {
            tutorial.StepUp2();
        }

        StopCoroutine("Typing");
        if (DataManager.instance._data.resources["iron"] >= DataManager.instance._data.weaponUpgrade["iron"]
        && DataManager.instance._data.resources["concrete"] >= DataManager.instance._data.weaponUpgrade["concrete"]
        && DataManager.instance._data.resources["bolt"] >= DataManager.instance._data.weaponUpgrade["bolt"]
        && DataManager.instance._data.resources["core"] >= DataManager.instance._data.weaponUpgrade["core"] && DataManager.instance._data.skillLevel < 15)
        {
            _upgradeButtons[0].interactable = false;
            PopUpWidowChange(upgradeText.gameObject, needWeaponResource);

            DataManager.instance._data.resources["iron"] -= DataManager.instance._data.weaponUpgrade["iron"];
            DataManager.instance._data.resources["concrete"] -= DataManager.instance._data.weaponUpgrade["concrete"];
            DataManager.instance._data.resources["bolt"] -= DataManager.instance._data.weaponUpgrade["bolt"];
            DataManager.instance._data.resources["core"] -= DataManager.instance._data.weaponUpgrade["core"];

            StartCoroutine(Typing(upgradeText, "강 화 중 . . .", 0.5f, upgradeText.gameObject, needWeaponResource, 1));
        }
        else if (DataManager.instance._data.skillLevel >= 15)
        {
            StartCoroutine(TextClear(upgradeSubText, "이미 무기를 최대로 강화했습니다"));
        }
        else
        {
            StartCoroutine(TextClear(upgradeSubText, "재료가 부족합니다"));
        }
    }

    //집 업그레이드 창 팝업
    public void OnClickHouseButton()
    {
        ShowNeedBuildResourse();

        PopUpWidowChange(houseWindow, upgradeWindow);
    }

    //집 강화 버튼 클릭(위와 같이 아직 기능 부족)
    public void OnClickHouseUpgradeButton()
    {
        if (DataManager.instance._data.resources["iron"] >= DataManager.instance._data.HouseUpgrade["iron"]
        && DataManager.instance._data.resources["concrete"] >= DataManager.instance._data.HouseUpgrade["concrete"]
        && DataManager.instance._data.resources["bolt"] >= DataManager.instance._data.HouseUpgrade["bolt"]
        && DataManager.instance._data.resources["core"] >= DataManager.instance._data.HouseUpgrade["core"]
        && DataManager.instance._data.buildLevel < 10)
        {
            _upgradeButtons[1].interactable = false;
            PopUpWidowChange(buildText.gameObject, needHouseResource);

            DataManager.instance._data.resources["iron"] -= DataManager.instance._data.HouseUpgrade["iron"];
            DataManager.instance._data.resources["concrete"] -= DataManager.instance._data.HouseUpgrade["concrete"];
            DataManager.instance._data.resources["bolt"] -= DataManager.instance._data.HouseUpgrade["bolt"];
            DataManager.instance._data.resources["core"] -= DataManager.instance._data.HouseUpgrade["core"];

            StartCoroutine(Typing(buildText, "건 설 중 . . .", 0.5f, buildText.gameObject, needHouseResource, 2));
        }
        else if (DataManager.instance._data.buildLevel >= 10)
        {
            StartCoroutine(TextClear(buildSubText, "이미 집을 최대로 강화했습니다"));
        }
        else
        {
            StartCoroutine(TextClear(buildSubText, "재료가 부족합니다"));
        }
    }

    //음식 제작(재료 아이템 제거하는거 있음)
    public void OnClickFoodButton()
    {
        if (DataManager.instance._data.resources["ingredient"] >= 5 && !GameManager.instance._isCooking)
        {
            GameManager.instance.ResourceReduction("ingredient", 5);
            GameManager.instance._isCooking = true;
        }
        else if (!GameManager.instance._isCooking)
        {
            StartCoroutine(TextClear(foodSubText, "재료가 부족합니다..."));
        }
        else
        {
            StartCoroutine(TextClear(foodSubText, "제작 중 추가 제작을 할 수 없습니다"));
        }

        Debug.Log("식량제작버튼 클릭");

        if (DataManager.instance._data.day == 2)
        {
            tutorial.StepUp2();
        }
    }

    //식량 회수
    public void OnClickFoodRecallButton()
    {
        if (GameManager.instance.foodCount != 0)
        {
            GameManager.instance.IncreseResource("food", GameManager.instance.foodCount);
            GameManager.instance.foodCount = 0;
        }
        else
        {
            StartCoroutine(TextClear(foodSubText, "제작 완료된 음식이 없습니다"));
        }

        Debug.Log("식량회수버튼 클릭");
    }

    //스토리 팝업
    public void OnClickTableButton()
    {
        WindowPopUp(bookWindow);
        BookOpen();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_bookFit);
    }

    //설정 팝업
    public void OnClickOption()
    {
        WindowPopUp(Option);
        OptionTabs("audio");
    }

    //업그레이드 팝업 나가기
    public void OnClickUpgradeBackground()
    {
        WindowDisappear(upgradeWindow);
    }

    //스토리 팝업 나가기
    public void OnClickTableBackground()
    {
        if (!_lookingStory)
        {
            WindowDisappear(bookWindow);
        }
    }

    //설정 팝업 나가기
    public void OnClickOptionBackground()
    {
        _deleteCount = 0;
        _deleteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_delete");
        DataManager.instance._data.resolution = Screen.currentResolution;
        DataManager.instance.Save();
        _resolutionLocked = true;
        WindowDisappear(Option);
    }
    #endregion

    //팝업 하나 키고 하나 끄기
    void PopUpWidowChange(GameObject popUp, GameObject window)
    {
        popUp.SetActive(!popUp.activeSelf);
        window.SetActive(!window.activeSelf);
    }

    //팝업 끄기
    void WindowDisappear(GameObject popUp)
    {
        popUp.SetActive(false);
    }

    //팝업 시키기
    void WindowPopUp(GameObject popUp)
    {
        popUp.SetActive(true);
    }

    //씬 넘기기
    public void HomeToFighting()
    {
        DataManager.instance.Save();
        StartCoroutine(Fade());
    }

    public GameObject _sceneChangeFade;

    IEnumerator Fade()
    {
        _sceneChangeFade.SetActive(true);
        float elapsedTime = 0f;
        float fadeTime = 1f; // fade time in seconds
        Color32 startColor = new Color32(0, 0, 0, 0);
        Color32 endColor = new Color32(0, 0, 0, 255);
        while (elapsedTime < fadeTime)
        {
            _sceneChangeFade.GetComponent<Image>().color = Color32.Lerp(startColor, endColor, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Loading");
    }

    //식량 개수 표시
    private void ShowResource()
    {
        resourceTexts[0].text = DataManager.instance._data.resources["iron"].ToString();
        resourceTexts[1].text = DataManager.instance._data.resources["concrete"].ToString();
        resourceTexts[2].text = DataManager.instance._data.resources["bolt"].ToString();
        resourceTexts[3].text = DataManager.instance._data.resources["core"].ToString();
        resourceTexts[4].text = DataManager.instance._data.resources["ingredient"].ToString();
        resourceTexts[5].text = DataManager.instance._data.resources["food"].ToString();
        foodCountText.text = GameManager.instance.foodCount.ToString();
    }

    //무기 강화 재료 필요 개수 표시
    private void ShowNeedWeaponResourse()
    {
        _needResourse.ChangeResourseAmount();

        needWeaponTexts[0].text = DataManager.instance._data.weaponUpgrade["iron"].ToString();
        needWeaponTexts[1].text = DataManager.instance._data.weaponUpgrade["concrete"].ToString();
        needWeaponTexts[2].text = DataManager.instance._data.weaponUpgrade["bolt"].ToString();
        needWeaponTexts[3].text = DataManager.instance._data.weaponUpgrade["core"].ToString();

        wLevel.text = "무기 레벨 : " + DataManager.instance._data.skillLevel;

        for (int i = 0; i <= 3; i++)
        {
            if (DataManager.instance._data.resources.ElementAt(i).Value < DataManager.instance._data.weaponUpgrade.ElementAt(i).Value)
            {
                needWeaponTexts[i].color = Color.red;
            }
            else
            {
                needWeaponTexts[i].color = Color.black;
            }
        }
    }

    //집 강화 재료 필요 개수 표시
    private void ShowNeedBuildResourse()
    {
        _needResourse.ChangeResourseAmount();

        needBuildTexts[0].text = DataManager.instance._data.HouseUpgrade["iron"].ToString();
        needBuildTexts[1].text = DataManager.instance._data.HouseUpgrade["concrete"].ToString();
        needBuildTexts[2].text = DataManager.instance._data.HouseUpgrade["bolt"].ToString();
        needBuildTexts[3].text = DataManager.instance._data.HouseUpgrade["core"].ToString();

        for (int i = 0; i <= 3; i++)
        {
            if (DataManager.instance._data.resources.ElementAt(i).Value < DataManager.instance._data.HouseUpgrade.ElementAt(i).Value)
            {
                needBuildTexts[i].color = Color.red;
            }
            else
            {
                needBuildTexts[i].color = Color.black;
            }
        }

        bLevel.text = "쉘터 레벨 : " + DataManager.instance._data.buildLevel;
    }

    private void ChangeWLevel()
    {
        DataManager.instance._data.skillLevel++;

        DataManager.instance.Save();

        ShowNeedWeaponResourse();
    }

    private void ChangeBLevel()
    {
        DataManager.instance._data.buildLevel++;

        DataManager.instance.Save();

        ShowNeedBuildResourse();
    }

    //타이핑 효과 코루틴
    IEnumerator Typing(TextMeshProUGUI typingText, string message, float speed, GameObject popUp, GameObject window, int type)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
        //TODO:effect anime
        yield return new WaitForSeconds(1);
        _upgradeButtons[0].interactable = true;
        _upgradeButtons[1].interactable = true;
        PopUpWidowChange(popUp, window);
        if (type == 1)
        {
            ChangeWLevel();
        }
        else
        {
            ChangeBLevel();
        }

        switch (DataManager.instance._data.skillLevel)
        {
            case 0:
                weaponImage.sprite = weaponSprites[0];
                break;
            case 2:
                weaponImage.sprite = weaponSprites[1];
                break;
            case 5:
                weaponImage.sprite = weaponSprites[2];
                break;
            case 8:
                weaponImage.sprite = weaponSprites[3];
                break;
            case 11:
                weaponImage.sprite = weaponSprites[4];
                break;
            default:
                weaponImage.sprite = weaponSprites[5];
                break;
        }
        switch (DataManager.instance._data.buildLevel)
        {
            case 2:
                shelterImage.sprite = shelterSprites[0];
                break;
            case 5:
                shelterImage.sprite = shelterSprites[1];
                break;
            case 8:
                shelterImage.sprite = shelterSprites[2];
                break;
            case 10:
                shelterImage.sprite = shelterSprites[3];
                break;
            default:
                break;
        }
    }

    //텍스트 1초 띄우기
    IEnumerator TextClear(TextMeshProUGUI text, string messege)
    {
        text.text = messege;
        yield return new WaitForSeconds(1f);
        text.text = " ";
    }

    public void Exit()
    {
        DataManager.instance.Save();
        Application.Quit();
    }

    public void BookOpen()
    {
        _title.text = string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "report"), DataManager.instance._data.day);
        if (DataManager.instance._data.day == 1)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay01Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 2)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay02Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 3)
        {
            _desc.text = string.Format("{0}\n\n{1}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03Text1"), LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03Text2"));
            _letter.SetActive(true);
            ShowStoryButton(ButtonType.Choice, 6);
        }
        else if (DataManager.instance._data.day == 5)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay05Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 7)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay07Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 8)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay08Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 9)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay09Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 13)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay13Text");
            ShowStoryButton(ButtonType.Choice, 7);
        }
        else if (DataManager.instance._data.day == 15 && DataManager.instance._data.buildLevel >= 2)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay15Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 18)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay18Text");
            ShowStoryButton(ButtonType.Choice, 8);
        }
        else if (DataManager.instance._data.day == 20)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay20Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 21 && DataManager.instance._data.buildLevel >= 5)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay21Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 23)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay23Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 24)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay24Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 26 && DataManager.instance._data.buildLevel >= 8)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay26Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 28)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay28Text");
            ShowStoryButton(ButtonType.Choice, 9);
        }
        else if (DataManager.instance._data.day == 34)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay34Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.day == 35)
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay28Text");
            ShowStoryButton(ButtonType.Normal);
        }
        else if (DataManager.instance._data.buildLevel == 10) // 이건 모든 게 완성되었을 때 추가됩니다.
        {
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyEndText");
            ShowStoryButton(ButtonType.Normal);
        }
        else
        {
            int loop = 0;
            while (loop <= 5)
            {
                ++loop;
                switch (_storyNum)
                {
                    case 0:
                        {
                            if (DataManager.instance._data.day >= 4)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04Text");
                                ShowStoryButton(ButtonType.Choice, 1);
                                goto Escape;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    case 1:
                        {
                            if (DataManager.instance._data.day < 13)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryUntilDay13Text");
                                ShowStoryButton(ButtonType.Choice, 2);
                                goto Escape;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    case 2:
                        {
                            if (DataManager.instance._data.day >= 6)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay06Text");
                                ShowStoryButton(ButtonType.Choice, 3);
                                goto Escape;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    case 3:
                        {
                            if (DataManager.instance._data.day > 13)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay13Text");
                                ShowStoryButton(ButtonType.Choice, 4);
                                goto Escape;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    case 4:
                        {
                            if (DataManager.instance._data.day > 18)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18Text");
                                ShowStoryButton(ButtonType.Choice, 5);
                                goto Escape;
                            }
                            else
                            {
                                continue;
                            }
                        }
                }
            }
            if (loop > 5)
            {
                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryDefaultText");
                ShowStoryButton(ButtonType.Normal);
            }
        Escape:
            {

            }
        }
    }

    public enum ButtonType
    {
        Normal,
        Choice
    }

    int eventNum = 0;

    public Button _deleteButton;
    public void DeleteFile()
    {
        _deleteCount++;
        if (_deleteCount < 5)
        {
            _deleteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_deletewarn"), 5 - _deleteCount);
        }
        else
        {
            DataManager.instance.Delete();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ShowStoryButton(ButtonType type, int eNum = 0)
    {
        _normal.SetActive(false);
        _choice.SetActive(false);
        switch (type)
        {
            case ButtonType.Normal:
                {
                    _normal.SetActive(true);
                    break;
                }
            case ButtonType.Choice:
                {
                    _choice.SetActive(true);
                    eventNum = eNum;
                    switch (eventNum)
                    {
                        case 1:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04TextChoice1"));
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04TextChoice2"));
                                break;
                            }
                        case 2:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryUntilDay13TextChoice1"));
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryUntilDay13TextChoice2"));
                                break;
                            }
                        case 3:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay06TextChoice1"));
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay06TextChoice2"));
                                break;
                            }
                        case 4:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay13TextChoice1"));
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay13TextChoice2"));
                                break;
                            }
                        case 5:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18TextChoice1"));
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18TextChoice2"));
                                break;
                            }
                        case 6:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03TextChoice1"));
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03TextChoice2"));
                                break;
                            }
                        case 7:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay13TextChoice1"));
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay13TextChoice2"));
                                break;
                            }
                        case 8:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay18TextChoice1"));
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay18TextChoice2"));
                                break;
                            }
                        case 9:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay28TextChoice1"));
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("▶ {0}", LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay28TextChoice2"));
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    _choice.SetActive(true);
                    break;
                }
        }
    }

    public GameObject _choice, _normal;
    public GameObject[] _choiceButtons;

    public void ChoiceOne()
    {
        _lookingStory = true;
        switch (eventNum)
        {
            case 1:
                {
                    if (Random.Range(0, 100) <= 70)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04TextChoice1Text1");
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04TextChoice1Text2");
                        ShowStoryButton(ButtonType.Normal);
                        GameManager.instance._healthReduce = 10;
                    }
                    break;
                }
            case 2:
                {
                    if (Random.Range(0, 100) <= 20)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryUntilDay13TextChoice1Text1");
                        DataManager.instance._data.resources["core"] += 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryUntilDay13TextChoice1Text2");
                        ShowStoryButton(ButtonType.Normal);
                        GameManager.instance._healthReduce = 5;
                    }
                    break;
                }
            case 3:
                {
                    if (Random.Range(0, 100) <= 20)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay06TextChoice1Text1");
                        DataManager.instance._data.resources["food"] += 2;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay06TextChoice1Text2");
                        ShowStoryButton(ButtonType.Normal);
                        GameManager.instance._healthReduce = 5;
                    }
                    break;
                }
            case 4:
                {
                    if (Random.Range(0, 100) <= 80)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay13TextChoice1Text1");
                        DataManager.instance._data.resources["core"] -= 1;
                        DataManager.instance._data.resources["concrete"] += 5;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay13TextChoice1Text2");
                        DataManager.instance._data.resources["core"] -= 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    break;
                }
            case 5:
                {
                    if (Random.Range(0, 100) <= 40)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18TextChoice1Text1");
                        DataManager.instance._data.resources["bolt"] -= 3;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18TextChoice1Text2");
                        DataManager.instance._data.resources["bolt"] -= 3;
                        DataManager.instance._data.resources["food"] += 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    break;
                }
            case 6:
                {
                    if (Random.Range(0, 100) <= 70)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03TextChoice1Text1");
                        GameManager.instance._healthReduce = 10;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03TextChoice1Text2");
                        DataManager.instance._data.resources["ingredient"] += 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    break;
                }
            case 7:
                {
                    if (Random.Range(0, 100) <= 80)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay13TextChoice1Text1");
                        DataManager.instance._data.resources["iron"] += 5;
                        DataManager.instance._data.resources["food"] -= 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay13TextChoice1Text2");
                        DataManager.instance._data.resources["ingredient"] -= 2;
                        DataManager.instance._data.resources["food"] -= 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    break;
                }
            case 8:
                {
                    if (Random.Range(0, 100) <= 60)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay18TextChoice1Text1");
                        DataManager.instance._data.resources["iron"] -= 3;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay18TextChoice1Text2");
                        DataManager.instance._data.resources["ingredient"] += 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    break;
                }
            case 9:
                {
                    if (Random.Range(0, 100) <= 60)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay28TextChoice1Text1");
                        DataManager.instance._data.resources["bolt"] += 15;
                        DataManager.instance._data.resources["core"] -= 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay28TextChoice1Text2");
                        DataManager.instance._data.resources["bolt"] += 15;
                        DataManager.instance._data.resources["core"] += 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    break;
                }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_bookFit);
    }

    public void ChoiceTwo()
    {
        _lookingStory = true;
        switch (eventNum)
        {
            case 1:
                {
                    if (Random.Range(0, 100) <= 50)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04TextChoice2Text1");
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04TextChoice2Text2");
                        ShowStoryButton(ButtonType.Normal);
                        GameManager.instance._healthReduce = 10;
                    }
                    break;
                }
            case 2:
                {
                    _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryUntilDay13TextChoice2Text");
                    ShowStoryButton(ButtonType.Normal);
                    break;
                }
            case 3:
                {
                    _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay06TextChoice2Text");
                    ShowStoryButton(ButtonType.Normal);
                    break;
                }
            case 4:
                {
                    _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay13TextChoice2Text");
                    ShowStoryButton(ButtonType.Normal);
                    break;
                }
            case 5:
                {
                    if (Random.Range(0, 100) <= 20)
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18TextChoice1Text1");
                        DataManager.instance._data.resources["concrete"] -= 1;
                        DataManager.instance._data.resources["core"] += 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    else
                    {
                        _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18TextChoice1Text2");
                        DataManager.instance._data.resources["concrete"] -= 1;
                        ShowStoryButton(ButtonType.Normal);
                    }
                    break;
                }
            case 6:
                {
                    _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03TextChoice2Text");
                    ShowStoryButton(ButtonType.Normal);
                    break;
                }
            case 7:
                {
                    _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay13TextChoice2Text");
                    ShowStoryButton(ButtonType.Normal);
                    break;
                }
            case 8:
                {
                    _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay18TextChoice2Text");
                    ShowStoryButton(ButtonType.Normal);
                    break;
                }
            case 9:
                {
                    _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay28TextChoice2Text");
                    ShowStoryButton(ButtonType.Normal);
                    break;
                }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_bookFit);
    }

    public TextMeshProUGUI _infoText;
    public TMP_Dropdown _languageDropdown;

    public void OptionTabs(string button)
    {
        switch (button)
        {
            case "audio":
                {
                    _deleteCount = 0;
                    _deleteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_delete");

                    _languageLocked = true;
                    _resolutionLocked = true;
                    _optionTabs[0].interactable = false;
                    _optionTabs[1].interactable = true;
                    _optionTabs[2].interactable = true;
                    _optionTabs[3].interactable = true;

                    _optionPanels[0].SetActive(true);
                    _optionPanels[1].SetActive(false);
                    _optionPanels[2].SetActive(false);
                    _optionPanels[3].SetActive(false);

                    _soundSliders[0].value = DataManager.instance._data.musicVolume;
                    _soundSliders[1].value = DataManager.instance._data.effectVolume;
                    _soundTexts[0].text = string.Format("{0}%", DataManager.instance._data.musicVolume);
                    _soundTexts[1].text = string.Format("{0}%", DataManager.instance._data.effectVolume);
                    break;
                }
            case "video":
                {
                    _deleteCount = 0;
                    _deleteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_delete");

                    _languageLocked = true;
                    _optionTabs[0].interactable = true;
                    _optionTabs[1].interactable = false;
                    _optionTabs[2].interactable = true;
                    _optionTabs[3].interactable = true;

                    _optionPanels[0].SetActive(false);
                    _optionPanels[1].SetActive(true);
                    _optionPanels[2].SetActive(false);
                    _optionPanels[3].SetActive(false);

                    _fullScreenModeDropdown.options.Clear();

                    _fullScreenModeDropdown.options.Add(new TMP_Dropdown.OptionData(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_fullscreen")));
                    _fullScreenModeDropdown.options.Add(new TMP_Dropdown.OptionData(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_borderless")));
                    _fullScreenModeDropdown.options.Add(new TMP_Dropdown.OptionData(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_windowed")));

                    _fullScreenModeDropdown.value = (int)DataManager.instance._data.fullScreenMode;

                    _resolutions.Clear();

                    for (int i = 0; i < Screen.resolutions.Length; i++)
                    {
                        if (Screen.resolutions[i].refreshRate == 60 || Screen.resolutions[i].refreshRate == 120 || Screen.resolutions[i].refreshRate == 144 || Screen.resolutions[i].refreshRate == 240)
                        {
                            _resolutions.Add(Screen.resolutions[i]);
                        }
                    }

                    _resolutionDropdown.options.Clear();

                    foreach (Resolution item in _resolutions)
                    {
                        TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                        optionData.text = string.Format("{0}x{1} ({2}Hz)", item.width, item.height, item.refreshRate);
                        _resolutionDropdown.options.Add(optionData);
                    }
                    _resolutionDropdown.RefreshShownValue();
                    try
                    {
                        _resolutionDropdown.value = _resolutions.IndexOf(Screen.currentResolution);
                    }
                    catch
                    {
                        _resolutionDropdown.value = 0;
                    }
                    _resolutionLocked = false;
                    break;
                }
            case "accessibility":
                {
                    _deleteCount = 0;
                    _deleteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_delete");

                    _optionTabs[0].interactable = true;
                    _optionTabs[1].interactable = true;
                    _optionTabs[2].interactable = false;
                    _optionTabs[3].interactable = true;

                    _optionPanels[0].SetActive(false);
                    _optionPanels[1].SetActive(false);
                    _optionPanels[2].SetActive(true);
                    _optionPanels[3].SetActive(false);

                    _3dAudioToggle.isOn = DataManager.instance._data.is3dAudio;
                    _screenVibrationToggle.isOn = DataManager.instance._data.isScreenVibration;
                    _damageToggle.isOn = DataManager.instance._data.displayDamage;
                    break;
                }
            case "other":
                {
                    _deleteCount = 0;
                    _deleteButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_delete");

                    _resolutionLocked = true;
                    _optionTabs[0].interactable = true;
                    _optionTabs[1].interactable = true;
                    _optionTabs[2].interactable = true;
                    _optionTabs[3].interactable = false;

                    _optionPanels[0].SetActive(false);
                    _optionPanels[1].SetActive(false);
                    _optionPanels[2].SetActive(false);
                    _optionPanels[3].SetActive(true);

                    _languageDropdown.options.Clear();

                    _languageDropdown.options.Add(new TMP_Dropdown.OptionData(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_lang_usesystem").ToString()));

                    // 게임이 지원하는 모든 언어를 가져온다.
                    for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
                    {
                        TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                        // optionData에 언어 이름을 넣는다. 형식은 "현지화된 언어 이름 (영어로 된 언어 이름)"이다.
                        if (LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code == "en")
                        {
                            optionData.text = string.Format("[WIP] {0} ({1})", new CultureInfo(LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code).NativeName, new CultureInfo(LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code));
                            _languageDropdown.options.Add(optionData);
                        }
                        else
                        {
                            optionData.text = string.Format("{0} ({1})", new CultureInfo(LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code).NativeName, new CultureInfo(LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code));
                            _languageDropdown.options.Add(optionData);
                        }
                    }

                    _languageDropdown.value = (int)DataManager.instance._data.language;
                    _languageLocked = false;

                    _infoText.text = string.Format(LocalizationSettings.StringDatabase.GetLocalizedString("UI", "options_info_text"), Application.version);
                    break;
                }
        }
    }



    public void OnChangeFullScreen(int value)
    {
        if (!_resolutionLocked)
        {
            FullScreenMode mode = FullScreenMode.ExclusiveFullScreen;
            switch (value)
            {
                case 0:
                    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                        mode = FullScreenMode.ExclusiveFullScreen;
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                        mode = FullScreenMode.MaximizedWindow;
#elif UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
                        mode = FullScreenMode.FullScreenWindow;
#endif
                        break;
                    }
                case 1:
                    {
                        mode = FullScreenMode.FullScreenWindow;
                        break;
                    }
                case 2:
                    {
                        mode = FullScreenMode.Windowed;
                        break;
                    }
            }
            DataManager.instance._data.fullScreenMode = mode;
            Screen.fullScreenMode = DataManager.instance._data.fullScreenMode;
        }
    }

    public void OnLanguageChange(int value)
    {
        if (!_languageLocked)
        {
            DataManager.instance._data.language = (Language)value;
            DataManager.instance.Save();
            StartCoroutine(ChangeLang());
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public Slider[] _soundSliders;
    public Toggle _3dAudioToggle;
    public Toggle _screenVibrationToggle, _damageToggle;
    public TextMeshProUGUI[] _soundTexts;

    public void OnValueChange(string type)
    {
        switch (type)
        {
            case "music":
                {
                    DataManager.instance._data.musicVolume = System.Convert.ToInt32(_soundSliders[0].value);
                    _soundTexts[0].text = DataManager.instance._data.musicVolume.ToString();
                    break;
                }
            case "sound":
                {
                    DataManager.instance._data.effectVolume = System.Convert.ToInt32(_soundSliders[1].value);
                    _soundTexts[1].text = DataManager.instance._data.effectVolume.ToString();
                    break;
                }
            case "3dAudio":
                {
                    DataManager.instance._data.is3dAudio = _3dAudioToggle.isOn;
                    break;
                }
            case "screenvib":
                {
                    DataManager.instance._data.isScreenVibration = _screenVibrationToggle.isOn;
                    break;
                }
            case "damage":
                {
                    DataManager.instance._data.displayDamage = _damageToggle.isOn;
                    break;
                }
        }
    }

    public void OnResolutionChange(int x)
    {
        if (!_resolutionLocked)
        {
            Screen.SetResolution(_resolutions[x].width, _resolutions[x].height, DataManager.instance._data.fullScreenMode, _resolutions[x].refreshRate);
            DataManager.instance._data.resolution = _resolutions[x];
        }
    }
}

public class NeedResourse
{
    public void ChangeResourseAmount()
    {
        #region 무기
        switch (DataManager.instance._data.skillLevel)
        {
            case 0:
                {
                    DataManager.instance.NeedWeaponResourseChange(10, 10, 14, 0);
                    break;
                }
            case 1:
                {
                    DataManager.instance.NeedWeaponResourseChange(12, 12, 16, 0);
                    break;
                }
            case 2:
                {
                    DataManager.instance.NeedWeaponResourseChange(14, 14, 18, 1);
                    break;
                }
            case 3:
                {
                    DataManager.instance.NeedWeaponResourseChange(16, 16, 19, 0);
                    break;
                }
            case 4:
                {
                    DataManager.instance.NeedWeaponResourseChange(17, 17, 21, 0);
                    break;
                }
            case 5:
                {
                    DataManager.instance.NeedWeaponResourseChange(18, 18, 25, 1);
                    break;
                }
            case 6:
                {
                    DataManager.instance.NeedWeaponResourseChange(20, 20, 27, 0);
                    break;
                }
            case 7:
                {
                    DataManager.instance.NeedWeaponResourseChange(22, 22, 30, 1);
                    break;
                }
            case 8:
                {
                    DataManager.instance.NeedWeaponResourseChange(27, 27, 35, 2);
                    break;
                }
            case 9:
                {
                    DataManager.instance.NeedWeaponResourseChange(29, 29, 38, 1);
                    break;
                }
            case 10:
                {
                    DataManager.instance.NeedWeaponResourseChange(32, 32, 42, 1);
                    break;
                }
            case 11:
                {
                    DataManager.instance.NeedWeaponResourseChange(35, 35, 49, 3);
                    break;
                }
            case 12:
                {
                    DataManager.instance.NeedWeaponResourseChange(38, 38, 53, 2);
                    break;
                }
            case 13:
                {
                    DataManager.instance.NeedWeaponResourseChange(42, 42, 59, 2);
                    break;
                }
            case 14:
                {
                    DataManager.instance.NeedWeaponResourseChange(48, 48, 71, 2);
                    break;
                }
            default:
                {
                    DataManager.instance.NeedWeaponResourseChange(999999, 999999, 999999, 999999);
                    break;
                }
        }
        #endregion

        #region 집
        switch (DataManager.instance._data.buildLevel)
        {
            case 0:
                {
                    DataManager.instance.NeedHouseResourseChange(20, 20, 13, 0);
                    break;
                }
            case 1:
                {
                    DataManager.instance.NeedHouseResourseChange(26, 26, 21, 1);
                    break;
                }
            case 2:
                {
                    DataManager.instance.NeedHouseResourseChange(32, 32, 24, 0);
                    break;
                }
            case 3:
                {
                    DataManager.instance.NeedHouseResourseChange(39, 39, 25, 0);
                    break;
                }
            case 4:
                {
                    DataManager.instance.NeedHouseResourseChange(47, 47, 31, 1);
                    break;
                }
            case 5:
                {
                    DataManager.instance.NeedHouseResourseChange(58, 58, 35, 1);
                    break;
                }
            case 6:
                {
                    DataManager.instance.NeedHouseResourseChange(71, 71, 42, 1);
                    break;
                }
            case 7:
                {
                    DataManager.instance.NeedHouseResourseChange(84, 84, 49, 1);
                    break;
                }
            case 8:
                {
                    DataManager.instance.NeedHouseResourseChange(93, 93, 54, 2);
                    break;
                }
            case 9:
                {
                    DataManager.instance.NeedHouseResourseChange(106, 106, 58, 3);
                    break;
                }
            default:
                {
                    DataManager.instance.NeedHouseResourseChange(999999, 999999, 999999, 999999);
                    break;
                }
        }
        #endregion
    }
}