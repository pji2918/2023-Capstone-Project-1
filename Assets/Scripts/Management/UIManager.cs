using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    //음식 제작 게이지
    private Slider foodSlider;

    //음식 제작 시간
    private float cookingTime = 30;
    //현재 제작 시간
    private float currentCookingTime = 0;
    //제작 상태
    private bool isCooking = false;

    NeedResourse _needResourse = new NeedResourse();
    #endregion

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            resourceTexts[i] = ResourceTextsParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            Debug.Log("재료 텍스트 : " + resourceTexts[i].name);

            if(i < 4)
            {
                needWeaponTexts[i] = needWeaponResourceTextsParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                needBuildTexts[i] = needHouseResourceTextsParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            
                Debug.Log("무기 강화 필요 재료 텍스트" +needWeaponTexts[i].name);
                Debug.Log("쉘터 강화 필요 재료 텍스트" +needBuildTexts[i].name);
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
        ShowNeedWeaponResourse();
        
        PopUpWidowChange(weaponWindow, upgradeWindow);
    }

    //무기 강화 버튼 클릭
    public void OnClickweaponUpgradeButton()
    {
        if(DataManager.instance._data.resources["iron"] >= DataManager.instance._data.weaponUpgrade["iron"] 
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
        if(DataManager.instance._data.resources["iron"] >= DataManager.instance._data.HouseUpgrade["iron"] 
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
        else if(!isCooking)
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
        if(GameManager.instance.foodCount != 0)
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
    private void ShowResource()
    {
        resourceTexts[0].text = DataManager.instance._data.resources["iron"].ToString();
        resourceTexts[1].text = DataManager.instance._data.resources["concrete"].ToString();
        resourceTexts[2].text = DataManager.instance._data.resources["bolt"].ToString();
        resourceTexts[3].text = DataManager.instance._data.resources["core"].ToString();
        resourceTexts[4].text = DataManager.instance._data.resources["food"].ToString();
        resourceTexts[5].text = DataManager.instance._data.resources["ingredient"].ToString();
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
}

public class NeedResourse
{
    public void ChangeResourseAmount()
    {
        #region 무기
        switch(DataManager.instance._data.skillLevel)
        {
            case 0:
                DataManager.instance.NeedWeaponResourseChange(1, 0, 0, 0);
            break;
            
            case 1:
                DataManager.instance.NeedWeaponResourseChange(1, 1, 0, 0);
            break;
            
            case 2:
                DataManager.instance.NeedWeaponResourseChange(1, 1, 1, 0);
            break;

            case 3:
                DataManager.instance.NeedWeaponResourseChange(1, 1, 1, 1);
            break;

            case 4:
                DataManager.instance.NeedWeaponResourseChange(1, 2, 3, 4);
            break;

            default:
                DataManager.instance.NeedWeaponResourseChange(5, 5, 5, 5);
            break;
        }
        #endregion

        #region 집
        switch(DataManager.instance._data.buildLevel)
        {
            case 0:
                DataManager.instance.NeedHouseResourseChange(1, 0, 0, 0);
            break;
            
            case 1:
                DataManager.instance.NeedHouseResourseChange(1, 1, 0, 0);
            break;
            
            case 2:
                DataManager.instance.NeedHouseResourseChange(1, 1, 1, 0);
            break;

            case 3:
                DataManager.instance.NeedHouseResourseChange(1, 1, 1, 1);
            break;

            case 4:
                DataManager.instance.NeedHouseResourseChange(1, 2, 3, 4);
            break;

            default:
                DataManager.instance.NeedHouseResourseChange(5, 5, 5, 5);
            break;
        }
        #endregion
    }
}