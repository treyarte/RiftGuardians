using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private float distanceBetween = .2f;
    [SerializeField] private List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField] private SplineComputer _mainSpline;
    [SerializeField] private List<GameObject> snakeBody = new List<GameObject>();
    private float countUp = 0;
    
    private void Start()
    {
        CreateBodyParts();
    }

    private void Update()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        SnakeMovement();
    }

    void CreateBodyParts()
    {
        if (snakeBody.Count == 0)
        {
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp1.GetComponent<MarkerManager>())
            {
                temp1.AddComponent<MarkerManager>();
            }

            if (temp1.GetComponent<SplineFollower>())
            {
                temp1.GetComponent<SplineFollower>().spline = _mainSpline;
                temp1.GetComponent<SplineFollower>().wrapMode = SplineFollower.Wrap.Loop;

            }
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }
        
        MarkerManager markM = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();

        if (countUp == 0)
        {
            markM.ClearMarkerList();
        }
        
        countUp += Time.deltaTime;

        if (countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position, markM.markerList[0].rotation,
                transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            
            if (temp.GetComponent<SplineFollower>())
            {
                temp.GetComponent<SplineFollower>().spline = _mainSpline;

            }
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }

    }

    void SnakeMovement()
    {
        if (snakeBody.Count > 1)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager markM = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }
}
