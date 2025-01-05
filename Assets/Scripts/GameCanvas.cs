using System;
using UnityEngine;

/// <summary>
/// Manages the game canvas and its interaction with the main camera.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class GameCanvas : MonoBehaviour
{
    private Camera mainCamera = null;
    private Canvas canvas = null;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        mainCamera = Camera.main;
        if (!mainCamera) mainCamera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        canvas.worldCamera = mainCamera;
    }
}
