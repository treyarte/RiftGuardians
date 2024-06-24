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
    private int enemies = 0;
    private int enemiesDefeated = 0;
    private void OnEnable()
    {
        SnakeEnemy.DestroySnake += onEnemyDestroy;
    }

    private void OnDisable()
    {
        SnakeEnemy.DestroySnake -= onEnemyDestroy;
    }

    private void Start()
    {
       waves = _enemyWaveManager._waves.Length;
       enemies = _enemyWaveManager.GetTotalEnemiesInWave();
       
       SetWaveText($"0/{waves}");
       SetEnemyText($"0/{enemies}");
    }

    private void onEnemyDestroy()
    {
        enemiesDefeated++;
        enemies = _enemyWaveManager.GetTotalEnemiesInWave();

        if (enemiesDefeated > enemies)
        {
            enemiesDefeated = 0;
        }
        
        SetEnemyText($"{enemiesDefeated}/{enemies}");
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
