using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Enemy wave")]
public class EnemyWave : ScriptableObject   
{
    [SerializeField] public float duration = 60f;
    [SerializeField] public bool hasStarted = false;
    [SerializeField] public bool isCompleted = false;
    [SerializeField] public GameObject[] enemies;
}
