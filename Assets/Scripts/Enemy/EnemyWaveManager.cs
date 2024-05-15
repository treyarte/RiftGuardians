using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [SerializeField] private EnemyWave[] _waves;
    [SerializeField] private SplineComputer _mainSpline;
    private EnemyWave _currentWave { get; set; }
    public GameObject[] currentEnemies;

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
        foreach (var enemy in currWave.enemies)
        {
            //TODO change this to a check when adding other enemies
            var snake = enemy.GetComponent<SpawnSnakes>();
            if (snake)
            {
                snake._mainSpline = _mainSpline;
                var newSnake = Instantiate(snake);
                currentEnemies = currWave.enemies;
                yield return new WaitUntil(() => newSnake.isSnakeBuilt);
                yield return new WaitForSeconds(5f);
            }
        }
        
        // yield return new WaitForSeconds(currWave.duration);
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
