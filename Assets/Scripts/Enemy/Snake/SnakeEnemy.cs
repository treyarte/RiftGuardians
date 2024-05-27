using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Dreamteck.Splines.Examples;
using UnityEngine;
using Random = UnityEngine.Random;

public class SnakeEnemy : MonoBehaviour
{
    [SerializeField] private SnakePart _snakeHead;
    [SerializeField] private List<SnakePart> _snakeParts;
    [SerializeField] private SnakePart _snakeTail;
    [SerializeField] private int _maxLength;
    [SerializeField] private float distanceBetween = .4f;
    [SerializeField] private Dictionary<int, GameObject> _snakePartsCreated = new Dictionary<int, GameObject>();
    private SplineComputer _enemyPath;
    private SplineFollower _snakeHeadFollower;
    private float _offset = 2.7f;

    private void OnEnable()
    {
        EnemyHealth.KillEnemy += DestroySnakePart;
    }

    private void OnDisable()
    {
        EnemyHealth.KillEnemy -= DestroySnakePart;
    }

    
    /// <summary>
    /// Adds the spline to the SnakePart that has a spline follower component
    /// </summary>
    /// <param name="snakePart"></param>
    private void AddSplineToFollower(SnakePart snakePart)
    {
        SplineFollower splineFollower = snakePart.GetComponent<SplineFollower>();
        if (splineFollower)
        {
            splineFollower.spline = _enemyPath;
        }
    }

    /// <summary>
    /// Creates the snake actually body excluding its head and tail
    /// </summary>
    /// <remarks>
    /// The prevPart should start of as the head of the snake
    /// </remarks>
    /// <param name="prevPart"></param>
    private SplineTracer CreateSnakeBody(SplineTracer prevPart)
    {
        int snakeSize = Random.Range(1, _maxLength);
        for (int i = 0; i < snakeSize; i++)
        {
            var rand = Random.Range(0, _snakeParts.Count);
            
            SnakePart bodyPart = _snakeParts[rand];

            var splinePositioner = bodyPart.GetComponent<SplinePositioner>();
            if (!splinePositioner)
            {
                throw new Exception($"Spline Positioner doesn't exist for snake part at index: {i}");
            }

            splinePositioner.spline = _enemyPath;
            splinePositioner.followTarget = prevPart;
            splinePositioner.followTargetDistance = _offset;
            SnakePart newSnakePart = Instantiate(bodyPart, transform);
            
            _snakePartsCreated.Add(newSnakePart.gameObject.GetInstanceID(), newSnakePart.gameObject);
            
            var prevSnakePart = prevPart.gameObject.GetComponent<SnakePart>();
            prevSnakePart.backObjId = newSnakePart.gameObject.GetInstanceID();
            newSnakePart.frontObjId = prevPart.gameObject.GetInstanceID();

            prevPart = newSnakePart.GetComponent<SplinePositioner>();
        }

        return prevPart;
    }
    
    /// <summary>
    /// Creates a new snake and all of its parts  
    /// </summary>
    /// <param name="enemyPath"></param>
    public void CreateSnake(SplineComputer enemyPath)
    {
        _enemyPath = enemyPath;
        AddSplineToFollower(_snakeHead);
        SnakePart snakeHead = Instantiate(_snakeHead, transform);

        _snakeHeadFollower = snakeHead.GetComponent<SplineFollower>();

        _snakePartsCreated.Add(snakeHead.gameObject.GetInstanceID(), snakeHead.gameObject);
        
        SplineTracer previousPart = snakeHead.GetComponent<SplineFollower>();

        previousPart = CreateSnakeBody(previousPart);
        
        var tailPositioner = _snakeTail.GetComponent<SplinePositioner>();
        tailPositioner.followTarget = previousPart;
        tailPositioner.followTargetDistance = _offset;
        tailPositioner.spline = _enemyPath;
        var prevSnakePart = previousPart.GetComponent<SnakePart>();
        _snakeTail.backObjId = previousPart.gameObject.GetInstanceID();
        var snakeTail = Instantiate(_snakeTail, transform);
        prevSnakePart.backObjId = snakeTail.gameObject.GetInstanceID();
        _snakePartsCreated.Add(snakeTail.gameObject.GetInstanceID(), snakeTail.gameObject);
    }
    

    private void DestroySnakePart(int enemyId)
    {
       bool isValidId = _snakePartsCreated.TryGetValue(enemyId, out GameObject foundPart);

       if (!isValidId)
       {
           return;
       }

       _snakePartsCreated.Remove(enemyId);

       SnakePart snakePart = foundPart.GetComponent<SnakePart>();
       
        if (snakePart.GetIsHead())
        {
            Destroy(this.gameObject);
            return;
        }

        int frontId = snakePart.frontObjId;
        int backId = snakePart.backObjId;
        
        bool isValidFront = _snakePartsCreated.TryGetValue(frontId, out GameObject foundFront);
        bool isValidBack = _snakePartsCreated.TryGetValue(backId, out GameObject foundBack);

        if (isValidFront && isValidBack)
        {
            foundFront.GetComponent<SnakePart>().backObjId = backId;
            foundBack.GetComponent<SnakePart>().frontObjId = frontId;
            foundBack.GetComponent<SplinePositioner>().followTarget = foundFront.GetComponent<SplineTracer>();
            var pushBackPercent = snakePart.GetComponent<SplinePositioner>().GetPercent();
            _snakeHeadFollower.SetPercent(pushBackPercent);
        }
        
    }
}
