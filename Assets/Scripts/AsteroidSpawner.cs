using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Asteroid asteroidPrefab;
    [SerializeField] Transform asteroidsParent;
    [SerializeField] string asteroidsParentName = "Asteroids";
    [Header("Spawning Values")]
    [SerializeField] float spawnRate = 1f;
    [SerializeField] int spawnAmount = 1;
    [SerializeField] float spawnDistance = 15f;
    [SerializeField] float trajectoryVariance = 15f;

    private void Awake()
    {
        if (!asteroidsParent) asteroidsParent = GameObject.Find(asteroidsParentName).transform;
    }

    void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    void Spawn()
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
