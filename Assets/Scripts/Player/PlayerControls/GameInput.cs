using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public static Action onPauseEvent;
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Ui.Enable();
    }

    private void Update()
    {
        if (_playerInputActions.Ui.Pause.triggered)
        {
            onPauseEvent?.Invoke();
        }
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;

        return inputVector;
    }
    
}
