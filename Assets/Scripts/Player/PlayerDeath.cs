using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    private PlayerHealth _playerHealth;
    public static event Action<Player> OnPlayerDeath;
    
    private void Start()
    {
        _playerHealth = _player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        //If the player is alive and their health is 0 we need to kill them
        if (_player.GetPlayerStatus() == PlayerStatus.Alive && _playerHealth.GetCurrentHealth() <= 0)
        {
            SetPlayerToDead();
        }
    }

    /// <summary>
    /// Updates the player status to dead and calls the player death event
    /// </summary>
    private void SetPlayerToDead()
    {
        _player.SetPlayerStatus(PlayerStatus.Dead);
        OnPlayerDeath?.Invoke(_player);
    }
}
