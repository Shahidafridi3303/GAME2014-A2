using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour, IDamage
{
    [SerializeField] private float bulletForce = 10.0f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float projectileLifetime = 3.0f;
    [SerializeField] private float addedForceCooldown = 1.0f;

    private Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        AddForceToProjectile();
        Invoke("DestroyBullet", projectileLifetime);

        // set the initial facing direction in the direction of the target
        if (FindObjectOfType<PlayerBehaviour>().transform.position.x - transform.position.x < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (FindObjectOfType<PlayerBehaviour>().transform.position.x - transform.position.x > 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Update()
    {
        addedForceCooldown += Time.deltaTime;
        if (addedForceCooldown >= 1.0f)
        {
            AddForceToProjectile();
            addedForceCooldown = 0.0f;
        }

        // projectile will rotate to face the target
        Quaternion rotation = Quaternion.LookRotation(
            FindObjectOfType<PlayerBehaviour>().transform.position - transform.position,
            transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

    private void AddForceToProjectile()
    {
        // reset velocity/forces
        rigidBody2D.totalForce = Vector2.zero;
        rigidBody2D.velocity = Vector2.zero;

        // add force in players direction
        Vector3 directionToTarget =
            (FindObjectOfType<PlayerBehaviour>().transform.position - transform.position).normalized;
        rigidBody2D.AddForce(directionToTarget * bulletForce, ForceMode2D.Impulse);
    }

    public int Damage()
    {
        return damage;
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
