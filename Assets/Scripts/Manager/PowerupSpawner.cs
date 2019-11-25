using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    public GameObject powerupPrefab;
    public int minimumCollectedFruitToSpawn;
    public float minSpawnDelay, maxSpawnDelay, unspawnDelay;
    public AnimationCurve spawnProbabilityCurve;
    
    private GameObject powerupGameObject;
    private Powerup powerup;
    private Transform currentSnakePosition;

    private void Start() {
        powerupGameObject = Instantiate(powerupPrefab, Vector3.zero, Quaternion.identity);
        powerup = powerupGameObject.GetComponent<Powerup>();
        powerup.Init(this);
        powerupGameObject.SetActive(false);
    }

    public void UpdateSpawnConditions(int actualCollectedFruit, Transform currentSnakePosition) {
        if (actualCollectedFruit == minimumCollectedFruitToSpawn) {
            StartCoroutine(WaitForSpawnDelay());
        }

        this.currentSnakePosition = currentSnakePosition;
    }

    public void CorrectPowerupPosition() {
        powerupGameObject.transform.Rotate(Random.Range(5, 10), 0, Random.Range(5, 10));
        powerup.Respawn(true);
    }

    public PowerupType CollectPowerup() {
        PowerupType currentType = powerup.GetCurrentType();
        powerupGameObject.SetActive(false);
        StartCoroutine(WaitForSpawnDelay());
        return currentType;
    }

    private void SpawnNewPowerup() {
        powerupGameObject.transform.rotation = currentSnakePosition.transform.rotation;
        powerupGameObject.transform.Rotate(180, 0, 0);

        powerupGameObject.SetActive(true);
        powerup.Respawn(false);
        StartCoroutine(WaitForUnspawnDelay());
    }

    private IEnumerator WaitForSpawnDelay() {
        yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        Debug.Log("Spawned at: " + Time.time);
        SpawnNewPowerup();
    }

    private IEnumerator WaitForUnspawnDelay() {
        //TODO: if powerup is collected before unspawned automatically, the next time the powerup shows up
        //a shorter amount of time
        yield return new WaitForSeconds(unspawnDelay);
        Debug.Log("Unspawned at: " + Time.time);
        powerupGameObject.SetActive(false);
        StartCoroutine(WaitForSpawnDelay());
    }
}
