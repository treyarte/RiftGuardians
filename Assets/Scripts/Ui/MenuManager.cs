using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject deathMenuUi;
    [SerializeField] private GameObject pauseMenuUi;
    private void OnEnable()
    {
        PlayerDeath.OnPlayerDeath += EnableDeathMenu;
    }

    private void OnDisable()
    {
        PlayerDeath.OnPlayerDeath -= EnableDeathMenu;
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
}
