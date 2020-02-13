using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    
    public float rendererShowDelay = 0.25f;
    public float baseDuration = 10f;
    public GameObject powerupRenderer;

    private PlayerPowerupTypes currentType;
    private float duration;
    private const float durationMultiplier = 2.5f;

    public void Respawn(List<PowerupObject> unlockedPowerups) {
        int randomPowerup = Random.Range(0, unlockedPowerups.Count);
        currentType = unlockedPowerups[randomPowerup].GetPowerupType();
        duration = baseDuration + durationMultiplier * unlockedPowerups[randomPowerup].GetCurrentLevel();
    }

    public PlayerPowerupTypes GetCurrentType() {
        return currentType;
    }

    public float GetDuration() {
        return duration;
    }
}
