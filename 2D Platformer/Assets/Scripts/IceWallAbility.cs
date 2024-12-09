using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceWallAbility : MonoBehaviour, IDamage
{
    [SerializeField] private GameObject iceWallGameObject;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float wallLifetime;
    [SerializeField] private float wallDuration;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private int damage;
    [SerializeField] private Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        playerGameObject = GameObject.Find("Player");
        wallLifetime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        wallLifetime += Time.deltaTime;
        if (wallLifetime >= wallDuration || currentHealth <= 0)
        {
            DestroyWall();
        }
    }

    private void DestroyWall()
    {
        Destroy(this.gameObject);
    }

    public int Damage()
    {
        return damage;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            DestroyWall();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(damage);
            this.TakeDamage(collision.gameObject.GetComponent<EnemyBehaviour>().Damage());
        }
    }
}
