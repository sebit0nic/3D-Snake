using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    public GameObject powerupPrefab;
    public int minimumCollectedFruitToSpawn;
    public float unspawnDelay;
    public AnimationCurve spawnProbabilityCurve;

    private int fruitCounter;
    private bool canSpawn = true;
    private GameObject powerupGameObject;
    private Powerup powerup;

    [Header("DEBUG")]
    public bool debugSpawnerActive = false;

    private void Start() {
        powerupGameObject = Instantiate(powerupPrefab, Vector3.zero, Quaternion.identity);
        powerup = powerupGameObject.GetComponent<Powerup>();
        powerup.Init(this);
        powerupGameObject.SetActive(false);
    }

    public void CheckSpawnConditions(int actualCollectedFruit, Transform currentFruitPosition) {
        if (!debugSpawnerActive) {
            if ( actualCollectedFruit >= minimumCollectedFruitToSpawn && canSpawn ) {
                fruitCounter++;

                float spawnProbability = spawnProbabilityCurve.Evaluate(fruitCounter);
                float randomValue = Random.Range(1f, 100f);

                if ( randomValue < spawnProbability ) {
                    SpawnNewPowerup(currentFruitPosition);
                }
            }
        } else {
            SpawnNewPowerup(currentFruitPosition);
        }
    }

    public void CorrectPowerupPosition() {
        powerupGameObject.transform.Rotate(Random.Range(5, 10), 0, Random.Range(5, 10));
        powerup.Respawn(true);
    }

    public Powerup.PowerupType CollectPowerup() {
        Powerup.PowerupType currentType = powerup.Collect();
        powerupGameObject.SetActive(false);
        canSpawn = true;
        return powerup.Collect();
    }

    private void SpawnNewPowerup(Transform currentFruitPosition) {
        powerupGameObject.transform.rotation = currentFruitPosition.transform.rotation;
        int randomDirection = Random.Range(0, 2);
        if ( randomDirection == 0 ) {
            powerupGameObject.transform.Rotate(0, 0, Random.Range(-90, -20));
        } else {
            powerupGameObject.transform.Rotate(0, 0, Random.Range(20, 90));
        }

        powerupGameObject.SetActive(true);
        powerup.Respawn(false);
        fruitCounter = 0;
        canSpawn = false;
    }

    private IEnumerator WaitForUnspawnDelay() {
        yield return new WaitForSeconds(unspawnDelay);
        powerupGameObject.SetActive(false);
        canSpawn = true;
    }
}
