using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines.Examples;
using UnityEngine;

/// <summary>
/// The base class singular snake body part 
/// </summary>
public class SnakePart : Wagon
{
    [SerializeField] private bool _isHead;
    [SerializeField] private bool _isTail;
    
    public void SetIsHead(bool isHead)
    {
        _isHead = isHead;
    }

    public bool GetIsHead()
    {
        return _isHead;
    }
    
    public void SetIsTail(bool isTail)
    {
        _isTail = isTail;
    }

    public bool GetIsTail()
    {
        return _isTail;
    }
    private void OnEnable()
    {
        // throw new NotImplementedException();
    }
    
}
