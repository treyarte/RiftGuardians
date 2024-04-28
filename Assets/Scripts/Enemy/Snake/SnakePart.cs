using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class singular snake body part 
/// </summary>
public class SnakePart : MonoBehaviour
{
    [SerializeField] private bool _isHead;

    public void SetIsHead(bool isHead)
    {
        _isHead = isHead;
    }

    public bool GetIsHead()
    {
        return _isHead;
    }
    private void OnEnable()
    {
        // throw new NotImplementedException();
    }

    private void OnDisable()
    {
        // throw new NotImplementedException();
    }
    
}
