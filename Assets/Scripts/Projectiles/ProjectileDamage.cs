using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 10f;

    private void Awake()
    {
        //TODO get damage amount
    }

    private void OnEnable()
    {
        HS_ProjectileMover.DoProjectileDamage += DealDamage;
    }
    
    private void OnDisable()
    {
        HS_ProjectileMover.DoProjectileDamage -= DealDamage;
    }


    private void DealDamage(Collision collision)
    {
        bool isDamageable = collision.gameObject.CompareTag("Damageable");
        var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

        if (isDamageable && enemyHealth != null)
        {
            enemyHealth.HandleEnemyTakeDamage(_damageAmount);
        }
    }
    
}
