using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kraken : MonoBehaviour
{
    [SerializeField]
    private GameObject smallKrakenPrefab;
    [SerializeField]
    private float smallUpTime;

    [Space(10)]

    [SerializeField]
    private GameObject earthquakePrefab;
    [SerializeField]
    private float earthquakeTime;

    private Slider hpBar;
    private GameObject hpBarObj;
    private float hp;
    [SerializeField]
    private float maxHp;

    private void Update()
    {
        if (hpBar is not null)
        {
            hpBar.value = (float)hp / maxHp;
        }
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        Vector2 randCirclePos = Random.insideUnitCircle;
    }

    private void Start()
    {
        InGameUI.instance._isBoss = true;
        hpBarObj = GameObject.Find("Canvas").transform.GetChild(4).GetChild(1).gameObject;
        InGameUI.instance._timerText.gameObject.SetActive(false);
        hpBarObj.SetActive(true);
        hpBar = hpBarObj.transform.GetChild(1).GetComponent<Slider>();

        hp = maxHp;
        StartCoroutine(SmallUpKrakenActive());
        StartCoroutine(EarthquakeActive());
    }

    void OnDestroy()
    {
        hpBarObj.SetActive(false);
        InGameUI.instance._timerText.gameObject.SetActive(true);
        InGameUI.instance._isBoss = false;
    }

    IEnumerator EarthquakeActive()
    {
        float ranNum = Random.Range(8f, 12f);
        yield return new WaitForSeconds(ranNum);
        StartCoroutine(Earthquake());
        StartCoroutine(EarthquakeActive());
    }

    IEnumerator Earthquake()
    {
        GameObject earthquake = Instantiate(earthquakePrefab);

        bool vertical = (Random.value > 0.5f);
        float ranNum = Random.Range(0, 30.0f);

        if (vertical)
        {
            earthquake.transform.position = new Vector2(-ranNum, 0);
            earthquake.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            earthquake.transform.position = new Vector2(0, ranNum);
            earthquake.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        GameObject earthquakeEffect = earthquake.transform.GetChild(0).gameObject;
        earthquakeEffect.SetActive(false);
        GameObject earthquakeArea = earthquake.transform.GetChild(1).gameObject;
        earthquakeArea.SetActive(true);

        float alphaValue = 180;

        //처음 시작 180
        while (alphaValue > 0)
        {
            yield return new WaitForSeconds(earthquakeTime / 180);

            earthquakeArea.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, (byte)alphaValue--);
        }

        earthquakeEffect.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        Destroy(earthquake);
    }

    IEnumerator SmallUpKrakenActive()
    {
        float ranNum = Random.Range(3.0f, 5.0f);
        yield return new WaitForSeconds(ranNum);
        StartCoroutine(UpKraken());
        StartCoroutine(SmallUpKrakenActive());
    }

    IEnumerator UpKraken()
    {
        GameObject krakenUpLeg = Instantiate(smallKrakenPrefab);
        krakenUpLeg.transform.position = PlayerController.instance.transform.position;

        GameObject krakenLeg = krakenUpLeg.transform.GetChild(0).gameObject;
        krakenLeg.SetActive(false);
        GameObject krakenArea = krakenUpLeg.transform.GetChild(1).gameObject;
        krakenArea.SetActive(true);

        float alphaValue = 180;

        //처음 시작 180
        while (alphaValue > 0)
        {
            yield return new WaitForSeconds(smallUpTime / 180);

            krakenArea.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, (byte)alphaValue--);
        }

        krakenLeg.SetActive(true);

        yield return new WaitForSeconds(6.0f);
        Destroy(krakenUpLeg);
    }

    public void Hit(float damage)
    {
        hp -= damage;
    }
}

