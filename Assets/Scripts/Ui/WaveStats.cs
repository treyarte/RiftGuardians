using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WaveStats : MonoBehaviour
{
    [SerializeField] public EnemyWaveManager WaveManager;
    [SerializeField] private TextMeshProUGUI _waveText;

    private void Start()
    {
        UpdateWaveNumber(0);
    }

    private void OnEnable()
    {
        EnemyWaveManager.WavesCompleted += UpdateWaveNumber;
    }

    private void OnDisable()
    {
        EnemyWaveManager.WavesCompleted -= UpdateWaveNumber;
    }

    private void UpdateWaveNumber(int currWave)
    {
        _waveText.text = $"{currWave}/{WaveManager.GetTotalWaves()}";
    } 
}
