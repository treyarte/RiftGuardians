using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Whether or not the game is paused
    /// </summary>
    /// <remarks>
    /// We may need to move this to a manager if we plan to use this else where
    /// </remarks>
    private bool _isGamePaused;
    
    /// <summary>
    /// The actual ui menu
    /// </summary>
    [SerializeField] private GameObject pauseMenuUi;
    
    //Setting up button press events
    private void OnEnable()
    {
        GameInput.onPauseEvent += TogglePause;
    }

    private void OnDisable()
    {
        GameInput.onPauseEvent -= TogglePause;
    }

    /// <summary>
    /// Pause and unpauses the game
    /// </summary>
    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            _isGamePaused = false;
            pauseMenuUi.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            _isGamePaused = true;
            pauseMenuUi.SetActive(true);
        }
    }
    /// <summary>
    /// Exits the game from the pause menu
    /// </summary>
    public void ExitGameFromPause()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
