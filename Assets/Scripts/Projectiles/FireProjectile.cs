using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField]
    private int prefab;
    public GameObject FirePoint;
    public GameObject[] prefabs;
    private float spawnTimer = 0f;
    public float spawnInterval = 0.5f;
    private bool _canShoot;

    private void OnEnable()
    {
        PlayerDeath.OnPlayerDeath += StopShooting;
    }

    private void OnDisable()
    {
        PlayerDeath.OnPlayerDeath -= StopShooting;
    }

    private void Awake()
    {
        _canShoot = true;
    }

    private void Update()
    {
        ShootProjectile();
    }

    private void StopShooting(Player player)
    {
        if (player.GetPlayerStatus() == PlayerStatus.Dead)
        {
            _canShoot = false;
        }
    }

    private void ShootProjectile()
    {
        if (!_canShoot)
        {
            return;
        }
        
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnInterval)
        {
            Instantiate(prefabs[prefab], FirePoint.transform.position, FirePoint.transform.rotation);
            spawnTimer = 0f;
        }
    }
}
