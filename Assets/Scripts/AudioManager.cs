using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Values")]
    [SerializeField] AudioClip explosionClip;
    [SerializeField] float explosionVolume;

    public void PlayExplosionClip() => PlayClip(explosionClip, explosionVolume);

    void PlayClip(AudioClip clip, float volume) => 
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
}
