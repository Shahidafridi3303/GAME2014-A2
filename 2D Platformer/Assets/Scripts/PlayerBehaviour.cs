using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour: MonoBehaviour
{
    [SerializeField] private float horizontalForce;
    [SerializeField] private float verticalForce;
    [SerializeField] private Transform groundingTransformPoint;
    [SerializeField] private float groundingRadius;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float horizontalSpeedLimit;
    [SerializeField] [Range(0, 1.0f)] private float airSpeedFactor;
    [SerializeField] [Range(0, 1.0f)] private float leftJoystickVerticalThreshold; // for joystick jump

    private Rigidbody2D rigidBody2D;
    private bool bIsGrounded;
    private float deathlyFallSpeed = 5.0f;

    private Joystick leftJoystick;
    private Animator animator;

    //private HealthBarController healthBarController;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        //healthBarController = GetComponentInChildren<HealthBarController>();

        if (GameObject.Find("GameUIPanel"))
        {
            leftJoystick = GameObject.Find("Dynamic Joystick").GetComponent<Joystick>();
        }
    }

    void Update()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bIsGrounded = Physics2D.OverlapCircle(groundingTransformPoint.position, groundingRadius, groundLayerMask);
        Move();
        Jump();
        //AnimationStateControl();
    }

    private void AnimationStateControl()
    {
        if (bIsGrounded)
        {
            if (Mathf.Abs(rigidBody2D.velocity.x) > 0.2f)
            {
                animator.SetInteger("state", (int)AnimationStates.RUN);
            }
            else
            {
                animator.SetInteger("state", (int)AnimationStates.IDLE);
            }
        }
        else
        {
            if (Mathf.Abs(rigidBody2D.velocity.y) > deathlyFallSpeed)
            {
                animator.SetInteger("state", (int)AnimationStates.FALL);
            }
            else
            {
                animator.SetInteger("state", (int)AnimationStates.JUMP);
            }
        }
    }

    private void Move()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        if (leftJoystick != null)
        {
            xInput = leftJoystick.Horizontal;
            Debug.Log(leftJoystick.Horizontal + "-" + leftJoystick.Vertical);
        }

        if (xInput != 0.0f)
        {
            Vector2 force = Vector2.right * xInput * horizontalForce;
            if (!bIsGrounded)
            {
                force = new Vector2(force.x * airSpeedFactor, force.y);
            }

            rigidBody2D.AddForce(force);
            GetComponent<SpriteRenderer>().flipX = (force.x <= 0.0f);
            if(Math.Abs(rigidBody2D.velocity.x) > horizontalSpeedLimit) // clamp horizontal velocity
            {
                rigidBody2D.velocity = new Vector2(Mathf.Clamp(rigidBody2D.velocity.x, -horizontalSpeedLimit, horizontalSpeedLimit), rigidBody2D.velocity.y);
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector3 damageTakenDirection = Vector2.up; //transform.position - collision.transform.position;
            rigidBody2D.AddForce(damageTakenDirection * 5, ForceMode2D.Impulse);
            //healthBarController.TakeDamage(collision.GetComponent<IDamage>().Damage());
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundingTransformPoint.position, groundingRadius);
    }
}
