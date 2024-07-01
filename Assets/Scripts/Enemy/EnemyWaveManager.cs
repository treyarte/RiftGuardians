using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    
    [SerializeField] public EnemyWave[] _waves;
    [SerializeField] private SplineComputer _mainSpline;
    [SerializeField] private bool _canSpawnEnemy = true;
    [SerializeField] private bool _waveInProgress = false;
    [SerializeField] private int _currWaveIndex = 0;
    [SerializeField] private int _enemiesDefeated = 0;

    public static event Action<int> WaveCompleted;
    public static event Action<int> AllWavesCompleted;
    
    private void OnEnable()
    {
        PathTriggers.StartPositionCheck += UpdateCanSpawn;
        SnakeEnemy.DestroySnake += UpdateEnemyDefeated;
    }

    private void OnDisable()
    {
        PathTriggers.StartPositionCheck -= UpdateCanSpawn;
        SnakeEnemy.DestroySnake -= UpdateEnemyDefeated;
    }
    
    private void Update()
    {
        if (!_waveInProgress)
        {
            StartWave(_currWaveIndex);
        }
    }

    private void StartWave(int waveIndex)
    {
        if (_waves.Length - 1 < waveIndex)
        {
            return;
        }

        var wave = _waves[waveIndex];
        _waveInProgress = true;
        StartCoroutine(SpawnWave(wave));
    }

    private IEnumerator SpawnWave(EnemyWave currWave)
    {
        currWave.hasStarted = true;
        StartCoroutine(WaitToSpawnEnemy());
        var index = 0;
        foreach (var enemy in currWave.enemies)
        {
            //TODO change this to a check when adding other enemies
            var newEnemy = Instantiate(enemy);
            var snake = newEnemy.GetComponent<SnakeEnemy>();
            if (snake)
            {
                snake.CreateSnake(_mainSpline);
            }

            _canSpawnEnemy = false;
            index++;
            yield return new WaitUntil(() => _canSpawnEnemy);
            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator WaitToSpawnEnemy()
    {
        yield return new WaitUntil(() => _canSpawnEnemy);
    }

    private void UpdateCanSpawn(bool canSpawn)
    {
        _canSpawnEnemy = canSpawn;
    }

    private void UpdateEnemyDefeated()
    {
        _enemiesDefeated++;
        var wave = _waves[_currWaveIndex];

        if (_enemiesDefeated <= wave.enemies.Length - 1)
        {
            return;
        }
        
        _waveInProgress = false;
        
        if (_currWaveIndex >= _waves.Length - 1)
        {
            Debug.Log("WAVE COMPLETED");
            _currWaveIndex++;
            WaveCompleted?.Invoke(_currWaveIndex);
            AllWavesCompleted?.Invoke(_currWaveIndex + 1);
            return;
        }
        
        _currWaveIndex++;
        _enemiesDefeated = 0;
        WaveCompleted?.Invoke(_currWaveIndex);
    }

    public int GetTotalEnemiesInWave()
    {
        int totalEnemies = 0;
        if (_waves.Length > _currWaveIndex)
        {
            var wave = _waves[_currWaveIndex];
            totalEnemies = wave.enemies.Length;
        }
        else
        {
            var wave = _waves.LastOrDefault();
            if (!wave) return 0;
            totalEnemies = wave.enemies.Length;
        }

        return totalEnemies;
    }

    public int GetTotalWaves()
    {
        return _waves.Length;
    }
    
    
}
