using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    
    public float rendererShowDelay = 0.25f;
    public float duration = 10f;
    public GameObject powerupRenderer;

    private PlayerPowerupTypes currentType;
    private PowerupSpawner powerupSpawner;

    private void Awake() {
        powerupSpawner = GameObject.Find("Game Manager").GetComponentInChildren<PowerupSpawner>();
    }

    public void Respawn() {
        int randomPowerup = Random.Range(0, 3);
        currentType = (PlayerPowerupTypes) randomPowerup;
    }

    public PlayerPowerupTypes GetCurrentType() {
        return currentType;
    }

    public float GetDuration() {
        return duration;
    }
}
