using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns asteroids at random positions and trajectories.
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private Transform asteroidsParent;
    [SerializeField] private string asteroidsParentName = "Asteroids";
    [Header("Spawning Values")]
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private int spawnAmount = 1;
    [SerializeField] private float spawnDistance = 15f;
    [SerializeField] private float trajectoryVariance = 15f;

    private void Awake()
    {
        if (!asteroidsParent) asteroidsParent = GameObject.Find(asteroidsParentName).transform;
    }

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        // Repeat For SpawnAmount
        for (int i = 0; i < spawnAmount; i++)
        {
            // Randomize Rotation, Trajectory, and Position
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPoint = transform.position + spawnDirection;
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion spawnRotation = Quaternion.AngleAxis(variance, Vector3.forward);
            // Instantiate New Asteroid Based on Random Values
            Asteroid asteroid = Instantiate(
                asteroidPrefab,
                spawnPoint,
                spawnRotation,
                asteroidsParent);
            // Set Asteroid Size and Trajectory
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(spawnRotation * -spawnDirection);
        }
    }
}
