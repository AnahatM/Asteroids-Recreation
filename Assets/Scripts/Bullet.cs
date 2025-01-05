using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls the behavior of bullets shot by the player.
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("Gameplay Values")]
    [SerializeField] private float bulletSpeed = 500f;
    [SerializeField] private float maxLifetime = 10f;

    private Rigidbody2D rb;

    private void Awake()
    {
        // Get References
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    private void HandleBulletCollision()
    {
        // Destroy Bullet Upon Collision
        Destroy(gameObject);
    }
}
