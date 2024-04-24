using System;
using Dreamteck.Splines;
using Unity.VisualScripting;
using UnityEngine;


public class SpawnEnemy : MonoBehaviour
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
        SplineTrigger trigger = new SplineTrigger( SplineTrigger.Type.Forward);
        trigger.position = 1;
        trigger.name = "Enemy Reached End";
        trigger.onCross.AddListener(OnTriggerCrossed);
        triggerGroup.triggers = new SplineTrigger[] { trigger };
        _mainSpline.triggerGroups = new TriggerGroup[] { triggerGroup };
    }

    private void OnTriggerCrossed(SplineUser user )
    {
        var enemyHealth = user.gameObject.GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            return;
        }

        float currentHealth = enemyHealth.GetCurrentHealth();
        DealDamageOnDeath?.Invoke(currentHealth);
    }
    
    
    
    
}

