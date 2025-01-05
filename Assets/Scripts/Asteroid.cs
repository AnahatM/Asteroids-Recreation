using UnityEngine;

/// <summary>
/// Controls the behavior of asteroids, including movement and collision handling.
/// </summary>
public class Asteroid : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private Sprite[] sprites;
    [SerializeField] public float minSize = 0.5f;
    [SerializeField] public float maxSize = 2f;
    [SerializeField] private float speed = 50f;
    [SerializeField] private float maxLifetime = 30f;
    [SerializeField] private float spawnPositionOffset = 1f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private AudioManager audioManager;

    public float size;

    private void Awake()
    {
        // Get References
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        SetRandomCharacteristics();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void SetRandomCharacteristics()
    {
        // Randomize Sprite, Rotation, Scale, and Mass
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        transform.localScale = Vector3.one * size;
        rb.mass = size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        // Set Trajectory Based on Direction
        rb.AddForce(direction * speed);
        // Destroy Asteroid After Lifetime
        Destroy(gameObject, maxLifetime);
    }

    private void HandleCollision(Collision2D collision)
    {
        // Handle Collision With Bullets
        if (!collision.gameObject.CompareTag("Bullet")) return;
        // Play Explosion Particles
        gameManager.AsteroidDestroyed(this);
        // Split Asteroid If Large Enough
        if ((size / 2) >= minSize)
        {
            CreateSplit();
            CreateSplit();
        }
        // Play Explosion Sounds
        audioManager.PlayExplosionClip();
        // Destroy Previous Asteroid
        Destroy(gameObject);
    }

    private void CreateSplit()
    {
        // Randomize Half Position with Offset
        Vector2 newPosition = transform.position;
        newPosition += Random.insideUnitCircle * spawnPositionOffset;
        // Instantiate Half
        Asteroid half = Instantiate(this, newPosition, transform.rotation);
        // Set Size of Half
        half.size = size / 2;
        // Set Trajectory in Any Direction
        half.SetTrajectory(Random.insideUnitCircle.normalized * speed);
    }
}
