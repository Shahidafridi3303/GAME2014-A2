using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamage
{
    [SerializeField] private float speed = 0.1f;
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

    // Start is called before the first frame update
    void Start()
    {
        playerDetection = GetComponentInChildren<PlayerDetection>();
        //animator = GetComponent<Animator>();
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
        //animator.SetInteger("State", (int)AnimationStates.IDLE);
        if (bIsGrounded && !playerDetection.GetLOSStatus())
        {
            Move();
        }
    }

    private void Move()
    {
        //animator.SetInteger("State", (int)AnimationStates.RUN);
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

    
}
