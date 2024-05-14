using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

/// <summary>
/// Health component for managing player health
/// </summary>
public class PlayerHealth : HealthManager
{
    private Animation _camAnim;
    private Player _player;
    
    public static event Action<float> DamagePlayer;
    public static event Action<float> HealthChanged;
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
        
        // if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount == 3)
        // {
        //     HandlePlayerTakeDamage(20);
        //     DamagePlayer.Invoke(20);
        // }
    }
    

    //TODO this needs to be moved into its own script
    /// <summary>
    /// Function for handle what happens when a player takes damage
    /// </summary>
    /// <param name="damage"></param>
    public void HandlePlayerTakeDamage(float damage)
    {
        SubtractHealth(damage);
        _camAnim.Play(_camAnim.clip.name);
        HealthChanged.Invoke(this.GetCurrentHealth());
    }
}
