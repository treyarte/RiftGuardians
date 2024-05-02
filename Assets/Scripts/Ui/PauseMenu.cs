using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private bool isGamePaused;
    [SerializeField] private GameObject pauseMenuUi;

    private void OnEnable()
    {
        GameInput.onPauseEvent += TogglePause;
    }

    private void OnDisable()
    {
        GameInput.onPauseEvent -= TogglePause;
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            isGamePaused = false;
            pauseMenuUi.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            isGamePaused = true;
            pauseMenuUi.SetActive(true);
        }
        
        
    }
}
