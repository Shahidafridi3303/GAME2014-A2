using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    private PlayerDetection playerDetection;
    private bool bHasLOS = false;
    private bool bIsInSensingRange = false;
    private GameObject playerGameObject;
    private Rigidbody2D rigidbody2D;
    private float movementTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerDetection = GetComponentInChildren<PlayerDetection>();    
        playerGameObject = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        bHasLOS = playerDetection.GetLOSStatus();
        bIsInSensingRange = playerDetection.GetSensingStatus();
        
        if (bIsInSensingRange && bHasLOS)
        {
            movementTimer += Time.deltaTime;

            if (movementTimer >= 0.15f)
            {
                MoveToPlayer();
                movementTimer = 0.0f;
            }
        }
    }

    private void MoveToPlayer()
    {
        Vector2 directionToPlayer = (playerGameObject.transform.position - this.transform.position).normalized;
        rigidbody2D.AddForce(directionToPlayer, ForceMode2D.Impulse);
    }
}
