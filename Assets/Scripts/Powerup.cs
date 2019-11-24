using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    public float rendererShowDelay = 0.25f;
    public GameObject powerupRenderer;
    public enum PowerupType {INVINCIBILTY, MAGNET, THIN}

    private PowerupType currentType;
    private PowerupSpawner powerupSpawner;

    public void Init(PowerupSpawner powerupSpawner) {
        this.powerupSpawner = powerupSpawner;
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Snake Tail")) {
            GameManager.instance.PowerupSpawnedInPlayer();
        }
    }

    public void Respawn(bool correction) {
        if (!correction) {
            StartCoroutine(WaitForShowDelay());
            int randomPowerup = Random.Range(0, 1);
            currentType = (PowerupType) randomPowerup;
        }
    }

    public PowerupType Collect() {
        return currentType;
    }

    private IEnumerator WaitForShowDelay() {
        powerupRenderer.SetActive(false);
        yield return new WaitForSeconds(rendererShowDelay);
        powerupRenderer.SetActive(true);
    }
}
