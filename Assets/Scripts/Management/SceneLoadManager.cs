using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

public class SceneLoadManager : MonoBehaviour
{
    public Slider _loadingBar;
    public GameObject _sceneChangeFade;
    public TextMeshProUGUI _loadingText, _tipText;
    private float _tipTimer = 5f;
    private LocalizedStringTable _table = new LocalizedStringTable { TableReference = "Tips" };
    bool _isTextFading = false;

    // Start is called before the first frame update
    void Start()
    {
        NextTip();
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        _tipTimer -= Time.deltaTime;
        if (_tipTimer <= 0f)
        {
            if (!_isTextFading)
            {
                StartCoroutine(ShowTip());
            }
        }
    }

    public void NextTip()
    {
        _tipTimer = 5f;
        if (!_isTextFading)
        {
            StartCoroutine(ShowTip());
        }
    }

    public IEnumerator ShowTip()
    {
        _isTextFading = true;
        for (int i = 255; i >= 0; i--)
        {
            _tipText.color = new Color32(255, 255, 255, (byte)i);
            yield return new WaitForSeconds(0.001f);
        }
        var table = _table.GetTableAsync();
        _tipTimer = 5f;
        int randomIndex = Random.Range(0, table.Result.SharedData.Entries.Count);
        var randomEntry = table.Result.SharedData.Entries[randomIndex];
        var entry = table.Result.GetEntry(randomEntry.Id);
        _tipText.text = entry.LocalizedValue;
        for (int i = 0; i <= 255; i++)
        {
            _tipText.color = new Color32(255, 255, 255, (byte)i);
            yield return new WaitForSeconds(0.001f);
        }
        _isTextFading = false;
    }

    IEnumerator Fade()
    {
        for (int i = 255; i >= 0; i--)
        {
            _sceneChangeFade.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)i);
            yield return new WaitForSeconds(0.001f);
        }
        _sceneChangeFade.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(LoadScene("Fighting"));
    }

    IEnumerator LoadScene(string sceneName)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Loading":
                {
                    sceneName = "Fighting";
                    break;
                }
            case "Loading_ToHome":
                {
                    sceneName = "Home";
                    break;
                }
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;

        while (!asyncLoad.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if (asyncLoad.progress < 0.9f)
            {
                _loadingText.text = Mathf.FloorToInt(_loadingBar.value * 100f) + "%";
                _loadingBar.value = Mathf.Lerp(asyncLoad.progress, 1f, timer);

                if (_loadingBar.value >= asyncLoad.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                _loadingBar.value = Mathf.Lerp(_loadingBar.value, 1f, timer);
                _loadingText.text = Mathf.FloorToInt(_loadingBar.value * 100f) + "%";

                if (_loadingBar.value == 1f)
                {
                    _loadingText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "pressakey").Result;
                    if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
                    {
                        _sceneChangeFade.SetActive(true);
                        for (int i = 0; i <= 255; i++)
                        {
                            _sceneChangeFade.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)i);
                            yield return new WaitForSeconds(0.005f);
                        }
                        asyncLoad.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }
}
