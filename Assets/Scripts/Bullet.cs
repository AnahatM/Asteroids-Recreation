using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    [Header("Gameplay Values")]
    [SerializeField] float bulletSpeed = 500f;
    [SerializeField] float maxLifetime = 10f;

	Rigidbody2D rb;

    void Awake()
    {
        // Get References
        rb = GetComponent<Rigidbody2D>();    
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleBulletCollision();
    }

    public void Project(Vector2 direction)
    {
        // Add Force to Rigidbody in Direction
        rb.AddForce(direction * bulletSpeed);
        // Destroy Bullet After Lifetime
        Destroy(gameObject, maxLifetime);
    }

    void HandleBulletCollision()
    {
        // Destroy Bullet Upon Collision
        Destroy(gameObject);
    }
}
