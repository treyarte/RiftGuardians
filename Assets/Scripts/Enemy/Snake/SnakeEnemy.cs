using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class SnakeEnemy : MonoBehaviour
{
    [SerializeField] private SnakePart _snakeHead;
    [SerializeField] private List<SnakePart> _snakeParts;
    [SerializeField] private SnakePart _snakeTail;
    [SerializeField] private int _maxLength;
    [SerializeField] private float distanceBetween = .4f;
    private SplineComputer _enemyPath;
    private void CreateSnakeBody()
    {
        int snakeSize = Random.Range(1, _maxLength);

        for (int i = 0; i < snakeSize; i++)
        {
            var rand = Random.Range(0, _snakeParts.Count);
            
            GameObject bodyPart = _snakeParts[rand].gameObject;

            // var snakePart = Instantiate(bodyPart, transform);

            StartCoroutine(WaitByDistance(bodyPart));
        }
    }

    private void AddSplineToSnakeParts(GameObject snakePart)
    {
        SplineFollower splineFollower = snakePart.GetComponent<SplineFollower>();
        if (splineFollower)
        {
            splineFollower.spline = _enemyPath;
        }
    } 
        
    public void CreateSnake(SplineComputer enemyPath)
    {
        _enemyPath = enemyPath;
        var snakeHeadObj = Instantiate(_snakeHead, transform);
        StartCoroutine(WaitByDistance(snakeHeadObj.gameObject));
        
        CreateSnakeBody();
        
        // var snakeTail = Instantiate(_snakeTail, transform);
        StartCoroutine(WaitByDistance(_snakeTail.gameObject));
    }
    
    IEnumerator WaitByDistance(GameObject snakePart)
    {
        AddSplineToSnakeParts(snakePart);
        Instantiate(snakePart, transform);
        yield return new WaitForSeconds(distanceBetween);
    }
    
    
}
