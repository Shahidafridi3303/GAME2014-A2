using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private float patrolSpeed = 2f; // Speed of movement
    [SerializeField] private float patrolDistance = 3f; // Distance to move left and right

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Animations")]
    private Animator animator;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 50; // Maximum health
    [SerializeField] private Slider healthBar;   // Reference to health bar UI3
    private int currentHealth;

    private Transform playerTransform;
    private bool isChasing = false;
    private bool isAttacking = false;
    private float lastAttackTime;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private float leftBound; // Leftmost point for patrol
    private float rightBound; // Rightmost point for patrol
    private Vector2 direction = Vector2.left; // Initial movement direction

    void Start()
    {
        // Initialize components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Set patrol bounds based on starting position
        leftBound = transform.position.x - patrolDistance;
        rightBound = transform.position.x + patrolDistance;

        // Health
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
    void Update()
    {
        if (isAttacking)
        {
            return; // Skip other logic while attacking
        }

        // Check for player detection
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (player)
        {
            isChasing = true;
            playerTransform = player.transform;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Check for attack
        if (playerTransform && Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            AttackPlayer();
        }
    }


    private void Patrol()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isChasing", false);

        // Reverse direction if beyond patrol bounds
        if (transform.position.x <= leftBound)
        {
            direction = Vector2.right; // Move right
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x >= rightBound)
        {
            direction = Vector2.left; // Move left
            spriteRenderer.flipX = true;
        }

        // Move enemy in the current direction
        rb.velocity = new Vector2(direction.x * patrolSpeed, rb.velocity.y);
    }

    private void ChasePlayer()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isChasing", true);

        if (playerTransform)
        {
            Vector2 chaseDirection = (playerTransform.position - transform.position).normalized;
            rb.velocity = new Vector2(chaseDirection.x * patrolSpeed * 1.5f, rb.velocity.y);

            // Flip sprite based on direction
            spriteRenderer.flipX = chaseDirection.x < 0;
        }
    }
    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        animator.SetBool("isAttacking", true); // Start attack animation
        isAttacking = true;
        lastAttackTime = Time.time;

        // Stop movement and freeze Rigidbody during the attack
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        // End attack after the animation finishes
        StartCoroutine(EndAttackAfterDuration(1f)); // Adjust duration to match the attack animation
    }

    private IEnumerator EndAttackAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration); // Wait for the attack animation to finish

        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Unfreeze X-axis movement
        isAttacking = false; // Allow other behaviors
        animator.SetBool("isAttacking", false); // End attack animation
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(leftBound, transform.position.y, 0), new Vector3(rightBound, transform.position.y, 0));
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent negative health

        healthBar.value = currentHealth; // Update the health bar

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died");
        // Trigger death animation
        GetComponent<Animator>().SetTrigger("die");

        // Destroy the enemy after the death animation
        Destroy(gameObject, 1f); // Adjust the delay to match animation duration
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
            TakeDamage(attackDamage);
        }
    }
}
