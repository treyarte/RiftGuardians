using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prevents player from moving outside of screen
/// </summary>
/// <remarks>
/// If you want resizing the screen to work add SetScreenBound to update
/// </remarks>
public class RestrictMovement : MonoBehaviour
{
    private Camera mainCam;
    private float minX, maxX, minY, maxY;

    private void Start()
    {
        SetScreenBounds();   
    }

    // Update is called once per frame
    void Update()
    {
        ClampPlayerMovementToScreen();
    }

    /// <summary>
    /// Get the screen size to set the min and max of the restriction from the camera
    /// </summary>
    private void SetScreenBounds()
    {
        mainCam = Camera.main;

        if (!mainCam)
        {
            return;
        }

        float camYPos = mainCam.transform.position.y;
        
        //Set screen bounds in world space
        Vector3 screenMin = mainCam.ScreenToWorldPoint(new Vector3(0, 0, camYPos));
        Vector3 screenMax = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camYPos));

        minX = screenMin.x;
        maxX = screenMax.x;
        minY = screenMin.z;
        maxY = screenMax.z;
    }

    /// <summary>
    /// Restricts the player movement
    /// </summary>
    private void ClampPlayerMovementToScreen()
    {
        Vector3 playerPos = transform.position;

        playerPos.x = Mathf.Clamp(playerPos.x, minX, maxX);
        playerPos.z = Mathf.Clamp(playerPos.z, minY, maxY);

        transform.position = playerPos;
    }
}
