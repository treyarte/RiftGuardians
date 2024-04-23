using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes object always face the camera
/// </summary>
public class Billboard : MonoBehaviour
{
    private Vector3 cameraPos;

    // Start is called before the first frame update
    private void Awake()
    {
        
        cameraPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        //Direction from billboard to camera
        Vector3 toCam = cameraPos - transform.position;
        transform.rotation = Quaternion.LookRotation(toCam);
        transform.Rotate(0, 180, 0);
    }
}
