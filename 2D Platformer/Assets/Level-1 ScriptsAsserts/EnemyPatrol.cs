using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;              // Movement speed
    [SerializeField] private Transform _pointA;             // Patrol Point A
    [SerializeField] private Transform _pointB;             // Patrol Point B
    [SerializeField] private LayerMask _playerLayer;        // Layer for detecting the player
    [SerializeField] private float _detectionRadius = 5f;   // Detection radius for the player

    private bool _movingToPointB = true;
    private Transform _targetPoint;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _targetPoint = _pointB; // Start moving towards Point B
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Patrol();

        // Check for the player
        Collider2D player = Physics2D.OverlapCircle(transform.position, _detectionRadius, _playerLayer);
        if (player)
        {
            ChasePlayer(player.transform);
        }
    }

    void Patrol()
    {
        // Move towards the target point
        transform.position = Vector2.MoveTowards(transform.position, _targetPoint.position, _speed * Time.deltaTime);

        // Flip the sprite based on direction
        _spriteRenderer.flipX = (_targetPoint.position.x < transform.position.x);

        // Switch target points when the enemy reaches one
        if (Vector2.Distance(transform.position, _targetPoint.position) < 0.1f)
        {
            _targetPoint = (_targetPoint == _pointA) ? _pointB : _pointA;
        }
    }

    void ChasePlayer(Transform player)
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, _speed * Time.deltaTime);
        _spriteRenderer.flipX = (player.position.x < transform.position.x);
    }

    private void OnDrawGizmos()
    {
        // Visualize the detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

        // Visualize patrol points
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_pointA.position, _pointB.position);
    }
}
