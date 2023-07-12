using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingTutorial : MonoBehaviour
{
    int tutorialStep;
    bool isTutorial;

    [SerializeField]
    GameObject tutorial;
    [SerializeField]
    GameObject step1;
    [SerializeField]
    GameObject step2;
    [SerializeField]
    GameObject step3;
    [SerializeField]
    GameObject step4;
    [SerializeField]
    GameObject step5;


    void Awake()
    {
        isTutorial = (DataManager.instance._data.day == 1);

        if (isTutorial)
        {
            tutorial.SetActive(true);
            tutorialStep = 0;
            StartCoroutine(Tutorial());
        }
    }

    void Start()
    {
        isTutorial = (DataManager.instance._data.day == 1);
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(0.75f);

        Step1();
        yield return new WaitWhile(() => tutorialStep < 1);

        Step2();
        yield return new WaitWhile(() => tutorialStep < 2);

        Step3();
        yield return new WaitWhile(() => tutorialStep < 3);

        Step4();
        yield return new WaitWhile(() => tutorialStep < 4);

        Step5();
        yield return new WaitWhile(() => tutorialStep < 5);

        Step6();
        yield return new WaitWhile(() => tutorialStep < 6);
    }

    public void AddStep()
    {
        tutorialStep++;
    }

    void SetActiveChange(GameObject window)
    {
        window.SetActive(!window.activeSelf);
    }

    void Step1()
    {
        Time.timeScale = 0;
        SetActiveChange(step1);
    }
    void Step2()
    {
        SetActiveChange(step1);
        SetActiveChange(step2);
    }
    void Step3()
    {
        SetActiveChange(step2);
        SetActiveChange(step3);
    }
    void Step4()
    {
        SetActiveChange(step3);
        SetActiveChange(step4);
    }
    void Step5()
    {
        SetActiveChange(step4);
        SetActiveChange(step5);
    }
    void Step6()
    {
        SetActiveChange(step5);
        Time.timeScale = 1;
    }
}
