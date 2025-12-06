using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    // Loading animation
    [Header("UI 元件")]
    public GameObject menuPanel;
    public GameObject loadingScreen; // 拉入你的 Loading Panel
    //public Slider slider;            // 拉入你的 Slider
    public Text progressText;        // (選用) 拉入顯示 % 的 Text
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuPanel.SetActive(true);
        loadingScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressNewGameButton() 
    {
        StartCoroutine(LoadAsynchronously("SampleScene"));
    }

    public void PressExitButton()
    {
        Application.Quit();
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        menuPanel.SetActive(false);
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the scene from being activated 
        operation.allowSceneActivation = false;

        float elapsedTime = 0f; // Elapsed time
        float maxLoadingTime = 1f; // Maximum loading time(fake)
        // Check if the scene is loaded
        while (!operation.isDone)
        {
            elapsedTime += Time.deltaTime;
            float timeProgress = Mathf.Clamp01(elapsedTime / maxLoadingTime);
        
            // Pick the smaller value between operation.progress and timeProgress
            float displayProgress = Mathf.Min(operation.progress, timeProgress);

            progressText.text = (displayProgress * 100f).ToString("F0") + "%";
            if (elapsedTime > maxLoadingTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        loadingScreen.SetActive(false);
        menuPanel.SetActive(true);
    }
}
