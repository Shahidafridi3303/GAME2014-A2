using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100; // Maximum health
    [SerializeField] private GameObject healthBar; // The health bar UI
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(1f); // Initialize health bar
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent health going below 0

        float healthNormalized = (float)currentHealth / maxHealth;
        StartCoroutine(UpdateHealthBarSmooth(healthNormalized)); // Smoothly update health bar

        if (currentHealth <= 0)
        {
            Die();
        }
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

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        float healthNormalized = (float)currentHealth / maxHealth;
        StartCoroutine(UpdateHealthBarSmooth(healthNormalized)); // Smoothly update health bar
    }

    private void UpdateHealthBar(float normalizedValue)
    {
        healthBar.transform.localScale = new Vector3(normalizedValue, 1f, 1f);
    }

    private IEnumerator UpdateHealthBarSmooth(float targetValue)
    {
        float currentValue = healthBar.transform.localScale.x;
        float changeAmount = currentValue - targetValue;

        while (Mathf.Abs(currentValue - targetValue) > Mathf.Epsilon)
        {
            currentValue -= changeAmount * Time.deltaTime;
            healthBar.transform.localScale = new Vector3(currentValue, 1f, 1f);
            yield return null;
        }

        healthBar.transform.localScale = new Vector3(targetValue, 1f, 1f);
    }
}
