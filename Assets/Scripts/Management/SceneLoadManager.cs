using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

public class SceneLoadManager : MonoBehaviour
{
    public Slider _loadingBar;
    public GameObject _sceneChangeFade;
    public TextMeshProUGUI _loadingText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {

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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;

        while (!asyncLoad.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            if (asyncLoad.progress < 0.9f)
            {
                _loadingText.text = (_loadingBar.value * 100f) + "%";
                _loadingBar.value = Mathf.Lerp(asyncLoad.progress, 1f, timer);

                if (_loadingBar.value >= asyncLoad.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                _loadingBar.value = Mathf.Lerp(_loadingBar.value, 1f, timer);
                _loadingText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "pressakey").Result;

                if (_loadingBar.value == 1f)
                {
                    if (Input.anyKeyDown)
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
