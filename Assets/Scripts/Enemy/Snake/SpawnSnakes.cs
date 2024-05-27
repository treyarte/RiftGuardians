using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnSnakes : MonoBehaviour
{
    [SerializeField] private float distanceBetween = .4f;
    [SerializeField] private int _snakeLength;
    [SerializeField] public List<GameObject> snakeBody = new List<GameObject>();
    [SerializeField] public SplineComputer _mainSpline;
    [SerializeField] private bool DoPushback = false;
    public bool isSnakeBuilt = false;
    /// <summary>
    /// Dictionary of snake body parts to quickly
    /// access them when known the gameObject instance id
    /// </summary>
    private Dictionary<int, GameObject> snakeBodyDict = new Dictionary<int, GameObject>();
    
    [Header("Snake Prefabs")]
    [SerializeField] private GameObject _snakeHead;
    [SerializeField] private List<GameObject> _snakeBodyList;
    [SerializeField] private GameObject _snakeTail;

    [Header("Snake Events")]
    public Action<bool> IsSpawnFinished;
    public static Action OnSnakeDestroyed;
    
    private float countUp = 0;
    private int _minSnakeLen = 4;
    private int _maxSnakeLen = 9;

    // private void OnEnable()
    // {
    //     EnemyHealth.KillEnemy += OnSnakePartDeath;
    // }
    //
    // private void OnDisable()
    // {
    //     EnemyHealth.KillEnemy -= OnSnakePartDeath;
    // }

    private void Start()
    {
        _snakeLength = Random.Range(_minSnakeLen, _maxSnakeLen);
        GenerateSnake();
    }
    
    private void Update()
    {
        if (snakeBody.Count < _snakeLength && !isSnakeBuilt)
        {
            GenerateSnake();
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

    private void GenerateSnake()
    {
        if (snakeBody.Count == 0)
        {
            CreateBodyPart(_snakeHead);
        }
        
        countUp += Time.deltaTime;
        //If the snake body is one behind the set length we can make a tail
        bool canMakeTail = _snakeLength - snakeBody.Count == 1;
        if (canMakeTail)
        {
            CreateBodyPart(_snakeTail);
            countUp = 0;
            isSnakeBuilt = true;
            IsSpawnFinished?.Invoke(true);
            return;
        }
        
        var rand = Random.Range(0, _snakeBodyList.Count);
        GameObject bodyPart = _snakeBodyList[rand];
        CreateBodyPart(bodyPart);
        countUp = 0;
    }
    void CreateBodyPart(GameObject snakeParPrefab)
    {
       GameObject newSnakePart = Instantiate(snakeParPrefab, transform.position, transform.rotation, transform);
       // SplineFollower splineFollower = newSnakePart.GetComponent<SplineFollower>();
       // if (splineFollower)
       // {
       //     splineFollower.spline = _mainSpline;
       // }
       snakeBodyDict.Add(newSnakePart.GetInstanceID(), newSnakePart);
       snakeBody.Add(newSnakePart);
    }

    void OnSnakePartDeath(GameObject enemyId)
    {
        //TODO use dictionary instead of array
        // var foundIndex = snakeBody.FindIndex(s => s.GetInstanceID() == enemyId);
        // if (foundIndex == -1)
        // {
        //     Debug.Log("Enemy id is not in snake body");
        //     return;
        // }
        //
        // //If it is the tail we do not need to move the position
        // if (foundIndex == snakeBody.Count - 1)
        // {
        //     snakeBody.RemoveAt(foundIndex);
        //     return;
        // }
        //
        // var foundSnakeBody = snakeBody[foundIndex];

        // var snakePart = foundSnakeBody.GetComponent<SnakePart>();
        //
        // if (snakePart.GetIsHead())
        // {
        //     OnSnakeDestroyed?.Invoke();
        //     Destroy(this.gameObject);
        //     //Add animation on all destroyed
        //     return;
        // }
        //
        // var movePercent = foundSnakeBody.GetComponent<SplineFollower>().GetPercent();
        // for (var i = foundIndex - 1; i >= 0; i--)
        // {
        //     var aheadSnakePart = snakeBody[i];
        //     var aheadSplineFollower = aheadSnakePart.GetComponent<SplineFollower>();
        //     var aheadPercent = aheadSplineFollower.GetPercent();
        //     aheadSplineFollower.SetPercent(movePercent);
        //     movePercent = aheadPercent;
        // }
        //
        // snakeBody.RemoveAt(foundIndex);
    }
}
