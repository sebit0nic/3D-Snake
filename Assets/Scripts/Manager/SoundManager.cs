using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all sound effect related logic.
/// </summary>
public class SoundManager : MonoBehaviour {

    public AudioSource[] soundEffects;
    public AudioSource[] musicLoop;

    private SoundStatus soundStatus;
    private AudioSource currentAudioSource;
    private int currentMusicLoop = 0;
    private const int musicLoopIncreaseFactor = 10;
    private const float minRandomPitch = 0.8f;
    private const float maxRandomPitch = 1.2f;
    private const float musicLoopVolume = 0.5f;

    public void Init( SaveLoadManager saveLoadManager ) {
        soundStatus = (SoundStatus) saveLoadManager.GetSoundStatus();

        if( soundStatus == SoundStatus.SOUND_OFF ) {
            foreach( AudioSource audioSource in musicLoop ) {
                if( audioSource != null ) {
                    audioSource.Stop();
                }
            }
        }
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
    /// Resume music loop (after watching ad for example).
    /// </summary>
    public void ResumeMusicLoop() {
        if( soundStatus == SoundStatus.SOUND_ON ) {
            foreach ( AudioSource audioSource in musicLoop ) {
                if ( audioSource != null ) {
                    audioSource.Play();
                }
            }
        }
    }

    /// <summary>
    /// Change music loop according to how many points the player has.
    /// </summary>
    public void CheckMusicLoopLevelIncrease( int currentScore ) {
        if( currentScore % musicLoopIncreaseFactor ==  0 && currentMusicLoop < musicLoop.Length - 1 ) {
            musicLoop[currentMusicLoop].volume = 0f;
            currentMusicLoop++;
            musicLoop[currentMusicLoop].volume = musicLoopVolume;
        }
    }

    /// <summary>
    /// Stop all music loops (on game over for example).
    /// </summary>
    public void StopMusicLoop() {
        foreach( AudioSource audioSource in musicLoop ) {
            if( audioSource != null ) {
                audioSource.Stop();
            }
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
    /// Stop all sound effects and music loops (even if not playing at that moment).
    /// </summary>
    public void StopAllSound() {
        foreach( AudioSource audioSource in soundEffects ) {
            if( audioSource != null ) {
                audioSource.Stop();
            }
        }

        foreach( AudioSource audioSource in musicLoop ) {
            if( audioSource != null ) {
                audioSource.Stop();
            }
        }
    }
}
