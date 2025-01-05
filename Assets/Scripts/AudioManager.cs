using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages audio playback for the game.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("Audio Values")]
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private float explosionVolume;

    public void PlayExplosionClip() => PlayClip(explosionClip, explosionVolume);

    private void PlayClip(AudioClip clip, float volume) =>
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
}
