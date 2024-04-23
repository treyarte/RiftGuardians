using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDeath : MonoBehaviour
{
    [SerializeField] private float DmgAmount;
    private Camera _mainCamera;

    public static event Action<float> DoDamage;

    public void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void DealDamage()
    {
        Debug.Log("Event Triggered");
        var enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            float amountOfDamage = enemyHealth.GetCurrentHealth();
            DoDamage?.Invoke(amountOfDamage);
            
        }
    }
}
