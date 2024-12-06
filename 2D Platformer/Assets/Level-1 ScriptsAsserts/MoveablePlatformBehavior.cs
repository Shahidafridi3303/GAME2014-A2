using System.Collections.Generic;
using UnityEngine;

public class MoveablePlatformBehavior : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _distance = 3f;

    private Vector2 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        float movement = Mathf.PingPong(Time.time * _speed, _distance);
        transform.position = new Vector2(_startPosition.x + movement, _startPosition.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}