using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// makes the player move with the platform instead of "sliding"
public class PlatformBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.SetParent(null);
    }
}
