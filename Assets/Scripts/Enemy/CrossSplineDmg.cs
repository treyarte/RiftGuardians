using System;
using Dreamteck.Splines;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles spawning enemies into and creates the Spline Trigger events for the enemies
/// </summary>
public class CrossSplineDmg : MonoBehaviour
{
    [SerializeField] private SplineComputer _mainSpline;
    
    public static event Action<float> DealDamageOnDeath;

    private void Start()
    {
        if (_mainSpline == null)
        {
            return;
        }

        TriggerGroup triggerGroup = new TriggerGroup();
        triggerGroup.name = "Enemy Group";
        SplineTrigger trigger = new SplineTrigger( SplineTrigger.Type.Forward);
        trigger.position = 1;
        trigger.name = "Enemy Reached End";
        trigger.onCross.AddListener(OnTriggerCrossed);
        triggerGroup.triggers = new SplineTrigger[] { trigger };
        _mainSpline.triggerGroups = new TriggerGroup[] { triggerGroup };
    }

    /// <summary>
    /// Function that invokes an event to deal damage to the player
    /// when the a spline user crosses a trigger point and the destroy splineUser
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private void OnTriggerCrossed(SplineUser enemy )
    {
        var enemyHealth = enemy.gameObject.GetComponent<EnemyHealth>();
        
        if (enemyHealth == null)
        {
            return;
        }

        float currentHealth = enemyHealth.GetCurrentHealth();
        DealDamageOnDeath?.Invoke(currentHealth);
        Destroy(enemy.gameObject); 
    }
    
    
    
    
}

