using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class EnemyCrossPath : MonoBehaviour
{
    [SerializeField] private SplineComputer _mainSpline;
    private float _triggerPosition = 0.058f;
    
    public static event Action<bool> StartPositionCheck;

    private void Awake()
    {
        if (_mainSpline == null)
        {
            return;
        }

        CreateStartPosCheckTrigger();

    }

    private void CreateStartPosCheckTrigger()
    {
        TriggerGroup triggerGroup = new TriggerGroup();
        triggerGroup.name = "Start Pos Group";
        SplineTrigger trigger = new SplineTrigger( SplineTrigger.Type.Forward);
        trigger.position = _triggerPosition;
        trigger.name = "Enemy Start Pos Check";
        trigger.onCross.AddListener(OnStartPosCross);
        trigger.workOnce = false;
        triggerGroup.triggers = new SplineTrigger[] { trigger };
        _mainSpline.triggerGroups = new TriggerGroup[] { triggerGroup };
    }

    /// <summary>
    /// Checks what type of enemy is on the start position and invokes the event
    /// </summary>
    /// <param name="enemy"></param>
    private void OnStartPosCross(SplineUser enemy )
    {
        var snake = enemy.gameObject.GetComponent<SnakePart>();

        if (snake == null || snake.GetIsTail())
        {
            StartPositionCheck?.Invoke(true);
            Debug.Log("OnStartPosCross Activated");
        }
        
    }
}
