using System;
using UnityEngine;

/// <summary>
/// Controls the player character, including movement and shooting.
/// </summary>
public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletsParent;
    [SerializeField] private string bulletsParentName = "Bullets";
    [Header("Gameplay Values")]
    [SerializeField] private float thrustSpeed = 1f;
    [SerializeField] private float turnSpeed = 1f;
    [Header("KeyBindings")]
    [SerializeField] private KeyCode thrustKey = KeyCode.W;
    [SerializeField] private KeyCode thrustKeyAlt = KeyCode.UpArrow;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode leftKeyAlt = KeyCode.LeftArrow;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode rightKeyAlt = KeyCode.RightArrow;
    [SerializeField] private KeyCode shootKey = KeyCode.Space;

    private Rigidbody2D rb;
    private GameManager gameManager;
    private AudioManager audioManager;

    private bool thrusting;
    private float turnDirection;

    private void Awake()
    {
        // Get References
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        if (!bulletsParent) bulletsParent = GameObject.Find(bulletsParentName).transform;
    }

    private void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void ProcessInput()
    {
        // Set Thrust and Turn Values
        thrusting = Input.GetKey(thrustKey) || Input.GetKey(thrustKeyAlt);
        if (Input.GetKey(leftKey) || Input.GetKey(leftKeyAlt)) turnDirection = 1f;
        else if (Input.GetKey(rightKey) || Input.GetKey(rightKeyAlt)) turnDirection = -1f;
        else turnDirection = 0f;
        if (Input.GetKeyDown(shootKey) || Input.GetMouseButtonDown(0)) Shoot();
    }

    private void ProcessMovement()
    {
        // Move Rigidbody Based on Input
        if (thrusting)
            rb.AddForce(transform.up * thrustSpeed);
        if (turnDirection != 0f)
            rb.AddTorque(turnDirection * turnSpeed);
    }

    private void Shoot()
    {
        // Instantiate New Bullet
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation, bulletsParent);
        bullet.Project(transform.up);
    }

    private void HandleCollision(Collision2D collision)
    {
        // Detect Collision With Asteroid
        if (!collision.gameObject.CompareTag("Asteroid")) return;
        // Stop Movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        // Play Audio
        audioManager.PlayExplosionClip();
        // Disable Player
        gameObject.SetActive(false);
        // Run Game Manager
        gameManager.PlayerDied();
    }
}
