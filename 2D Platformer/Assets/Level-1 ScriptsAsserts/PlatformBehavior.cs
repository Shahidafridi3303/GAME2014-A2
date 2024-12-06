using System.Collections;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    public enum PlatformType
    {
        Static,
        Moving,
        Bouncing,
        Collapsing,
        Swinging
    }

    [Header("General Settings")]
    [SerializeField] private PlatformType platformType = PlatformType.Static;

    [Header("Moving Platform Settings")]
    [SerializeField] private Vector2 moveDirection = Vector2.right;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Bouncing Platform Settings")]
    [SerializeField] private float bounceForce = 10f;

    [Header("Collapsing Platform Settings")]
    [SerializeField] private float collapseDelay = 1f;

    [Header("Swinging Platform Settings")]
    [SerializeField] private float swingSpeed = 2f;
    [SerializeField] private float swingAngle = 30f;

    // Internal variables
    private Vector2 startPosition;
    private bool movingForward = true;
    private Rigidbody2D rb;
    private Quaternion startRotation;

    void Start()
    {
        if (platformType == PlatformType.Moving)
        {
            startPosition = transform.position;
        }

        if (platformType == PlatformType.Collapsing)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (platformType == PlatformType.Swinging)
        {
            startRotation = transform.rotation;
        }
    }

    void Update()
    {
        switch (platformType)
        {
            case PlatformType.Moving:
                HandleMovingPlatform();
                break;
            case PlatformType.Swinging:
                HandleSwingingPlatform();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (platformType)
            {
                case PlatformType.Bouncing:
                    HandleBouncingPlatform(collision);
                    break;
                case PlatformType.Collapsing:
                    HandleCollapsingPlatform();
                    break;
            }
        }
    }

    // Moving Platform Logic
    private void HandleMovingPlatform()
    {
        if (movingForward)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            if (Vector2.Distance(startPosition, transform.position) >= moveDistance)
                movingForward = false;
        }
        else
        {
            transform.Translate(-moveDirection * moveSpeed * Time.deltaTime);
            if (Vector2.Distance(startPosition, transform.position) <= 0.1f)
                movingForward = true;
        }
    }

    // Bouncing Platform Logic
    private void HandleBouncingPlatform(Collision2D collision)
    {
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
        }
    }

    // Collapsing Platform Logic
    private void HandleCollapsingPlatform()
    {
        if (rb != null)
        {
            Invoke(nameof(Collapse), collapseDelay);
        }
    }

    private void Collapse()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // Make the platform fall
        Destroy(gameObject, 2f); // Destroy platform after 2 seconds
    }

    // Swinging Platform Logic
    private void HandleSwingingPlatform()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.rotation = startRotation * Quaternion.Euler(0, 0, angle);
    }
}
