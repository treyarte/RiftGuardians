using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;
using Touch = UnityEngine.Touch;
using TouchPhase = UnityEngine.TouchPhase;

public class DragFingerOffset : MonoBehaviour
{
    private float deltaX, deltaY;
    private Rigidbody rb;
    private Plane plane = new Plane(Vector3.down, 0);
    private bool _canMove;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += StopMovement;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= StopMovement;
    }

    private void Start()
    {
        _canMove = true;
        rb = GetComponent<Rigidbody>();
    }

    private void StopMovement(Player player)
    {
        if (player.GetPlayerStatus() == PlayerStatus.Dead)
        {
            _canMove = false;
        }
    }

    private void FixedUpdate()
    {
        if (!_canMove)
        {
            return;
        }
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            var ray = Camera.main.ScreenPointToRay(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (plane.Raycast(ray, out float distance))
                    {
                        var worldPos = ray.GetPoint(distance);
                        // rb.MovePosition(new Vector3(worldPos.x - deltaX, 0,worldPos.y - deltaY));
                        deltaX = worldPos.x - transform.position.x;
                        deltaY = worldPos.z - transform.position.z;
                        
                    }
              
                    break;
                
                case TouchPhase.Moved:
                    if (plane.Raycast(ray, out float distance2))
                    {
                        var worldPos = ray.GetPoint(distance2);
                        var currentPos = transform.position;
                        //For moving up and down
                        // rb.MovePosition(new Vector3(worldPos.x - deltaX, 0,worldPos.z - deltaY));
                        rb.MovePosition(new Vector3(worldPos.x - deltaX, currentPos.y, currentPos.z));
                        
                    }
                    break;
                case TouchPhase.Ended:
                    rb.velocity = Vector3.zero;
                    break;
            }
        }
    }
}
