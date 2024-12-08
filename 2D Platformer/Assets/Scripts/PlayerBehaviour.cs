using System;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float horizontalForce;
    [SerializeField] private float verticalForce;
    [SerializeField] private float horizontalSpeedLimit;
    [SerializeField][Range(0, 1.0f)] private float airSpeedFactor;

    [Header("Grounding Settings")]
    [SerializeField] private Transform groundingTransformPoint;
    [SerializeField] private float groundingRadius;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Joystick Settings")]
    [SerializeField][Range(0, 1.0f)] private float leftJoystickVerticalThreshold; // for joystick jump

    [Header("Animation Settings")]
    private Animator animator;
    private enum AnimationStates { IDLE, RUN, JUMP, FALL }
    //private float deathlyFallSpeed = 5.0f;

    [Header("UI and Joystick")]
    private Joystick leftJoystick;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f; // Range of the attack
    [SerializeField] private int attackDamage = 20; // Damage dealt by the attack
    [SerializeField] private LayerMask enemyLayer; // Layer of enemies to detect
    private bool isAttacking = false;

    private Rigidbody2D rigidBody2D;
    private bool bIsGrounded;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (GameObject.Find("GameUIPanel"))
        {
            leftJoystick = GameObject.Find("Dynamic Joystick").GetComponent<Joystick>();
        }
    }

    void FixedUpdate()
    {
        // Check if player is grounded
        bIsGrounded = Physics2D.OverlapCircle(groundingTransformPoint.position, groundingRadius, groundLayerMask);

        // Handle movement and jumping
        Move();
        Jump();

        // Handle animations
        AnimationStateControl();

        if (Input.GetButtonDown("Fire1") && !isAttacking) // Replace "Fire1" with your input setup
        {
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        // Detect enemies within range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(attackDamage);
        }

        // Reset attack state after animation
        Invoke(nameof(ResetAttack), 0.5f); // Adjust duration to match attack animation
    }

    private void ResetAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void AnimationStateControl()
    {
        float horizontalSpeed = Mathf.Abs(rigidBody2D.velocity.x);
        float verticalSpeed = rigidBody2D.velocity.y;

        // Set Animator parameters
        animator.SetFloat("Speed", horizontalSpeed);
        animator.SetBool("IsGrounded", bIsGrounded);
        animator.SetFloat("VerticalVelocity", verticalSpeed);

        //if (bIsGrounded)
        //{
        //    if (horizontalSpeed > 0.1f)
        //    {
        //        animator.SetInteger("state", (int)AnimationStates.RUN);
        //    }
        //    else
        //    {
        //        animator.SetInteger("state", (int)AnimationStates.IDLE);
        //    }
        //}
        //else
        //{
        //    if (verticalSpeed > 0)
        //    {
        //        animator.SetInteger("state", (int)AnimationStates.JUMP);
        //    }
        //    else
        //    {
        //        animator.SetInteger("state", (int)AnimationStates.FALL);
        //    }
        //}
    }


    private void Move()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        if (leftJoystick != null)
        {
            xInput = leftJoystick.Horizontal;
        }

        if (Math.Abs(xInput) > 0.01f)
        {
            Vector2 force = Vector2.right * xInput * horizontalForce;

            // Adjust speed in the air
            if (!bIsGrounded)
            {
                force = new Vector2(force.x * airSpeedFactor, force.y);
            }

            // Apply force and flip sprite
            rigidBody2D.AddForce(force);
            GetComponent<SpriteRenderer>().flipX = (force.x < 0);

            // Clamp horizontal velocity
            if (Mathf.Abs(rigidBody2D.velocity.x) > horizontalSpeedLimit)
            {
                rigidBody2D.velocity = new Vector2(
                    Mathf.Clamp(rigidBody2D.velocity.x, -horizontalSpeedLimit, horizontalSpeedLimit),
                    rigidBody2D.velocity.y
                );
            }
        }
    }

    private void Jump()
    {
        float jumpPressed = Input.GetAxisRaw("Jump");

        if (leftJoystick != null)
        {
            jumpPressed = leftJoystick.Vertical;
        }

        if (jumpPressed > leftJoystickVerticalThreshold && bIsGrounded)
        {
            rigidBody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
            //SoundManager.instance.PlayPlayerJumpSound();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundingTransformPoint.position, groundingRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector3 damageTakenDirection = Vector2.up;
            rigidBody2D.AddForce(damageTakenDirection * 5, ForceMode2D.Impulse);

            // Apply damage from the enemy
            //collision.GetComponent<Enemy>().TakeDamage(10);
            GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }
}
