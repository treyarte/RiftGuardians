using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner spawnManager;
    [SerializeField]
    private GameObject _levelResMenu;

    public int wavesCompleted;

    public int maxWaves;
    
    public int remainingEnemies;

    public static Action WaveCompleted;

    private void OnEnable()
    {
        EnemyHealth.KillEnemy += DecrementRemainEnemies;
    }

    private void OnDisable()
    {
        EnemyHealth.KillEnemy -= DecrementRemainEnemies;
    }

    // Start is called before the first frame update
    void Start()
    {
        remainingEnemies = spawnManager.GetTotalEnemiesInWave();
        maxWaves = spawnManager.GetTotalWaves();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            WaveCompleted.Invoke();
        }
    }

    private void DecrementRemainEnemies(int _)
    {
        if (remainingEnemies <= 0)
        {
            //Because we only have one level right now
            return;
        }
        
        remainingEnemies--;

        if (remainingEnemies <= 0)
        {
            wavesCompleted++;
            WaveCompleted?.Invoke();
        }
    }
}
