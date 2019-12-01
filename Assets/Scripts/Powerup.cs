using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    
    public float rendererShowDelay = 0.25f;
    public float duration = 10f;
    public GameObject powerupRenderer;

    private PowerupType currentType;
    private PowerupSpawner powerupSpawner;

    private void Awake() {
        powerupSpawner = GameObject.Find("Game Manager").GetComponentInChildren<PowerupSpawner>();
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Snake Tail")) {
            powerupSpawner.CorrectPowerupPosition();
        }
    }

    public void Respawn(bool correction) {
        if (!correction) {
            int randomPowerup = Random.Range(1, 2);
            currentType = (PowerupType) randomPowerup;
        }
    }

    public PowerupType GetCurrentType() {
        return currentType;
    }

    public float GetDuration() {
        return duration;
    }
}
