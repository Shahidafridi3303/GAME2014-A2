using System.Collections;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Hazard Settings")]
    [SerializeField] private int damage = 10; // Damage dealt by the hazard
    [SerializeField] private float damageInterval = 2f; // Interval between damage in seconds

    private bool isPlayerInHazard = false; // Tracks if the player is within the hazard
    private Coroutine damageCoroutine; // Reference to the coroutine handling damage

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInHazard = true;

            // Start dealing damage to the player
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayer(collision.GetComponent<PlayerHealth>()));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInHazard = false;

            // Stop dealing damage when the player leaves the hazard
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DamagePlayer(PlayerHealth playerHealth)
    {
        while (isPlayerInHazard && playerHealth != null)
        {
            playerHealth.TakeDamage(damage); // Deal damage to the player
            yield return new WaitForSeconds(damageInterval); // Wait for the next interval
        }
    }
}