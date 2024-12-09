using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour, IDamage
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private Transform baseCenterPoint;
    [SerializeField] private Transform frontGroundPoint;
    [SerializeField] private Transform frontObstaclePoint;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int damage = 10;
    
    private bool bIsGrounded;
    private bool bIsOnEdge;
    private bool bIsThereAnyObstacleInFront;

    private PlayerDetection playerDetection;
    private Animator animator;

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    private Slider healthBar;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinAmount;
    [SerializeField] private float scatterRadius;
    private bool isDead = false;
    [SerializeField] private float ScatterForce;
    [SerializeField] private int coinValue;


    // Start is called before the first frame update
    void Start()
    {
        playerDetection = GetComponentInChildren<PlayerDetection>();
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        bIsGrounded = Physics2D.Linecast(baseCenterPoint.position, baseCenterPoint.position + Vector3.down * groundCheckDistance, layerMask);
        bIsOnEdge = Physics2D.Linecast(baseCenterPoint.position, frontGroundPoint.position, layerMask);
        bIsThereAnyObstacleInFront = Physics2D.Linecast(baseCenterPoint.position, frontObstaclePoint.position, layerMask);
        
        if (bIsGrounded && (!bIsOnEdge || bIsThereAnyObstacleInFront))
        {
            ChangeDirection();
        }

        if (this.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f && !playerDetection.GetSensingStatus())
        {
            animator.SetInteger("State", (int)AnimationStates.IDLE);
        }

        if (bIsGrounded && !playerDetection.GetLOSStatus())
        {
            Move();
        }
    }

    private void Move()
    {
        animator.SetInteger("State", (int)AnimationStates.RUN);
        transform.position += Vector3.left * transform.localScale.x * speed;
    }

    private void ChangeDirection()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public int Damage()
    {
        return damage;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(baseCenterPoint.position, baseCenterPoint.position + Vector3.down * groundCheckDistance);
        Debug.DrawLine(baseCenterPoint.position, frontGroundPoint.position);
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
        //GetComponent<Animator>().SetTrigger("die");

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
}
