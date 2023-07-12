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
        float elapsedTime = 0f;
        float fadeTime = 1f;
        _isTextFading = true;
        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            _tipText.color = new Color32(255, 255, 255, (byte)(alpha * 255));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        var table = _table.GetTableAsync();
        _tipTimer = 5f;
        int randomIndex = Random.Range(0, table.Result.SharedData.Entries.Count);
        var randomEntry = table.Result.SharedData.Entries[randomIndex];
        var entry = table.Result.GetEntry(randomEntry.Id);
        try
        {
            _tipText.text = entry.LocalizedValue;
        }
        catch
        {
            _tipText.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI", "tiperror");
        }

        elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            _tipText.color = new Color32(255, 255, 255, (byte)(alpha * 255));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _isTextFading = false;
    }

    IEnumerator Fade()
    {
        _sceneChangeFade.SetActive(true);
        float elapsedTime = 0f;
        float fadeTime = 1f; // fade time in seconds
        Color32 startColor = new Color32(0, 0, 0, 255);
        Color32 endColor = new Color32(0, 0, 0, 0);
        while (elapsedTime < fadeTime)
        {
            _sceneChangeFade.GetComponent<Image>().color = Color32.Lerp(startColor, endColor, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _sceneChangeFade.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(LoadScene("Fighting"));
    }
    public bool _isCursorOnTip = false;

    public void Cursor(bool state)
    {
        _isCursorOnTip = state;
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
                    if (!DataManager.instance._data.skipLoading)
                    {
                        _loadingText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "pressakey").Result;
                    }
                    if ((Input.anyKeyDown && !_isCursorOnTip) || DataManager.instance._data.skipLoading)
                    {
                        _sceneChangeFade.SetActive(true);
                        float elapsedTime = 0f;
                        float fadeTime = 1f;
                        while (elapsedTime < fadeTime)
                        {
                            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
                            _sceneChangeFade.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)(alpha * 255));
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        asyncLoad.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }
    }
}
