using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile"))
        {
            Destroy(collision.gameObject);
        }
    }
}
