using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages the game state, including player lives, score, and respawning.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem explosionParticles;
    [Header("User Interface")]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [Header("Gameplay Values")]
    [SerializeField] private int lives = 3;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private float respawnImmunityTime = 3f;
    [Header("Layer Values")]
    [SerializeField] private string ignoreCollisionsLayer = "IgnoreCollisions";
    [SerializeField] private string defaultPlayerLayer = "Player";
    [Header("Scoring Values")]
    [SerializeField] private float largeAsteroidSize = 2f;
    [SerializeField] private float mediumAsteroidSize = 1.25f;
    [SerializeField] private float smallAsteroidSize = 0.75f;
    [SerializeField] private int largeAsteroidScore = 50;
    [SerializeField] private int mediumAsteroidScore = 100;
    [SerializeField] private int smallAsteroidScore = 150;

    private int score = 0;

    private void Awake()
    {
        if (!player) player = FindObjectOfType<Player>();
    }

    public void AsteroidDestroyed(Asteroid destroyedAsteroid)
    {
        // Play Explosion Particles
        PlayParticles(explosionParticles, destroyedAsteroid.transform.position);
        // Handle Score
        HandleAsteroidScore(destroyedAsteroid);
    }

    public void PlayerDied()
    {
        // Check Lives
        HandleLives();
    }

    private void HandleLives()
    {
        // Check GameOver or Respawn
        lives -= 1;
        // Play Explosion Particles
        PlayParticles(explosionParticles, player.transform.position);
        // Update User Interface
        UpdateText(livesText, "x" + lives.ToString());
        if (lives <= 0)
            Invoke(nameof(GameOver), respawnDelay);
        else
            Invoke(nameof(Respawn), respawnDelay);
    }

    private void HandleAsteroidScore(Asteroid destroyedAsteroid)
    {
        // Increase Score Based on Asteroid Size
        if (destroyedAsteroid.size < smallAsteroidSize)
            score += smallAsteroidScore;
        else if (destroyedAsteroid.size < mediumAsteroidSize)
            score += mediumAsteroidScore;
        else if (destroyedAsteroid.size <= largeAsteroidSize)
            score += largeAsteroidScore;
        // Update User Interface
        UpdateText(scoreText, score.ToString());
    }

    private void Respawn()
    {
        // Position and ReEnable Player
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer(ignoreCollisionsLayer);
        player.gameObject.SetActive(true);
        // Handle Respawn Invulnerability
        Invoke(nameof(EnableCollisions), respawnImmunityTime);
    }

    private void EnableCollisions()
    {
        // Reset Player Physics Layer
        player.gameObject.layer = LayerMask.NameToLayer(defaultPlayerLayer);
    }

    private void GameOver()
    {
        // Reload Scene on Game Over
        SceneManager.LoadScene(0);
    }

    private void PlayParticles(ParticleSystem particles, Vector2 position)
    {
        // Position and Play Particles
        GameObject newParticles = Instantiate(particles.gameObject, position, Quaternion.identity);
        Destroy(newParticles, particles.main.duration);
    }

    // Update Any Text Element With a String Value
    private void UpdateText(TextMeshProUGUI textElement, string value) =>
        textElement.text = value;
}
