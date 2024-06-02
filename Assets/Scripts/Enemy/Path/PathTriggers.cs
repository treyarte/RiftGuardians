using System;
using Dreamteck.Splines;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles spawning enemies into and creates the Spline Trigger events for the enemies
/// </summary>
public class PathTriggers : MonoBehaviour
{
    [SerializeField] private SplineComputer _mainSpline;
    public static event Action<bool> StartPositionCheck;
    public static event Action<float> DealDmgOnCross;
    private float _checkEnemyTriggerPos = 0.058f;
    private float _dealEnemyDmgTriggerPos = 1f;
    
    private void Awake()
    {
        if (_mainSpline == null)
        {
            return;
        }

        // CreateStartPosCheckTrigger();

    }

    private void CreateStartPosCheckTrigger()
    {
        TriggerGroup triggerGroup = new TriggerGroup();
        triggerGroup.name = "Start Pos Group";
        SplineTrigger trigger = new SplineTrigger( SplineTrigger.Type.Forward);
        trigger.position = _checkEnemyTriggerPos;
        trigger.name = "Enemy Check";
        trigger.onCross.AddListener(CheckEnemy);
        trigger.workOnce = false;
        
        SplineTrigger trigger2 = new SplineTrigger( SplineTrigger.Type.Forward);
        trigger2.position = _dealEnemyDmgTriggerPos;
        trigger2.name = "Deal Enemy Damage";
        trigger2.onCross.AddListener(DealEnemyDmg);
        trigger2.workOnce = false;
        
        triggerGroup.triggers = new SplineTrigger[] { trigger, trigger2 };
        _mainSpline.triggerGroups = new TriggerGroup[] { triggerGroup };
    }

    
    /// <summary>
    /// Function that invokes an event to deal damage to the player
    /// when the spline user crosses a trigger point and the destroy splineUser
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public void DealEnemyDmg(SplineUser enemy )
    {
        var enemyHealth = enemy.gameObject.GetComponent<EnemyHealth>();
        
        if (enemyHealth == null)
        {
            return;
        }

        float currentHealth = enemyHealth.GetCurrentHealth();
        DealDmgOnCross?.Invoke(currentHealth);

        var snakeEnemy = enemy.GetComponentInParent<SnakeEnemy>();

        if (snakeEnemy == null)
        {
            throw new Exception("Failed to find snake part parent");
        }

        var headNode = snakeEnemy._snakePartsOrderedList.Head();

        snakeEnemy._snakePartsOrderedList.RemoveFirst();
        var newHeadNode = snakeEnemy._snakePartsOrderedList.Head();

        // var splineFollow = headNode.Data.GetComponent<SplineFollower>();

        var follower = newHeadNode.Data.AddComponent<SplineFollower>();
        var positioner = newHeadNode.Data.GetComponent<SplinePositioner>();
         
        follower.spline = _mainSpline;
        follower.clipFrom = positioner.position;
        follower.useTriggers = true;
        follower.triggerGroup = 0;
        
        // if (newHeadNode.Data.GetComponent<SplineFollower>())
        // {
        //     Destroy(newHeadNode.Data.GetComponent<SplinePositioner>());
        //     var spl = newHeadNode.Data.GetComponent<SplineFollower>();
        //     spl.followSpeed = splineFollow.followSpeed;
        //     spl.spline = _mainSpline;
        //     // spl.SetPercent(0.7f);
        //     spl.follow = true;
        //     var travel = spl.Travel(0.0,127f,  Spline.Direction.Forward);
        //     spl.SetPercent(travel);
        //     spl.enabled = true;
        //     
        //     
        //     
        // }
        
        
        
        // Destroy(enemy.gameObject); 
    }
    
    /// <summary>
    /// Checks what type of enemy is on the start position and invokes the event
    /// </summary>
    public void CheckEnemy(SplineUser enemy )
    {
        var snake = enemy.gameObject.GetComponent<SnakePart>();
    
        if (snake == null || snake.GetIsTail())
        {
            StartPositionCheck?.Invoke(true);
            Debug.Log("OnStartPosCross Activated");
        }
        
    }
    
}
