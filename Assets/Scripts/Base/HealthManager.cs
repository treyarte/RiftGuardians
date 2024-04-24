using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthManager : MonoBehaviour
{
   [SerializeField] private float _maxHealth = 100f;
   [SerializeField] private float _health = 100f;

   /// <summary>
   /// Get maximum health
   /// </summary>
   /// <returns></returns>
   public float GetMaxHealth()
   {
      return _maxHealth;
   }

   /// <summary>
   /// Get current 
   /// </summary>
   /// <returns></returns>
   public float GetCurrentHealth()
   {
      return _health;
   }
   
   /// <summary>
   /// Updates the health
   /// </summary>
   /// <param name="healAmount"></param>
   protected void AddHealth(float healAmount)
   {
      float newHealth = healAmount + _health;

      if (newHealth > _maxHealth)
      {
         //So when dont over heal nor add more health to the player
         _health = _maxHealth;
      }
      else
      {
         //If for some reason the newHealth is negative so we dont go into the negative
         _health = newHealth > 0 ? newHealth : 0;
      }
   }

   public void SubtractHealth(float subAmt)
   {
      
      if (_health > 0)
      {
         _health -= Mathf.Clamp(subAmt, 0, _health);
         return;
      }

      _health = 0;

      //Kill Player
   }
}
