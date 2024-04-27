using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class SpawnSnakes : MonoBehaviour
{
    [SerializeField] private float distanceBetween = .2f;
    [SerializeField] private List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField] private List<GameObject> snakeBody = new List<GameObject>();
    [SerializeField] private SplineComputer _mainSpline;
    [SerializeField] private bool DoPushback = false;
    
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
            pushBack();
    }
    
    private void pushBack()
    {
        if (snakeBody.Count < 2 || !DoPushback)
        {
            return;
        }

        var head = snakeBody[0];
        var body = snakeBody[1];

        var bodyFollow = body.GetComponent<SplineFollower>();
        var follower = head.GetComponent<SplineFollower>();

        var percent = bodyFollow.GetPercent();
        follower.SetPercent(percent);
        
        Destroy(body);
        snakeBody.RemoveAt(1);
        DoPushback = false;
    }

    IEnumerator Wait(SplineFollower splineFollower)
    {
        
       
        yield return new WaitForSeconds(0.1f);
        splineFollower.follow = true;
    }
    
    void CreateBodyParts()
    {
        if (snakeBody.Count == 0)
        {
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            SplineFollower temp1SplineFollower = temp1.GetComponent<SplineFollower>();
            if (temp1SplineFollower)
            {
                temp1SplineFollower.spline = _mainSpline;
                
            }
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }
        
        countUp += Time.deltaTime;

        if (countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], transform.position, transform.rotation,
                transform);
            SplineFollower tempSplineFollower = temp.GetComponent<SplineFollower>();
            if (tempSplineFollower)
            {
                tempSplineFollower.spline = _mainSpline;

            }
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            countUp = 0;
        }

    }

}
