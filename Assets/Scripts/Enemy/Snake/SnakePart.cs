using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Dreamteck.Splines.Examples;
using UnityEngine;
using Untils;

/// <summary>
/// The base class singular snake body part 
/// </summary>
public class SnakePart : MonoBehaviour
{
    [SerializeField] private bool _isHead;
    [SerializeField] private bool _isTail;
    public Node<GameObject> snakeNode; 
    public int frontObjId;
    public int backObjId;
    
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
}
