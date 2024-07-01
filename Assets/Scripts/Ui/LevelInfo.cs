using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveInfoText;
    [SerializeField] private TextMeshProUGUI _enemiesInfoText;
    [SerializeField] private EnemyWaveManager _enemyWaveManager;
    private int waves = 0;
    private int enemyCount = 0;
    private int totalEnemiesDefeated = 0;
    private int enemiesDefeatedInWave = 0;
    private void OnEnable()
    {
        SnakeEnemy.DestroySnake += onEnemyDestroy;
        EnemyWaveManager.WaveCompleted += onWaveCompleted;
    }

    private void OnDisable()
    {
        SnakeEnemy.DestroySnake -= onEnemyDestroy;
        EnemyWaveManager.WaveCompleted -= onWaveCompleted;
    }

    private void Start()
    {
       waves = _enemyWaveManager._waves.Length;
       enemyCount = _enemyWaveManager.GetTotalEnemiesInWave();
       
       SetWaveText($"0/{waves}");
       SetEnemyText($"0/{enemyCount}");
    }

    private void onEnemyDestroy()
    {
        enemiesDefeatedInWave++;
        enemyCount = _enemyWaveManager.GetTotalEnemiesInWave();
        SetEnemyText($"{enemiesDefeatedInWave}/{enemyCount}");
    }

    /// <summary>
    /// Update the wave text and reset the enemies counter
    /// </summary>
    private void onWaveCompleted(int waveNum)
    {
        SetWaveText($"{waveNum}/{waves}");
        enemyCount = _enemyWaveManager.GetTotalEnemiesInWave();
        totalEnemiesDefeated = enemiesDefeatedInWave;
        enemiesDefeatedInWave = 0;
        SetEnemyText($"0/{enemyCount}");
    }

    private void SetWaveText(string text)
    {
        _waveInfoText.text = $"{text} Waves";
    }
    
    private void SetEnemyText(string text)
    {
        _enemiesInfoText.text = $"{text} Enemies";
    }
    

}
