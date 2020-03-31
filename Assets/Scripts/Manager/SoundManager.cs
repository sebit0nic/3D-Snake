using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource[] soundEffects;

    private SoundStatus soundStatus;
    private AudioSource currentAudioSource;

    public void Init(SaveLoadManager saveLoadManager) {
        soundStatus = (SoundStatus) saveLoadManager.GetSoundStatus();
    }

    public void PlaySound(SoundEffectType soundEffectType, bool randomPitch) {
        if (soundStatus == SoundStatus.SOUND_ON) {
            currentAudioSource = soundEffects[(int) soundEffectType];
            if (randomPitch) {
                currentAudioSource.pitch = Random.Range(0.8f, 1.2f);
            }
            currentAudioSource.Play();
        }
    }

    public void StopSound(SoundEffectType soundEffectType) {
        if (soundStatus == SoundStatus.SOUND_ON) {
            currentAudioSource = soundEffects[(int) soundEffectType];
            currentAudioSource.Stop();
        }
    }
}
