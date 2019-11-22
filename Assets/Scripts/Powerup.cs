using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    public float rendererShowDelay = 0.25f;
    public GameObject powerupRenderer;
    public enum PowerupType {INVINCIBILTY, MAGNET, THIN}

    private PowerupType currentType;

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Snake Tail")) {
            GameManager.instance.PowerupSpawnedInPlayer();
        }
    }

    public void Respawn(bool correction) {
        if (!correction) {
            StartCoroutine(WaitForShowDelay());
        }
    }

    public void Respawn(bool correction, PowerupType newType) {
        if (!correction) {
            StartCoroutine(WaitForShowDelay());
            currentType = newType;
            //TODO: add timer to unspawn the powerup after some time
        }
    }

    public PowerupType CollectPowerup() {
        //TODO: set gameobject inactive
        return currentType;
    }

    private IEnumerator WaitForShowDelay() {
        powerupRenderer.SetActive(false);
        yield return new WaitForSeconds(rendererShowDelay);
        powerupRenderer.SetActive(true);
    }
}
