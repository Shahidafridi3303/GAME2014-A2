using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Coin Drop Settings")]
    [SerializeField] private GameObject coinPrefab; 
    [SerializeField] private int coinValue = 1;
    [SerializeField] private int coinAmount = 5;
    [SerializeField] private float scatterRadius = 1.5f;
    [SerializeField] private int ScatterForce = 25;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float patrolDistance = 3f;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool canShoot = false; // Enable projectile attack for this enemy type

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab; // Projectile to shoot

    [Header("Animations")]
    private Animator animator;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private Slider healthBar;

    private int currentHealth;
    private Transform playerTransform;
    private bool isChasing = false;
    private bool isAttacking = false;
    private float lastAttackTime;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private float leftBound;
    private float rightBound;
    private Vector2 direction = Vector2.left;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        leftBound = transform.position.x - patrolDistance;
        rightBound = transform.position.x + patrolDistance;

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void Update()
    {
        if (isAttacking || isDead) return; // Skip logic when attacking or dead

        // Detect the player within detection radius
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
            if (canShoot)
            {
                ShootAtPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }

        // Handle melee attack
        if (!canShoot && playerTransform && Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            AttackPlayer();
        }
    }

    private void Patrol()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isChasing", false);

        // Determine direction and flip the sprite accordingly
        if (transform.position.x <= leftBound)
        {
            direction = Vector2.right;
        }
        else if (transform.position.x >= rightBound)
        {
            direction = Vector2.left;
        }

        UpdateFacingDirection(direction.x);

        // Move the enemy in the current patrol direction
        rb.velocity = new Vector2(direction.x * patrolSpeed, rb.velocity.y);
    }

    private void ChasePlayer()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isChasing", true);

        if (playerTransform)
        {
            // Calculate direction towards the player
            Vector2 chaseDirection = (playerTransform.position - transform.position).normalized;

            UpdateFacingDirection(chaseDirection.x); // Flip the sprite to face the player

            // Move the enemy towards the player
            rb.velocity = new Vector2(chaseDirection.x * patrolSpeed * 1.5f, rb.velocity.y);
        }
    }

    private void UpdateFacingDirection(float xDirection)
    {
        // Flip the sprite based on movement or player direction
        if (xDirection < 0)
        {
            spriteRenderer.flipX = true; // Face left
        }
        else if (xDirection > 0)
        {
            spriteRenderer.flipX = false; // Face right
        }
    }

    private void ShootAtPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        // Instantiate the projectile at the spawn point
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set the owner of the projectile
        projectile.GetComponent<Projectile>().SetOwner(GetComponent<Collider2D>());

        // Calculate the direction to the player
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Ensure the enemy faces the player when shooting
        UpdateFacingDirection(directionToPlayer.x);
    }


    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        animator.SetBool("isAttacking", true);
        isAttacking = true;
        lastAttackTime = Time.time;

        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        StartCoroutine(EndAttackAfterDuration(1f));
    }

    private IEnumerator EndAttackAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isAttacking = false;
        animator.SetBool("isAttacking", false);
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Enemy Died");
        GetComponent<Animator>().SetTrigger("die");

        // Scatter coins
        ScatterCoins();

        Destroy(gameObject, 0.6f); // Delay destruction to allow death animation to play
    }

    private void ScatterCoins()
    {
        for (int i = 0; i < coinAmount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * scatterRadius;

            GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

            // Apply a random upward and outward force
            Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
            if (coinRb != null)
            {
                Vector2 randomForce = new Vector2(UnityEngine.Random.Range(-ScatterForce, ScatterForce), UnityEngine.Random.Range(ScatterForce, ScatterForce)); // Random force
                coinRb.AddForce(randomForce, ForceMode2D.Impulse);
            }

            coin.GetComponent<Coin>()?.SetValue(coinValue);
        }
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
