using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all sound effect related logic.
/// </summary>
public class SoundManager : MonoBehaviour {

    public AudioSource[] soundEffects;

    private SoundStatus soundStatus;
    private AudioSource currentAudioSource;
    private const float minRandomPitch = 0.8f;
    private const float maxRandomPitch = 1.2f;

    public void Init( SaveLoadManager saveLoadManager ) {
        soundStatus = (SoundStatus) saveLoadManager.GetSoundStatus();
    }

    /// <summary>
    /// Play a sound effect.
    /// </summary>
    public void PlaySound( SoundEffectType soundEffectType, bool randomPitch ) {
        if( soundStatus == SoundStatus.SOUND_ON ) {
            currentAudioSource = soundEffects[(int) soundEffectType];
            if( randomPitch ) {
                currentAudioSource.pitch = Random.Range( minRandomPitch, maxRandomPitch );
            }
            currentAudioSource.Play();
        }
    }

    /// <summary>
    /// Play a sound effect with a specific pitch.
    /// </summary>
    public void PlaySoundWithPitch( SoundEffectType soundEffectType, float pitch ) {
        if( soundStatus == SoundStatus.SOUND_ON ) {
            currentAudioSource = soundEffects[(int) soundEffectType];
            currentAudioSource.pitch = pitch;
            currentAudioSource.Play();
        }
    }

    /// <summary>
    /// Stop a specific sound effect.
    /// </summary>
    public void StopSound( SoundEffectType soundEffectType ) {
        if( soundStatus == SoundStatus.SOUND_ON ) {
            currentAudioSource = soundEffects[(int) soundEffectType];
            currentAudioSource.Stop();
        }
    }

    /// <summary>
    /// Stop all sound effects (even if not playing at that moment).
    /// </summary>
    public void StopAllSound() {
        foreach( AudioSource audioSource in soundEffects ) {
            if( audioSource != null ) {
                audioSource.Stop();
            }
        }
    }
}
