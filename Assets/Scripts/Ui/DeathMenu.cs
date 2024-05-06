using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private float minLoadScreenDelay = 4f;
    [SerializeField] private Slider progressBar;
    private TextMeshProUGUI loadingText;

    private void Start()
    {
        Time.timeScale = 1f;
        loadingText = progressBar.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void LoadLoadingScreen()
    {
        _loadingScreen.SetActive(true);

        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        // yield return new WaitForSeconds(minLoadScreenDelay);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        
        while (!asyncLoad.isDone)
        {
            
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingText.text = $"Loading...{progress:#0.##%}";
            progressBar.value = progress;

            yield return null;
        }
    }
}
