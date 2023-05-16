using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
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
    [SerializeField]//음식 제작 상태확인 텍스트
    private TextMeshProUGUI foodText;
    [SerializeField]//제작 완료된 음식 개수 확인 텍스트
    private TextMeshProUGUI foodCountText;
    #endregion

    #region 기타
    [SerializeField]//무기 강화시 필요한 재료 확인 텍스트 묶음(빈 오브젝트)
    private GameObject needWeaponResource;
    [SerializeField]//집 강화시 필요한 재료 확인 텍스트 묶음(빈 오브젝트)
    private GameObject needHouseResource;
    [SerializeField]//재료 개수 확인 텍스트의 부모 오브잭트(빈 오브잭트)
    private GameObject ResourceTextsParent;

    // 책 텍스트
    public TextMeshProUGUI _title, _desc;

    //음식 제작 게이지
    private Slider foodSlider;

    //재료 개수 확인 텍스트 모음
    private TextMeshProUGUI[] resourceTexts = new TextMeshProUGUI[6];

    //음식 제작 시간
    private float cookingTime = 30;
    //현재 제작 시간
    private float currentCookingTime = 0;
    //제작 상태
    private bool isCooking = false;
    #endregion

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            resourceTexts[i] = ResourceTextsParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            Debug.Log(resourceTexts[i].name);
        }

        foodSlider = GameObject.Find("Slider").GetComponent<Slider>();
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

                GameManager.instance.foodCount++;
                foodSlider.value = 0;
                isCooking = false;
                currentCookingTime = 0;
            }
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
        PopUpWidowChange(weaponWindow, upgradeWindow);
    }

    //무기 강화 버튼 클릭(아직 아이템 제거하는거 없음)
    public void OnClickweaponUpgradeButton()
    {
        PopUpWidowChange(upgradeText.gameObject, needWeaponResource);
        StartCoroutine(Typing(upgradeText, "강 화 중 . . .", 0.5f, upgradeText.gameObject, needWeaponResource));
        DataManager.instance._data.skillLevel++;
        DataManager.instance.Save();
    }

    //집 업그레이드 창 팝업
    public void OnClickHouseButton()
    {
        PopUpWidowChange(houseWindow, upgradeWindow);
    }

    //집 강화 버튼 클릭(위와 같이 아직 기능 부족)
    public void OnClickHouseUpgradeButton()
    {
        PopUpWidowChange(buildText.gameObject, needHouseResource);
        StartCoroutine(Typing(buildText, "건 설 중 . . .", 0.5f, buildText.gameObject, needHouseResource));
    }

    //음식 제작(재료 아이템 제거하는거 있음)
    public void OnClickFoodButton()
    {
        if (DataManager.instance._data.resources["ingredient"] >= 5 && !isCooking)
        {
            GameManager.instance.ResourceReduction("ingredient", 5);
            isCooking = true;
        }
        else
        {
            StartCoroutine(TextClear(foodText, "재료가 부족합니다..."));
        }

        Debug.Log("식량제작버튼 클릭");
    }

    //식량 회수
    public void OnClickFoodRecallButton()
    {
        GameManager.instance.IncreseResource("food", GameManager.instance.foodCount);
        Debug.Log("식량회수버튼 클릭");
        GameManager.instance.foodCount = 0;
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
        WindowDisappear(bookWindow);
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
        SceneManager.LoadScene("Fighting");
    }

    //식량 개수 표시
    public void ShowResource()
    {
        resourceTexts[0].text = DataManager.instance._data.resources["iron"].ToString();
        resourceTexts[1].text = DataManager.instance._data.resources["concrete"].ToString();
        resourceTexts[2].text = DataManager.instance._data.resources["bolt"].ToString();
        resourceTexts[3].text = DataManager.instance._data.resources["core"].ToString();
        resourceTexts[4].text = DataManager.instance._data.resources["food"].ToString();
        resourceTexts[5].text = DataManager.instance._data.resources["ingredient"].ToString();
        foodCountText.text = GameManager.instance.foodCount.ToString();
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
            _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03Text");
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
        else if (DataManager.instance._data.day == 15)
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
        else if (DataManager.instance._data.day == 21)
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
        else if (DataManager.instance._data.day == 26)
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
        else if (false) // 이건 모든 게 완성되었을 때 추가됩니다.
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
                switch (Random.Range(0, 5))
                {
                    case 0:
                        {
                            if (DataManager.instance._data.day >= 4)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04");
                                ShowStoryButton(ButtonType.Choice, 1);
                            }
                            else
                            {
                                continue;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (DataManager.instance._data.day < 13)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryUntilDay13");
                                ShowStoryButton(ButtonType.Choice, 2);
                            }
                            else
                            {
                                continue;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (DataManager.instance._data.day >= 6)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay06");
                                ShowStoryButton(ButtonType.Choice, 3);
                            }
                            else
                            {
                                continue;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (DataManager.instance._data.day > 13)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay13");
                                ShowStoryButton(ButtonType.Choice, 4);
                            }
                            else
                            {
                                continue;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (DataManager.instance._data.day > 18)
                            {
                                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18");
                                ShowStoryButton(ButtonType.Choice, 5);
                            }
                            else
                            {
                                continue;
                            }
                            break;
                        }
                }
            }
            if (loop > 5)
            {
                _desc.text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryDefault");
                ShowStoryButton(ButtonType.Normal);
            }
        }
    }

    public enum ButtonType
    {
        Normal,
        Choice
    }

    public void ShowStoryButton(ButtonType type, int eventNum = 0)
    {
        switch (type)
        {
            case ButtonType.Normal:
                {
                    _normal.SetActive(true);
                    break;
                }
            case ButtonType.Choice:
                {
                    switch (eventNum)
                    {
                        case 1:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04Choice1");
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay04Choice2");
                                break;
                            }
                        case 2:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryUntilDay13TextChoice1");
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryUntilDay13TextChoice2");
                                break;
                            }
                        case 3:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay6TextChoice1");
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay6TextChoice2");
                                break;
                            }
                        case 4:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay13TextChoice1");
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay13TextChoice2");
                                break;
                            }
                        case 5:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18TextChoice1");
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "randomStoryAfterDay18TextChoice2");
                                break;
                            }
                        case 6:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03TextChoice1");
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay03TextChoice2");
                                break;
                            }
                        case 7:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay13TextChoice1");
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay13TextChoice2");
                                break;
                            }
                        case 8:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay18TextChoice1");
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay18TextChoice2");
                                break;
                            }
                        case 9:
                            {
                                _choiceButtons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay28TextChoice1");
                                _choiceButtons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("Story", "storyDay28TextChoice2");
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

    }

    public void ChoiceTwo()
    {

    }
}
