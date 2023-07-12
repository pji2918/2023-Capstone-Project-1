using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    UIManager ui;
    [SerializeField]
    GameObject tutorial;
    [SerializeField]
    GameObject tutorial2;
    [SerializeField]
    GameObject step1;
    [SerializeField]
    TextMeshProUGUI step1Text;
    [SerializeField]
    GameObject step2;
    [SerializeField]
    GameObject step4;
    [SerializeField]
    GameObject step5;
    [SerializeField]
    GameObject step6;
    [SerializeField]
    GameObject step7;
    [SerializeField]
    GameObject step8;
    [SerializeField]
    GameObject step9;
    [SerializeField]
    GameObject step10;
    [SerializeField]
    GameObject step11;
    [SerializeField]
    GameObject step12;
    [SerializeField]
    GameObject step13;
    [SerializeField]
    GameObject step14;
    [SerializeField]
    GameObject step15;
    [SerializeField]
    GameObject step16;


    int tutorialStep;
    int tutorialStep2;

    void Awake()
    {
        tutorial.SetActive(false);
        tutorial2.SetActive(false);
    }


    void Start()
    {
        if (DataManager.instance._data.day == 1)
        {
            tutorialStep = 0;
            tutorial.SetActive(true);
            StartCoroutine(TutorialStart());
        }
        else if (DataManager.instance._data.day == 2)
        {
            tutorialStep2 = 0;
            tutorial2.SetActive(true);
            StartCoroutine(TutorialStart2());
        }
    }

    void Update()
    {
        Debug.Log("step1: " + tutorialStep + " / step2: " + tutorialStep2);
    }

    IEnumerator TutorialStart()
    {
        Step1();
        yield return new WaitWhile(() => tutorialStep < 1);

        Step2();
        yield return new WaitWhile(() => tutorialStep < 2);

        Step3();
        yield return new WaitWhile(() => tutorialStep < 3);

        Step4();
        yield return new WaitWhile(() => tutorialStep < 4);
    }

    void SetActiveChange(GameObject window)
    {
        window.SetActive(!window.activeSelf);
    }

    public void StepUp()
    {
        tutorialStep++;
    }

    public void StepUp2()
    {
        tutorialStep2++;
    }

    void Step1()
    {
        SetActiveChange(step1);
        step1Text.text = "저희 게임이 처음이시군요!!\n게임을 시작하기에 앞서 튜토리얼을 진행해 주세요!";
    }

    void Step2()
    {
        step1Text.text = "저희 게임은 하루 단위로 진행되며 다이어리를 통해 전투로 돌입하고 전투가 끝나면 하루가 진행됩니다\n바로 한번 진행해 볼까요?";
    }

    void Step3()
    {
        SetActiveChange(step1);
        SetActiveChange(step2);
    }

    void Step4()
    {
        SetActiveChange(step2);
        SetActiveChange(step4);
    }

    IEnumerator TutorialStart2()
    {
        TStep1();
        yield return new WaitWhile(() => tutorialStep2 < 1);

        TStep2();
        yield return new WaitWhile(() => tutorialStep2 < 2);

        TStep3();
        yield return new WaitWhile(() => tutorialStep2 < 3);

        TStep4();
        yield return new WaitWhile(() => tutorialStep2 < 4);

        TStep5();
        yield return new WaitWhile(() => tutorialStep2 < 5);

        TStep6();
        yield return new WaitWhile(() => tutorialStep2 < 6);

        TStep7();
        yield return new WaitWhile(() => tutorialStep2 < 7);

        TStep8();
        yield return new WaitWhile(() => tutorialStep2 < 8);

        TStep9();
        yield return new WaitWhile(() => tutorialStep2 < 9);

        TStep10();
        yield return new WaitWhile(() => tutorialStep2 < 10);

        TStep11();
        yield return new WaitWhile(() => tutorialStep2 < 11);

        TStep12();
        yield return new WaitWhile(() => tutorialStep2 < 12);

        TStep13();
        yield return new WaitWhile(() => tutorialStep2 < 13);
    }

    void TStep1()
    {
        SetActiveChange(step5);
    }

    void TStep2()
    {
        SetActiveChange(step5);
        SetActiveChange(step6);
    }

    void TStep3()
    {
        SetActiveChange(step6);
        SetActiveChange(step7);
    }

    void TStep4()
    {
        SetActiveChange(step7);
        SetActiveChange(step8);
    }

    void TStep5()
    {
        SetActiveChange(step8);
        SetActiveChange(step9);
    }

    void TStep6()
    {
        SetActiveChange(step9);
        SetActiveChange(step10);
    }

    void TStep7()
    {
        SetActiveChange(step10);
        SetActiveChange(step11);
    }

    void TStep8()
    {
        SetActiveChange(step11);
        SetActiveChange(step12);
    }


    void TStep9()
    {
        ui.weaponWindow.SetActive(false);
        ui.upgradeWindow.SetActive(false);
        SetActiveChange(step12);
        SetActiveChange(step13);
    }

    void TStep10()
    {
        SetActiveChange(step13);
        SetActiveChange(step14);
    }

    void TStep11()
    {
        ui._statWindow.SetActive(false);
        SetActiveChange(step14);
        SetActiveChange(step15);
    }

    void TStep12()
    {
        SetActiveChange(step15);
        SetActiveChange(step16);
    }

    void TStep13()
    {
        SetActiveChange(step16);
    }
}