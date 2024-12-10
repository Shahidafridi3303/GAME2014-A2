using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private bool bIsSensing;
    [SerializeField] private bool bHasLOS;
    [SerializeField] PlayerBehaviour player;
    [SerializeField] private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsSensing)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position  + new Vector3(0, 4.0f, 0), layerMask);
            Vector2 playerDirection = player.transform.position - transform.position;
            float playerDirectionValue = (playerDirection.x > 0) ? 1 : -1;
            float enemyLookingDirection = (transform.parent.localScale.x > 0) ? -1 : 1;

            bHasLOS = (hit.collider.name == "Player") && playerDirectionValue == enemyLookingDirection;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bIsSensing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bIsSensing = false;
        }
    }

    private void OnDrawGizmos()
    {
        Color color = (bHasLOS) ? Color.green : Color.red;

        if (bIsSensing)
        {
            Debug.DrawLine(transform.position, player.transform.position + new Vector3(0, 4.0f, 0), color);
        }
    }

    public bool GetLOSStatus()
    {
        return bHasLOS;
    }

    public bool GetSensingStatus()
    {
        return bIsSensing;
    }
}
