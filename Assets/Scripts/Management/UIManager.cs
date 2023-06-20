using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region 팝업창
    [SerializeField]//업그레이드창
    private GameObject upgradeWindow;
    [SerializeField]//무기 업그레이드창
    private GameObject weaponWindow;
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

    //재료 개수 확인 텍스트 모음
    private TextMeshProUGUI[] resourceTexts = new TextMeshProUGUI[6];
    //무기 필요 재료 확인 텍스트 모음
    private TextMeshProUGUI[] needWeaponTexts = new TextMeshProUGUI[4];
    //집 필요 재료 확인 텍스트 모음
    private TextMeshProUGUI[] needBuildTexts = new TextMeshProUGUI[4];
    #endregion

    #region 기타
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

    int _storyNum;

    // 책 텍스트
    public TextMeshProUGUI _title, _desc;

    //음식 제작 게이지
    private Slider foodSlider;

    //음식 제작 시간
    private float cookingTime = 30;
    //현재 제작 시간
    private float currentCookingTime = 0;
    //제작 상태
    private bool isCooking = false;

    private bool _lookingStory = false;
    NeedResourse _needResourse = new NeedResourse();
    #endregion

    private void Start()
    {
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
    }

    private void Update()
    {
        ShowResource();
        if (isCooking)
        {
            currentCookingTime += Time.deltaTime;

            foodSlider.value = currentCookingTime / cookingTime;

            foodText.text = "음식 제작중... 남은 시간 : " + (int)(cookingTime - currentCookingTime);

            if (currentCookingTime >= cookingTime)
            {
                StartCoroutine(TextClear(foodText, "제작이 완료되었습니다"));

                GameManager.instance.foodCount += 2;
                foodSlider.value = 0;
                isCooking = false;
                currentCookingTime = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            _storyNum = Random.Range(0, 5);
        }
    }

    #region 버튼 함수

    //업그레이드 창 팝업
    public void OnClickUpgradeButton()
    {
        WindowPopUp(upgradeWindow);
    }

    //무기 업그레이드 창 팝업
    public void OnClickWeaponButton()
    {
        ShowNeedWeaponResourse();

        PopUpWidowChange(weaponWindow, upgradeWindow);
    }

    //무기 강화 버튼 클릭
    public void OnClickweaponUpgradeButton()
    {
        if (DataManager.instance._data.resources["iron"] >= DataManager.instance._data.weaponUpgrade["iron"]
        && DataManager.instance._data.resources["concrete"] >= DataManager.instance._data.weaponUpgrade["concrete"]
        && DataManager.instance._data.resources["bolt"] >= DataManager.instance._data.weaponUpgrade["bolt"]
        && DataManager.instance._data.resources["core"] >= DataManager.instance._data.weaponUpgrade["core"])
        {
            PopUpWidowChange(upgradeText.gameObject, needWeaponResource);

            DataManager.instance._data.resources["iron"] -= DataManager.instance._data.weaponUpgrade["iron"];
            DataManager.instance._data.resources["concrete"] -= DataManager.instance._data.weaponUpgrade["concrete"];
            DataManager.instance._data.resources["bolt"] -= DataManager.instance._data.weaponUpgrade["bolt"];
            DataManager.instance._data.resources["core"] -= DataManager.instance._data.weaponUpgrade["core"];

            StartCoroutine(Typing(upgradeText, "강 화 중 . . .", 0.5f, upgradeText.gameObject, needWeaponResource));

            DataManager.instance._data.skillLevel++;

            ShowNeedWeaponResourse();

            DataManager.instance.Save();
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
        && DataManager.instance._data.resources["core"] >= DataManager.instance._data.HouseUpgrade["core"])
        {
            PopUpWidowChange(buildText.gameObject, needHouseResource);

            DataManager.instance._data.resources["iron"] -= DataManager.instance._data.HouseUpgrade["iron"];
            DataManager.instance._data.resources["concrete"] -= DataManager.instance._data.HouseUpgrade["concrete"];
            DataManager.instance._data.resources["bolt"] -= DataManager.instance._data.HouseUpgrade["bolt"];
            DataManager.instance._data.resources["core"] -= DataManager.instance._data.HouseUpgrade["core"];

            StartCoroutine(Typing(buildText, "건 설 중 . . .", 0.5f, buildText.gameObject, needHouseResource));

            DataManager.instance._data.buildLevel++;

            ShowNeedBuildResourse();

            DataManager.instance.Save();
        }
        else
        {
            StartCoroutine(TextClear(buildSubText, "재료가 부족합니다"));
        }
    }

    //음식 제작(재료 아이템 제거하는거 있음)
    public void OnClickFoodButton()
    {
        if (DataManager.instance._data.resources["ingredient"] >= 5 && !isCooking)
        {
            GameManager.instance.ResourceReduction("ingredient", 5);
            isCooking = true;
        }
        else if (!isCooking)
        {
            StartCoroutine(TextClear(foodSubText, "재료가 부족합니다..."));
        }
        else
        {
            StartCoroutine(TextClear(foodSubText, "제작 중 추가 제작을 할 수 없습니다"));
        }

        Debug.Log("식량제작버튼 클릭");
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
    }

    //설정 팝업
    public void OnClickOption()
    {
        WindowPopUp(Option);
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
        for (int i = 0; i <= 255; i++)
        {
            _sceneChangeFade.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)i);
            yield return new WaitForSeconds(0.00005f);
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
    }

    //타이핑 효과 코루틴
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

    //텍스트 1초 띄우기
    IEnumerator TextClear(TextMeshProUGUI text, string messege)
    {
        text.text = messege;
        yield return new WaitForSeconds(1f);
        text.text = " ";
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
                    DataManager.instance.NeedWeaponResourseChange(14, 14, 20, 0);
                    break;
                }
            case 2:
                {
                    DataManager.instance.NeedWeaponResourseChange(24, 24, 31, 1);
                    break;
                }
            case 3:
                {
                    DataManager.instance.NeedWeaponResourseChange(21, 21, 28, 0);
                    break;
                }
            case 4:
                {
                    DataManager.instance.NeedWeaponResourseChange(29, 29, 36, 0);
                    break;
                }
            case 5:
                {
                    DataManager.instance.NeedWeaponResourseChange(39, 39, 45, 1);
                    break;
                }
            case 6:
                {
                    DataManager.instance.NeedWeaponResourseChange(45, 45, 51, 0);
                    break;
                }
            case 7:
                {
                    DataManager.instance.NeedWeaponResourseChange(53, 53, 66, 1);
                    break;
                }
            case 8:
                {
                    DataManager.instance.NeedWeaponResourseChange(68, 68, 81, 2);
                    break;
                }
            case 9:
                {
                    DataManager.instance.NeedWeaponResourseChange(84, 84, 103, 1);
                    break;
                }
            case 10:
                {
                    DataManager.instance.NeedWeaponResourseChange(98, 98, 124, 1);
                    break;
                }
            case 11:
                {
                    DataManager.instance.NeedWeaponResourseChange(39, 39, 45, 1);
                    break;
                }
        }
        #endregion

        #region 집
        switch (DataManager.instance._data.buildLevel)
        {
            case 0:
                {
                    DataManager.instance.NeedHouseResourseChange(35, 35, 25, 0);
                    break;
                }
            case 1:
                {
                    DataManager.instance.NeedHouseResourseChange(42, 42, 62, 1);
                    break;
                }
            case 2:
                {
                    DataManager.instance.NeedHouseResourseChange(56, 56, 41, 0);
                    break;
                }
            case 3:
                {
                    DataManager.instance.NeedHouseResourseChange(72, 72, 58, 0);
                    break;
                }
            case 4:
                {
                    DataManager.instance.NeedHouseResourseChange(95, 95, 102, 1);
                    break;
                }
            case 5:
                {
                    DataManager.instance.NeedHouseResourseChange(112, 112, 78, 1);
                    break;
                }
            case 6:
                {
                    DataManager.instance.NeedHouseResourseChange(134, 134, 110, 1);
                    break;
                }
            case 7:
                {
                    DataManager.instance.NeedHouseResourseChange(158, 158, 164, 2);
                    break;
                }
            case 8:
                {
                    DataManager.instance.NeedHouseResourseChange(174, 174, 154, 2);
                    break;
                }
            case 9:
                {
                    DataManager.instance.NeedHouseResourseChange(224, 224, 243, 3);
                    break;
                }
        }
        #endregion
    }
}