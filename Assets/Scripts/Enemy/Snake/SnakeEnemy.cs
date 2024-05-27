using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Dreamteck.Splines.Examples;
using UnityEngine;

public class SnakeEnemy : MonoBehaviour
{
    [SerializeField] private SnakePart _snakeHead;
    [SerializeField] private List<SnakePart> _snakeParts;
    [SerializeField] private SnakePart _snakeTail;
    [SerializeField] private int _maxLength;
    [SerializeField] private float distanceBetween = .4f;
    private SplineComputer _enemyPath;
    [SerializeField] private List<SnakePart> _partsToCreate;
    private float _offset = 2.7f;

    private void AddSplineToSnakeParts(SnakePart snakePart)
    {
        SplineFollower splineFollower = snakePart.GetComponent<SplineFollower>();
        if (splineFollower)
        {
            splineFollower.spline = _enemyPath;
        }
    } 
    
    private void AddSplineToSnakePartPosition(SnakePart snakePart)
    {
        SplinePositioner splinPos = snakePart.GetComponent<SplinePositioner>();
        if (splinPos)
        {
            splinPos.spline = _enemyPath;
        }
    } 
        
    public void CreateSnake(SplineComputer enemyPath)
    {
        _enemyPath = enemyPath;

        AddSplineToSnakeParts(_snakeHead);
        SnakePart snakeHead = Instantiate(_snakeHead, transform);

        SnakePart previousPart = snakeHead;
        Wagon.SplineSegment initialSegment = new Wagon.SplineSegment(_enemyPath, -1, Spline.Direction.Forward);
        previousPart.SetupRecursively(null, initialSegment);
        
        int snakeSize = Random.Range(1, _maxLength);
        
        for (int i = 0; i < snakeSize; i++)
        {
            var rand = Random.Range(0, _snakeParts.Count);
            
            SnakePart bodyPart = _snakeParts[rand];
            bodyPart.offset = _offset;
            SnakePart newSnakePart = Instantiate(bodyPart, transform);

            newSnakePart.front = previousPart;
            previousPart.back = newSnakePart;
            
            newSnakePart.SetupRecursively(previousPart, initialSegment);
            
            newSnakePart.UpdateOffset();

            previousPart = newSnakePart;
        }

        _snakeTail.offset = _offset;
        SnakePart tail = Instantiate(_snakeTail, transform);
        tail.front = previousPart;
        previousPart.back = tail;
        
        tail.SetupRecursively(previousPart, initialSegment);
        tail.UpdateOffset();
        
    }
    
}
