using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour, IDamage
{
    [SerializeField] private float bulletForce = 10.0f;
    [SerializeField] private int damage = 5;

    private Rigidbody2D rigidBody2D;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        Vector3 directionToTarget = (FindObjectOfType<PlayerBehaviour>().transform.position - transform.position).normalized;
        rigidBody2D.AddForce(directionToTarget * bulletForce, ForceMode2D.Impulse);
        Invoke("DestroyBullet", 2.0f);
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
