using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    public GameObject powerupPrefab;
    public int minimumCollectedFruitToSpawn;
    public float minSpawnDelay, maxSpawnDelay, unspawnDelay;
    
    private GameObject powerupGameObject;
    private Powerup powerup;

    private void Start() {
        powerupGameObject = Instantiate(powerupPrefab, Vector3.zero, Quaternion.identity);
        powerup = powerupGameObject.GetComponent<Powerup>();
        powerupGameObject.SetActive(false);
    }

    public void UpdateActualCollectedFruit(int actualCollectedFruit) {
        if (actualCollectedFruit == minimumCollectedFruitToSpawn) {
            ResumeSpawning();
        }
    }

    public void ResumeSpawning() {
        StartCoroutine("WaitForSpawnDelay");
    }

    public void CorrectPowerupPosition() {
        powerupGameObject.transform.Rotate(Random.Range(5, 10), 0, Random.Range(5, 10));
        powerup.Respawn(true);
    }

    public PowerupType CollectPowerup() {
        PowerupType currentType = powerup.GetCurrentType();
        powerupGameObject.SetActive(false);
        StopAllCoroutines();
        return currentType;
    }

    public void Stop() {
        StopAllCoroutines();
        powerupGameObject.SetActive(false);
    }

    public float GetPowerupDuration() {
        return powerup.GetDuration();
    }

    private IEnumerator WaitForSpawnDelay() {
        yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        powerupGameObject.transform.rotation = GameManager.instance.GetCurrentSnakePosition().rotation;
        powerupGameObject.transform.Rotate(180, 0, 0);

        powerupGameObject.SetActive(true);
        powerup.Respawn(false);
        StopAllCoroutines();
        StartCoroutine("WaitForUnspawnDelay");
    }

    private IEnumerator WaitForUnspawnDelay() {
        yield return new WaitForSeconds(unspawnDelay);
        powerupGameObject.SetActive(false);
        StartCoroutine("WaitForSpawnDelay");
    }
}
