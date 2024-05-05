using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class CautionLine : MonoBehaviour
{
    private LineRenderer _lr;
    private LineTextureMode _textureMode = LineTextureMode.Tile;
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private Material lineMat;
    private void Awake()
    {
        _lr = gameObject.AddComponent<LineRenderer>();
    }

    private void Start()
    {
        SetUpLine();
    }

    private void SetUpLine()
    {
        _lr.positionCount = points.Length;
        _lr.useWorldSpace = true;
        _lr.SetMaterials(new List<Material>(){lineMat});
        _lr.textureMode = _textureMode;
        
        bool canSetColor = ColorUtility.TryParseHtmlString("#FFFF00", out Color cautionColor);
        if (canSetColor)
        {
            _lr.startColor = cautionColor;
            _lr.endColor = cautionColor;
        }
        
        _lr.startWidth = .5f;
        _lr.endWidth = .5f;

        for (int i = 0; i < points.Length; i++)
        {
            _lr.SetPosition(i, points[i].position);
        }
    }
}
