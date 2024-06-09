using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Dreamteck.Splines.Examples;
using UnityEngine;
using Untils;
using Random = UnityEngine.Random;

public class SnakeEnemy : MonoBehaviour
{
    [SerializeField] private SnakePart _snakeHead;
    [SerializeField] private List<SnakePart> _snakeParts;
    [SerializeField] private SnakePart _snakeTail;
    [SerializeField] private int _maxLength;
    [SerializeField] private float distanceBetween = .4f;
    private Dictionary<int, GameObject> _snakePartsCreated = new Dictionary<int, GameObject>();
    private SplineComputer _enemyPath;
    private SplineFollower _snakeHeadFollower;
    private float _offset = 2.7f;
    private float _distanceFromHead = 1.9f;
    private float _startPos = 0f;
    public DoublyLinkedList<GameObject> _snakePartsOrderedList = new DoublyLinkedList<GameObject>();

    private void OnEnable()
    {
        EnemyHealth.KillEnemy += OnSnakePartDestroyed;
        PathTriggers.DestroyOnCross += OnSnakePartDestroyed;
    }

    private void OnDisable()
    {
        EnemyHealth.KillEnemy -= OnSnakePartDestroyed;
        PathTriggers.DestroyOnCross -= OnSnakePartDestroyed;
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
        // int snakeSize = Random.Range(6, 8); 
        for (int i = 0; i < snakeSize; i++)
        {
            var rand = Random.Range(0, _snakeParts.Count);
            
            SnakePart bodyPart = _snakeParts[rand];
            
            SnakePart newSnakePart = Instantiate(bodyPart, transform);
            
            
            var snakeNode = _snakePartsOrderedList.AddLast(newSnakePart.gameObject);

            newSnakePart.snakeNode = snakeNode;
            
            _snakePartsCreated.Add(newSnakePart.gameObject.GetInstanceID(), newSnakePart.gameObject);
            
            var splinePositioner = newSnakePart.GetComponent<SplinePositioner>();
            if (!splinePositioner)
            {
                throw new Exception($"Spline Positioner doesn't exist for snake part at index: {i}");
            }

            splinePositioner.spline = _enemyPath;
            splinePositioner.followTarget = prevPart;

            if (i == 0)
            {
                splinePositioner.followTargetDistance = _distanceFromHead;
            }
            else
            {
                splinePositioner.followTargetDistance = _offset;
            }
            
            
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
        
        var snakeHeadNode = _snakePartsOrderedList.AddLast(snakeHead.gameObject);

        snakeHead.snakeNode = snakeHeadNode;
        
        _snakeHeadFollower = snakeHead.GetComponent<SplineFollower>();
        
        //TODO set where we want the snake to spawn at
        _snakeHeadFollower.SetPercent(_startPos);

        _snakePartsCreated.Add(snakeHead.gameObject.GetInstanceID(), snakeHead.gameObject);
        
        SplineTracer previousPart = snakeHead.GetComponent<SplineFollower>();

        previousPart = CreateSnakeBody(previousPart);
        
        var tailPositioner = _snakeTail.GetComponent<SplinePositioner>();
        tailPositioner.spline = _enemyPath;
        tailPositioner.followTarget = previousPart;
        tailPositioner.followTargetDistance = _offset;
        
        var prevSnakePart = previousPart.GetComponent<SnakePart>();
        _snakeTail.backObjId = previousPart.gameObject.GetInstanceID();
        
        //Initialize tail
        var snakeTail = Instantiate(_snakeTail, transform);
        var snakeTailNode = _snakePartsOrderedList.AddLast(snakeTail.gameObject);
        snakeTail.snakeNode = snakeTailNode;
        
        prevSnakePart.backObjId = snakeTail.gameObject.GetInstanceID();
        _snakePartsCreated.Add(snakeTail.gameObject.GetInstanceID(), snakeTail.gameObject);
    }

    /// <summary>
    /// Change the follow target of the snake body part
    /// </summary>
    private void ChangeFollowTargets(Node<GameObject> prevNode, Node<GameObject> nextNode)
    {
        var splinePosNextNode = nextNode.Data.GetComponent<SplinePositioner>();

        bool isPrevNodeHead = _snakePartsOrderedList.Head() == prevNode;
        
        if (isPrevNodeHead)
        {
            splinePosNextNode.followTargetDistance = _distanceFromHead;
            splinePosNextNode.followTarget = prevNode.Data.GetComponent<SplineFollower>();
        }
        else
        {
            splinePosNextNode.followTargetDistance = _offset;
            splinePosNextNode.followTarget = prevNode.Data.GetComponent<SplinePositioner>();
        }
    }

    /// <summary>
    /// Swaps the passed in snake node prev and next with each other
    /// </summary>
    /// <param name="snakeNode"></param>
    private void SwapSnakeNodes(Node<GameObject> snakeNode)
    {
        var prevNode = snakeNode.Previous;

        var nextNode = snakeNode.Next;
        
        _snakePartsOrderedList.Remove(snakeNode);

        ChangeFollowTargets(prevNode, nextNode);
    }

    /// <summary>
    /// Pushes the snake head back which in return
    /// pushes the entire snake back
    /// </summary>
    /// <param name="snakeObj"></param>
    private void PushSnakeBack(GameObject snakeObj)
    {
        var snakeHead = _snakePartsOrderedList.Head();
        var distance = snakeObj.GetComponentInChildren<Renderer>().bounds.size.z;
        var snakeHeadFollower = snakeHead.Data.GetComponent<SplineFollower>();
        var percentDifference =
            snakeHeadFollower.GetPercent() - (distance / snakeHeadFollower.spline.CalculateLength());
        
        
        snakeHead.Data.GetComponent<SplineFollower>().SetPercent(percentDifference);
    }

    //TODO onDestroy need to happen on the individual snake part
    private void OnDestroy()
    {
        
    }

    /// <summary>
    /// Handles when a snake part is destroyed
    /// </summary>
    /// <param name="snakeObj"></param>
    private void OnSnakePartDestroyed(GameObject snakeObj)
    {
        var isValid = _snakePartsCreated.TryGetValue(snakeObj.GetInstanceID(), out _);

        if (!isValid)
        {
            return;
        }
        
        //Because other snakes will have this method caled when destroyed we need to check
        var snakePart = snakeObj.GetComponent<SnakePart>();

        if (!snakePart)
        {
            return;
        }
        
        var snakeNode = snakePart.snakeNode;
        PushSnakeBack(snakeObj);

        if (snakeObj == _snakePartsOrderedList.Tail().Data)
        {
            var newTail = snakeNode.Previous;
            if (newTail == _snakePartsOrderedList.Head())
            {
                Destroy(this.gameObject);
                return;
            }
            _snakePartsOrderedList.RemoveLast();
            newTail.Data.GetComponent<SnakePart>().SetIsTail(true);
            return;
        }
        
        // if (snakePart.GetIsTail())
        // {
        //     //If the we only have the tail and head left destroy the entire snake
        //     if (snakeNode.Previous.Data.GetInstanceID() == _snakePartsOrderedList.Head().Data.GetInstanceID())
        //     {
        //         Destroy(this.gameObject);
        //     }
        //
        //     snakeNode.Previous.Data.GetComponent<SnakePart>().SetIsTail(true);
        //     _snakePartsOrderedList.RemoveLast();
        //     return;
        // }
        //
        // if (snakeNode.Next == null && snakeNode.Previous.Data == _snakePartsOrderedList.Head().Data)
        // {
        //         Destroy(this.gameObject);
        //         return;
        // }
        
        SwapSnakeNodes(snakeNode);
    }

    /// <summary>
    /// Destroys the snake part when reaching the end
    /// </summary>
    /// <remarks>
    /// Because of the way we have things setup. When it come to
    /// snakes the head should be the only thing that should reach the end
    /// </remarks>
    private void OnSnakeReachEnd(GameObject snakeObj)
    {
        SnakePart snakePart = snakeObj.GetComponent<SnakePart>();
        if (snakePart.GetIsHead())
        {
            var snakeBody = snakePart.snakeNode.Previous.Data;
            OnSnakePartDestroyed(snakeBody);
            
        }
    }
}
