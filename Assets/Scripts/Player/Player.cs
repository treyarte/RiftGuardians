using System;
using System.Collections;
using System.Collections.Generic;
using Script.Enums;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private GameObject _playerVisual;

    private bool isWalking;
    [SerializeField]private PlayerStatus _playerStatus;

    private void Awake()
    {
        _playerStatus = PlayerStatus.Alive;
    }

    private void Update()
    {
        //Stopping movement
        if (_playerStatus == PlayerStatus.Dead)
        {
            return;
        }
        
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        var currPos = transform.position;
        // Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        Vector3 moveDir = new Vector3(inputVector.x, 0f, 0f);
        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        isWalking = moveDir != Vector3.zero;

        float roateSpeed = 10f;
        _playerVisual.transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * roateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public PlayerStatus GetPlayerStatus()
    {
        return _playerStatus;
    }
    
    public void SetPlayerStatus(PlayerStatus status)
    {
        _playerStatus = status;
    }
}
