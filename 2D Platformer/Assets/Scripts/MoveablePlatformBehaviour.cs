using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveablePlatformBehaviour : MonoBehaviour
{
    [SerializeField] private PlatformMovementTypes platformMovementType;
    [SerializeField] private float horizontalSpeed = 5;
    [SerializeField] private float horizontalDistance = 5;
    [SerializeField] private float verticalSpeed = 5;
    [SerializeField] private float verticalDistance = 5;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition;
    [SerializeField] private List<Transform> pathList = new List<Transform>();
    [SerializeField, Range(0f, 0.1f)] private float customPlatformMoveAmountEachFrame;

    private List<Vector2> destinationList = new List<Vector2>();
    private int destinationIndex = 0;
    private float timer;




    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in pathList)
        {
            destinationList.Add(t.position);
        }
        destinationList.Add(transform.position);

        startPosition = transform.position;
        endPosition = destinationList[0];
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void FixedUpdate()
    {
        if (platformMovementType == PlatformMovementTypes.CUSTOM)
        {
            if (timer >= 1)
            {
                timer = 0;
                destinationIndex++;
                if (destinationIndex >= destinationList.Count)
                {
                    destinationIndex = 0;
                }
                startPosition = transform.position;
                endPosition = destinationList[destinationIndex];
            }
            else
            {
                timer += customPlatformMoveAmountEachFrame;
            }
        }
    }

    private void Move()
    {
        switch (platformMovementType)
        {
            case PlatformMovementTypes.HORIZONTAL:
                transform.position = new Vector2(Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPosition.x, transform.position.y);
                break;
            case PlatformMovementTypes.VERTICAL:
                transform.position = new Vector2(transform.position.x, Mathf.PingPong(verticalSpeed * Time.time, verticalDistance) + startPosition.y);
                break;
            case PlatformMovementTypes.DIAGONAL_RIGHT:
                transform.position = new Vector2(Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance) + startPosition.x, 
                                                    Mathf.PingPong(verticalSpeed * Time.time, verticalDistance) + startPosition.y);
                break;
            case PlatformMovementTypes.DIAGONAL_LEFT:
                transform.position = new Vector2(startPosition.x - Mathf.PingPong(horizontalSpeed * Time.time, horizontalDistance),
                                                    Mathf.PingPong(verticalSpeed * Time.time, verticalDistance) + startPosition.y);
                break;
            case PlatformMovementTypes.CUSTOM:
                transform.position = Vector2.Lerp(startPosition, endPosition, timer);
                break;
        }
    }
}
