using UnityEngine;

public class FlyingEnemyProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    private Vector2 moveDirection;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move in the initialized direction
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Add logic for player hit reduce health
            //Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
