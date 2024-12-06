using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100; // Maximum health
    [SerializeField] private GameObject healthBar; // The health bar UI
    [SerializeField] private float damageCooldown = 1f; // Time in seconds between damage
    [SerializeField] private float healthBarSmoothSpeed = 2f; // Speed of smooth health bar update
    private int currentHealth;
    private bool canTakeDamage = true; // Cooldown flag

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(1f); // Initialize health bar
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage) return; // Skip if damage is on cooldown

        // Apply damage
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent health going below 0

        // Smoothly update health bar
        float healthNormalized = (float)currentHealth / maxHealth;
        StartCoroutine(UpdateHealthBarSmooth(healthNormalized));

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }

        // Start cooldown
        StartCoroutine(DamageCooldown());
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false; // Prevent further damage
        yield return new WaitForSeconds(damageCooldown); // Wait for cooldown
        canTakeDamage = true; // Allow damage again
    }

    private void Die()
    {
        Debug.Log("Player Died");
        // Trigger death animation
        GetComponent<Animator>().SetTrigger("die");

        // Disable player controls
        GetComponent<PlayerBehaviour>().enabled = false;

        // Add Game Over logic here (e.g., restart level or show game over screen)
    }

    private IEnumerator UpdateHealthBarSmooth(float targetValue)
    {
        float currentValue = healthBar.transform.localScale.x;
        while (Mathf.Abs(currentValue - targetValue) > Mathf.Epsilon)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * healthBarSmoothSpeed);
            healthBar.transform.localScale = new Vector3(currentValue, 1f, 1f);
            yield return null;
        }
    }

    private void UpdateHealthBar(float normalizedValue)
    {
        healthBar.transform.localScale = new Vector3(normalizedValue, 1f, 1f);
    }
}
