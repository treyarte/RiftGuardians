using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    
    [SerializeField] private EnemyWave[] _waves;
    [SerializeField] private SplineComputer _mainSpline;
    [SerializeField] private bool _canSpawnEnemy = true; 
    private EnemyWave _currentWave { get; set; }
    public GameObject[] currentEnemies;

    private void OnEnable()
    {
        PathTriggers.StartPositionCheck += UpdateCanSpawn;
    }

    private void OnDisable()
    {
        PathTriggers.StartPositionCheck -= UpdateCanSpawn;
    }

    private void Awake()
    {
        foreach (var currWave in _waves)
        {
        }

        _currentWave = _waves.First();
        StartCoroutine(SpawnWave(_waves.First()));
    }

    private IEnumerator SpawnWave(EnemyWave currWave)
    {
        
        // _currentWave = currWave;
        _currentWave.hasStarted = true;
        StartCoroutine(WaitToSpawnEnemy());
        var index = 0;
        foreach (var enemy in currWave.enemies)
        {
            // if (index >= 1)
            // {
            //     break;
            // }
                
            // yield return _currentWave;
            
            //TODO change this to a check when adding other enemies
            var newEnemy = Instantiate(enemy);
            var snake = newEnemy.GetComponent<SnakeEnemy>();
            if (snake)
            {
                snake.CreateSnake(_mainSpline);
                currentEnemies = currWave.enemies;
            }

            _canSpawnEnemy = false;
            index++;
            yield return new WaitUntil(() => _canSpawnEnemy);
            yield return new WaitForSeconds(5f);
        }

        // yield return new WaitForSeconds(currWave.duration);
    }

    private IEnumerator WaitToSpawnEnemy()
    {
        yield return new WaitUntil(() => _canSpawnEnemy);
    }

    private void UpdateCanSpawn(bool canSpawn)
    {
        _canSpawnEnemy = canSpawn;
    }

    public int GetTotalEnemiesInWave()
    {
        return _currentWave.enemies.Length;
    }

    public int GetTotalWaves()
    {
        return _waves.Length;
    }
    
    
}
