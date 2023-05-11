using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region �˾�â
    [SerializeField]//���׷��̵�â
    private GameObject upgradeWindow;
    [SerializeField]//���� ���׷��̵�â
    private GameObject weaponWindow;
    [SerializeField]//�� ���׷��̵�â
    private GameObject houseWindow;
    [SerializeField]//���丮 â
    private GameObject bookWindow;
    [SerializeField]//����â
    private GameObject Option;
    #endregion

    #region �ؽ�Ʈ
    [SerializeField]//��ȭ��...Ÿ���� �ؽ�Ʈ
    private TextMeshProUGUI upgradeText;
    [SerializeField]//������... Ÿ���� �ؽ�Ʈ
    private TextMeshProUGUI buildText;
    [SerializeField]//���� ���� ����Ȯ�� �ؽ�Ʈ
    private TextMeshProUGUI foodText;
    [SerializeField]//���� �Ϸ�� ���� ���� Ȯ�� �ؽ�Ʈ
    private TextMeshProUGUI foodCountText;
    #endregion

    #region ��Ÿ
    [SerializeField]//���� ��ȭ�� �ʿ��� ��� Ȯ�� �ؽ�Ʈ ����(�� ������Ʈ)
    private GameObject needWeaponResource;
    [SerializeField]//�� ��ȭ�� �ʿ��� ��� Ȯ�� �ؽ�Ʈ ����(�� ������Ʈ)
    private GameObject needHouseResource;
    [SerializeField]//��� ���� Ȯ�� �ؽ�Ʈ�� �θ� ������Ʈ(�� ������Ʈ)
    private GameObject ResourceTextsParent;

    //���� ���� ������
    private Slider foodSlider;

    //��� ���� Ȯ�� �ؽ�Ʈ ����
    private TextMeshProUGUI[] resourceTexts = new TextMeshProUGUI[6];

    //���� ���� �ð�
    private float cookingTime = 30;
    //���� ���� �ð�
    private float currentCookingTime = 0;
    //���� ����
    private bool isCooking = false;
    #endregion

    private void Start()
    {
        for(int i = 0; i < 6; i++)
        {
            resourceTexts[i] = ResourceTextsParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            Debug.Log(resourceTexts[i].name);
        }

        foodSlider = GameObject.Find("Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        ShowResource();
        if(isCooking)
        {
            currentCookingTime += Time.deltaTime;

            foodSlider.value = currentCookingTime / cookingTime;

            foodText.text = "���� ������... ���� �ð� : " + (int)(cookingTime - currentCookingTime);

            if (currentCookingTime >= cookingTime)
            {
                StartCoroutine(TextClear(foodText, "������ �Ϸ�Ǿ����ϴ�"));

                GameManager.instance.foodCount++;
                foodSlider.value = 0;
                isCooking = false;
                currentCookingTime = 0;
            }
        }
    }

    #region ��ư �Լ�

    //���׷��̵� â �˾�
    public void OnClickUpgradeButton()
    {
        WindowPopUp(upgradeWindow);
    }

    //���� ���׷��̵� â �˾�
    public void OnClickWeaponButton()
    {
        PopUpWidowChange(weaponWindow, upgradeWindow);
    }

    //���� ��ȭ ��ư Ŭ��(���� ������ �����ϴ°� ����)
    public void OnClickweaponUpgradeButton()
    {
        PopUpWidowChange(upgradeText.gameObject, needWeaponResource);
        StartCoroutine(Typing(upgradeText, "�� ȭ �� . . .", 0.5f, upgradeText.gameObject, needWeaponResource));
    }

    //�� ���׷��̵� â �˾�
    public void OnClickHouseButton()
    {
        PopUpWidowChange(houseWindow, upgradeWindow);
    }

    //�� ��ȭ ��ư Ŭ��(���� ���� ���� ��� ����)
    public void OnClickHouseUpgradeButton()
    {
        PopUpWidowChange(buildText.gameObject, needHouseResource);
        StartCoroutine(Typing(buildText, "�� �� �� . . .", 0.5f, buildText.gameObject, needHouseResource));
    }

    //���� ����(��� ������ �����ϴ°� ����)
    public void OnClickFoodButton()
    {
        if(GameManager.instance.resources["ingredient"] >= 5 && !isCooking)
        {
            GameManager.instance.ResourceReduction("ingredient", 5);
            isCooking = true;
        }
        else
        {
            StartCoroutine(TextClear(foodText, "��ᰡ �����մϴ�..."));
        }
        
        Debug.Log("�ķ����۹�ư Ŭ��");
    }

    //�ķ� ȸ��
    public void OnClickFoodRecallButton()
    {
        GameManager.instance.IncreseResource("food", GameManager.instance.foodCount);
        Debug.Log("�ķ�ȸ����ư Ŭ��");
        GameManager.instance.foodCount = 0;
    }

    //���丮 �˾�
    public void OnClickTableButton()
    {
        WindowPopUp(bookWindow);
    }

    //���� �˾�
    public void OnClickOption()
    {
        WindowPopUp(Option);
    }

    //���׷��̵� �˾� ������
    public void OnClickUpgradeBackground()
    {
        WindowDisappear(upgradeWindow);
    }

    //���丮 �˾� ������
    public void OnClickTableBackground()
    {
        WindowDisappear(bookWindow);
    }

    //���� �˾� ������
    public void OnClickOptionBackground()
    {
        WindowDisappear(Option);
    }
    #endregion

    //�˾� �ϳ� Ű�� �ϳ� ����
    void PopUpWidowChange(GameObject popUp, GameObject window)
    {
        popUp.SetActive(!popUp.activeSelf);
        window.SetActive(!window.activeSelf);
    }

    //�˾� ����
    void WindowDisappear(GameObject popUp)
    {
        popUp.SetActive(false);
    }

    //�˾� ��Ű��
    void WindowPopUp(GameObject popUp)
    {
        popUp.SetActive(true);
    }

    //�� �ѱ��
    public void HomeToFighting()
    {
        SceneManager.LoadScene("Fighting");
    }

    //�ķ� ���� ǥ��
    public void ShowResource()
    {
        resourceTexts[0].text = GameManager.instance.resources["iron"].ToString();
        resourceTexts[1].text = GameManager.instance.resources["concrete"].ToString();
        resourceTexts[2].text = GameManager.instance.resources["bolt"].ToString();
        resourceTexts[3].text = GameManager.instance.resources["core"].ToString();
        resourceTexts[4].text = GameManager.instance.resources["food"].ToString();
        resourceTexts[5].text = GameManager.instance.resources["ingredient"].ToString();
        foodCountText.text = GameManager.instance.foodCount.ToString();
    }

    //Ÿ���� ȿ�� �ڷ�ƾ
    IEnumerator Typing(TextMeshProUGUI typingText, string message, float speed, GameObject popUp, GameObject window)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
        typingText.text = "�Ϸ�";
        yield return new WaitForSeconds(1);
        PopUpWidowChange(popUp, window);
    }

    //�ؽ�Ʈ 1�� ����
    IEnumerator TextClear(TextMeshProUGUI text, string messege)
    {
        text.text = messege;
        yield return new WaitForSeconds(1f);
        text.text = " ";
    }
}
