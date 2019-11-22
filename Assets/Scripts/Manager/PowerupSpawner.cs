using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    public GameObject powerupPrefab;
    public int minimumCollectedFruitToSpawn;
    public AnimationCurve spawnProbabilityCurve;

    private int fruitCounter;
    private GameObject powerupGameObject;
    private Powerup powerup;

    private void Start() {
        powerupGameObject = Instantiate(powerupPrefab, Vector3.zero, Quaternion.identity);
        powerup = powerupGameObject.GetComponent<Powerup>();
        powerupGameObject.SetActive(false);
    }

    public void CheckSpawnConditions(int actualCollectedFruit) {
        if (actualCollectedFruit >= minimumCollectedFruitToSpawn ) {
            fruitCounter++;

            float spawnProbability = spawnProbabilityCurve.Evaluate(fruitCounter);
            float randomValue = Random.Range(1f, 100f);

            if ( randomValue < spawnProbability ) {
                powerupGameObject.SetActive(true);
                powerup.Respawn(false, Powerup.PowerupType.INVINCIBILTY);
                //TODO: adjust powerup position according to current fruit position
                fruitCounter = 0;
            }
        }
    }

    public void CorrectPowerupPosition() {
        powerupGameObject.transform.Rotate(Random.Range(5, 10), 0, Random.Range(5, 10));
        powerup.Respawn(true);
    }

    public Powerup.PowerupType CollectPowerup() {
        return powerup.CollectPowerup();
    }
}
