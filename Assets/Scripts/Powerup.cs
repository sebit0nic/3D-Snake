using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    public float rendererShowDelay = 0.25f;
    public float duration = 10f;
    public GameObject powerupRenderer;

    private PowerupType currentType;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Snake Tail")) {
            GameManager.instance.PowerupSpawnedInPlayer();
        }
    }

    public void Respawn(bool correction) {
        if (!correction) {
            int randomPowerup = Random.Range(0, 1);
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
