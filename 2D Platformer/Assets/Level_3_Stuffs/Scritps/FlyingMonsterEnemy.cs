using UnityEngine;

public class FlyingMonsterEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public Vector2 verticalRange = new Vector2(3f, 5f); // Min and max range for vertical movement
    public Vector2 horizontalRange = new Vector2(2f, 4f); // Min and max range for horizontal movement
    private Vector2 startPos;
    private Vector2 targetOffset;
    private float targetChangeCooldown = 2f; // How often the target changes direction
    private float nextChangeTime;

    [Header("Attack Settings")]
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float attackCooldown = 3f;
    private float lastAttackTime;

    [Header("Player Reference")]
    public Transform player; // Reference to the player
    public float detectionRange = 5f;

    [Header("Animator")]
    public Animator animator;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPos = transform.position;
        SetRandomTargetOffset();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Reference the SpriteRenderer
    }

    void Update()
    {
        HandleMovement();
        FlipToFacePlayerOrMovement();
        DetectAndAttack();
        DrawDebugLines();
    }

    private void HandleMovement()
    {
        if (animator != null)
            animator.SetBool("isIdle", true);

        // Calculate the target position based on the random offsets
        Vector2 targetPos = startPos + targetOffset;
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // If close to the target, pick a new random target
        if (Vector2.Distance(transform.position, targetPos) < 0.1f && Time.time >= nextChangeTime)
        {
            SetRandomTargetOffset();
            nextChangeTime = Time.time + targetChangeCooldown;
        }
    }

    private void SetRandomTargetOffset()
    {
        float randomX = Random.Range(horizontalRange.x, horizontalRange.y) * (Random.value > 0.5f ? 1 : -1);
        float randomY = Random.Range(verticalRange.x, verticalRange.y) * (Random.value > 0.5f ? 1 : -1);
        targetOffset = new Vector2(randomX, randomY);
    }

    private void FlipToFacePlayerOrMovement()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            // Flip based on player's position relative to the enemy
            if (player.position.x > transform.position.x && !spriteRenderer.flipX)
                spriteRenderer.flipX = true; // Face right (reverse logic)
            else if (player.position.x < transform.position.x && spriteRenderer.flipX)
                spriteRenderer.flipX = false; // Face left (reverse logic)
        }
        else
        {
            // Flip based on movement direction when the player is out of range
            if (targetOffset.x > 0 && !spriteRenderer.flipX)
                spriteRenderer.flipX = true; // Face right (reverse logic)
            else if (targetOffset.x < 0 && spriteRenderer.flipX)
                spriteRenderer.flipX = false; // Face left (reverse logic)
        }
    }

    private void DetectAndAttack()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange && Time.time - lastAttackTime >= attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void AttackPlayer()
    {
        if (animator != null)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttack", true); // Set isAttack to true
        }

        if (projectilePrefab != null && projectileSpawnPoint != null && player != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            FlyingEnemyProjectile projectileScript = projectile.GetComponent<FlyingEnemyProjectile>();
            if (projectileScript != null)
            {
                Vector2 direction = (player.position - projectileSpawnPoint.position).normalized;
                projectileScript.Initialize(direction);
            }
        }

        // Start coroutine to reset the attack animation after 3 seconds
        StartCoroutine(ResetAttackAfterDelay(3f));
    }

    private System.Collections.IEnumerator ResetAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (animator != null)
        {
            animator.SetBool("isAttack", false); // Reset isAttack to false
            animator.SetBool("isIdle", true); // Set isIdle back to true
        }
    }

    private void DrawDebugLines()
    {
        Debug.DrawLine(new Vector2(startPos.x - horizontalRange.y, transform.position.y),
                       new Vector2(startPos.x + horizontalRange.y, transform.position.y), Color.blue);
        Debug.DrawLine(new Vector2(transform.position.x, startPos.y - verticalRange.y),
                       new Vector2(transform.position.x, startPos.y + verticalRange.y), Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * detectionRange, Color.red);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
