using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : HealthManager
{
   private TextMeshPro _childText;
   private bool _isChildTextNotNull;
   private MeshRenderer _enemyVisuals;
   [SerializeField] private Material _hitMaterial;
   
   public static event Action<GameObject> KillEnemy;

   private void Awake()
   {
      _childText = GetComponentInChildren<TextMeshPro>();
      _isChildTextNotNull = _childText != null;
      _enemyVisuals = GetComponentInChildren<MeshRenderer>();
      _childText.text = $"{this.GetCurrentHealth()}";
   }

   private void Update()
   {
      if (this.GetCurrentHealth() <= 0)
      {
         HandleEnemyDeath();
      }
   }

   private void ToggleEnemyHitEffect()
   {
      StartCoroutine(ChangeColorCoroutine());
   }

   IEnumerator ChangeColorCoroutine()
   {
      var currentMat = _enemyVisuals.material;
      var newMats = new List<Material>() { _hitMaterial };
      _enemyVisuals.SetMaterials(newMats);
      yield return new WaitForSeconds(0.2f);
      _enemyVisuals.SetMaterials(new List<Material>(){currentMat});
   }
   
   /// <summary>
   /// When enemy takes damage reduce its health and have some type of
   /// effect to show the enemy has been hurt
   /// </summary>
   public void HandleEnemyTakeDamage(float dmgAmount)
   {
      this.SubtractHealth(dmgAmount);
      float currentHealth = this.GetCurrentHealth();
      
      if (_isChildTextNotNull)
      {
         _childText.text = $"{this.GetCurrentHealth()}";
         ToggleEnemyHitEffect();
      }
   }
   
   public void HandleEnemyDeath()
   {
      GameObject enemy;
      int enemyId = (enemy = this.gameObject).GetInstanceID();
      Debug.Log($"I Ran:{enemyId}");
      KillEnemy?.Invoke(enemy);
      DestroyImmediate (this.gameObject);
   }
   
}
