using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Player player;
    [SerializeField] ParticleSystem explosionParticles;
    [Header("User Interface")]
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [Header("Gameplay Values")]
    [SerializeField] int lives = 3;
    [SerializeField] float respawnDelay = 3f;
    [SerializeField] float respawnImmunityTime = 3f;
    [Header("Layer Values")]
    [SerializeField] string ignoreCollisionsLayer = "IgnoreCollisions";
    [SerializeField] string defaultPlayerLayer = "Player";
    [Header("Scoring Values")]
    [SerializeField] float largeAsteroidSize = 2f;
    [SerializeField] float mediumAsteroidSize = 1.25f;
    [SerializeField] float smallAsteroidSize = 0.75f;
    [SerializeField] int largeAsteroidScore = 50;
    [SerializeField] int mediumAsteroidScore = 100;
    [SerializeField] int smallAsteroidScore = 150;

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

    void HandleLives()
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

    void HandleAsteroidScore(Asteroid destroyedAsteroid)
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

    void Respawn()
    {
        // Position and ReEnable Player
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer(ignoreCollisionsLayer);
        player.gameObject.SetActive(true);
        // Handle Respawn Invulnerability
        Invoke(nameof(EnableCollisions), respawnImmunityTime);
    }

    void EnableCollisions()
    {
        // Reset Player Physics Layer
        player.gameObject.layer = LayerMask.NameToLayer(defaultPlayerLayer);
    }

    void GameOver()
    {
        // Reload Scene on Game Over
        SceneManager.LoadScene(0);
    }

    void PlayParticles(ParticleSystem particles, Vector2 position)
    {
        // Position and Play Particles
        GameObject newParticles = Instantiate(particles.gameObject, position, Quaternion.identity);
        Destroy(newParticles, particles.main.duration);
    }

    // Update Any Text Element With a String Value
    void UpdateText(TextMeshProUGUI textElement, string value) =>
        textElement.text = value;
}
