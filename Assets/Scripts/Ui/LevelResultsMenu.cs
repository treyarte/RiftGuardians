using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelResultsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _levelResultsScreen;

    private void OnEnable()
    {
        EnemyWaveManager.AllWavesCompleted += ToggleResultsScreen;
    }

    private void OnDisable()
    {
        EnemyWaveManager.AllWavesCompleted -= ToggleResultsScreen;
    }

    private void ToggleResultsScreen(int _)
    {
        _levelResultsScreen.SetActive(!_levelResultsScreen.activeSelf);
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }
    
    
}
