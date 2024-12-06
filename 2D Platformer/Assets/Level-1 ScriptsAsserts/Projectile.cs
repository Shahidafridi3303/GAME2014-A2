using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    [SerializeField] private float _speed = 10;
    [SerializeField] private int _damage = 5;

    private Collider2D ownerCollider;

    public void SetOwner(Collider2D owner)
    {
        ownerCollider = owner;
    }

    public int Damage()
    {
        return _damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        // Target the player's center or specific aim point
        PlayerBehaviour player = FindObjectOfType<PlayerBehaviour>();
        if (player != null)
        {
            Vector3 playerAimPoint = player.transform.position + new Vector3(0, 0.5f, 0); // Adjust aim to the middle
            Vector3 directionToTarget = (playerAimPoint - transform.position).normalized;

            // Apply force to the projectile
            _rigidbody.AddForce(directionToTarget * _speed, ForceMode2D.Impulse);
        }

        // Destroy the projectile after a set time
        Invoke(nameof(DestroyBullet), 1.5f);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collision with the owner
        if (collision == ownerCollider) return;

        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(_damage);
            Destroy(gameObject); // Destroy projectile after hitting the player
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the projectile if it hits the ground or environment
        }
    }
}