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
    

    private void Start()
    {
        foreach (var currWave in _waves)
        {
        }
        StartCoroutine(SpawnWave(_waves.First()));
    }

    private IEnumerator SpawnWave(EnemyWave currWave)
    {
        foreach (var enemy in currWave.enemies)
        {
            //TODO change this to a check when adding other enemies
            var snake = enemy.GetComponent<SpawnSnakes>();
            if (snake)
            {
                snake._mainSpline = _mainSpline;
                var newSnake = Instantiate(snake);
                yield return new WaitUntil(() => newSnake.isSnakeBuilt);
                Debug.Log("Snake Built");
                yield return new WaitForSeconds(5f);
                Debug.Log("10s after snake built");
            }
        }
        
        // yield return new WaitForSeconds(currWave.duration);
    }
}
