using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SounDProximity : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player; // Player transform (usually the Main Camera)

    [Header("Sound Settings")]
    public float maxDistance = 15f;  // Distance where sound is fully faded out
    public float fadeSpeed = 1f;     // How fast the volume changes

    private AudioSource audioSource;
    private float currentVolume = 0f; // Current audio volume

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.Play(); // Start playing (it will be silent until close)
    }

    private void Update()
    {
        if (player != null)
            ControlSoundBasedOnDistance();
    }

    private void ControlSoundBasedOnDistance()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // Calculate target volume (1 when close, 0 when far)
        float targetVolume = Mathf.Clamp01(1 - (distance / maxDistance));

        // Smoothly interpolate toward target volume
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }
}