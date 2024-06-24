using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject deathMenuUi;
    [SerializeField] private GameObject pauseMenuUi;
    [SerializeField] private GameObject levelResMenuUi;
    private void OnEnable()
    {
        PlayerDeath.OnPlayerDeath += EnableDeathMenu;
        CompleteLevel.WaveCompleted += EnableLevelResults;
    }

    private void OnDisable()
    {
        PlayerDeath.OnPlayerDeath -= EnableDeathMenu;
        CompleteLevel.WaveCompleted -= EnableLevelResults;
    }

    private void EnableDeathMenu(Player _)
    {
        deathMenuUi.SetActive(true);
        
        //Making sure other menus are not enabled
        if (pauseMenuUi.activeSelf)
        {
            pauseMenuUi.gameObject.SetActive(false);
        }
    }

    private void EnableLevelResults()
    {
        levelResMenuUi.SetActive(true);
        Time.timeScale = 0f;
        
        if (pauseMenuUi.activeSelf)
        {
            pauseMenuUi.gameObject.SetActive(false);
        }
    }
}
