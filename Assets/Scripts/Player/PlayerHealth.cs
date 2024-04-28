using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using UnityEngine;

/// <summary>
/// Health component for managing player health
/// </summary>
public class PlayerHealth : HealthManager
{
    private Animation _camAnim;
    private Player _player;
    
    public static event Action<Player> OnPlayerDeath;
    private void Awake()
    {
        _player = GetComponent<Player>();
        _camAnim = Camera.main.GetComponent<Animation>();
    }

    //Adding events
    private void OnEnable()
    {
        CrossSplineDmg.DealDamageOnDeath += HandlePlayerTakeDamage;
    }

    //Removing events
    private void OnDisable()
    {
        CrossSplineDmg.DealDamageOnDeath -= HandlePlayerTakeDamage;
    }

    private void Update()
    {
        if (this.GetCurrentHealth() <= 0)
        {
            //Kill player
        }
    }

    //TODO this needs to be moved into its own script
    /// <summary>
    /// Function for handle what happens when a player takes damage
    /// </summary>
    /// <param name="damage"></param>
    public void HandlePlayerTakeDamage(float damage)
    {
        Debug.Log("I ran");
        SubtractHealth(damage);
        _camAnim.Play(_camAnim.clip.name);
        var currPlayerStatus = _player.GetPlayerStatus();

        if (this.GetCurrentHealth() <= 0 && currPlayerStatus != PlayerStatus.Dead)
        {
            _player.SetPlayerStatus(PlayerStatus.Dead);
            OnPlayerDeath?.Invoke(_player);
        }

    }
}
