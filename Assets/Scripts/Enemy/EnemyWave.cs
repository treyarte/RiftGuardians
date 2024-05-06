using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float duration;
    //because
    public bool hasStarted { get; set; }
    public bool isCompleted { get; set; }

}
