//Author: Small Hedge Games
//Updated: 13/06/2024

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SoundManager
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsSO SO;
        private static SoundManager instance = null;
        private AudioSource audioSource;
        private static AudioSource loopingAudioSource;




        // Tracking the last play time for each sound type
        private static Dictionary<SoundType, float> lastPlayedTimes = new Dictionary<SoundType, float>();
        private const float minPlayDelay = 0.1f; // Minimum delay between the same sound type

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                audioSource = GetComponent<AudioSource>();
            }
        }

        public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1)
        {
           

            float currentTime = Time.time;

            // Check if the sound type can be played
            if (Time.timeScale > 0)
            {
                if (lastPlayedTimes.ContainsKey(sound) && currentTime - lastPlayedTimes[sound] < minPlayDelay)
                {
                    // If the time since the last play is too short, skip playing the sound
                    Debug.Log($"Sound {sound} skipped due to min play delay.");
                    return;
                }
                lastPlayedTimes[sound] = currentTime;
            }

            // Update the last played time for this sound type
            lastPlayedTimes[sound] = currentTime;

            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            float pitch = 1.0f; // Default pitch (no change)
            if (soundList.useRandomPitch)
            {
                // Define the scale steps in semitones (using pentatonic as an example)
                int[] pentatonicSemitones = new[] { 0, 2, 4, 7 };
                int randomIndex = UnityEngine.Random.Range(0, pentatonicSemitones.Length);
                int semitones = pentatonicSemitones[randomIndex];

                // Calculate the pitch adjustment
                pitch = Mathf.Pow(1.059463f, semitones); // 1.059463f is the 12th root of 2 (semitone ratio)
            }

            if (source)
            {
                source.outputAudioMixerGroup = soundList.mixer;
                source.clip = randomClip;
                source.volume = volume * soundList.volume;
                source.pitch = pitch; // Apply pitch adjustment
                source.Play();
            }
            else
            {
                instance.audioSource.outputAudioMixerGroup = soundList.mixer;
                instance.audioSource.pitch = pitch; // Apply pitch adjustment
                instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
            }
        }


        private System.Collections.IEnumerator FadeMusic(AudioSource from, AudioSource to, float targetVolume)
        {
            float time = 0;
            float duration = 4.0f; // Crossfade duration

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                from.volume = Mathf.Lerp(targetVolume, 0, t);
                to.volume = Mathf.Lerp(0, targetVolume, t);
                yield return null;
            }

            from.Stop();
        }
        public static void PlayLoopingSound(SoundType sound, float volume = 1)
        {
            if (!instance)
            {
                Debug.LogError("No SoundManager instance found!");
                return;
            }

            if (loopingAudioSource == null)
            {
                loopingAudioSource = instance.gameObject.AddComponent<AudioSource>();
            }

            SoundList soundList = instance.SO.sounds[(int)sound];
            if (soundList.sounds.Length == 0)
            {
                Debug.LogError($"No audio clips assigned for {sound} in SoundsSO!");
                return;
            }

            AudioClip randomClip = soundList.sounds[UnityEngine.Random.Range(0, soundList.sounds.Length)];

            float pitch = 1.0f;
            if (soundList.useRandomPitch)
            {
                int[] pentatonicSemitones = new[] { 0, 2, 4, 7 };
                int randomIndex = UnityEngine.Random.Range(0, pentatonicSemitones.Length);
                int semitones = pentatonicSemitones[randomIndex];
                pitch = Mathf.Pow(1.059463f, semitones);
            }

            loopingAudioSource.clip = randomClip;
            loopingAudioSource.outputAudioMixerGroup = soundList.mixer;
            loopingAudioSource.volume = volume * soundList.volume;
            loopingAudioSource.pitch = pitch;
            loopingAudioSource.loop = true;
            loopingAudioSource.Play();
        }

        // **Smoothly Fade Out and Stop the Looping Sound**
        public static IEnumerator FadeOutLoopingSound(float fadeDuration)
        {
            if (!loopingAudioSource || !loopingAudioSource.isPlaying)
                yield break;

            float startVolume = loopingAudioSource.volume;
            float elapsed = 0;

            while (elapsed < fadeDuration)
            {
                loopingAudioSource.volume = Mathf.Lerp(startVolume, 0, elapsed / fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            loopingAudioSource.Stop();
            loopingAudioSource.volume = startVolume; // Reset volume for future playbacks
        }
        public static void StopLoopingSound()
        {
            if (loopingAudioSource != null && loopingAudioSource.isPlaying)
            {
                loopingAudioSource.Stop();
            }
        }
        public static void PauseLoopingSound()
        {
            if (loopingAudioSource != null && loopingAudioSource.isPlaying)
            {
                loopingAudioSource.Pause();
            }
        }

        public static void ResumeLoopingSound()
        {
            if (loopingAudioSource != null && !loopingAudioSource.isPlaying)
            {
                loopingAudioSource.UnPause();
            }
        }

    }






    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
        public bool useRandomPitch;
    }
}
