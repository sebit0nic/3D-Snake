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

    public PlayerPowerupTypes CollectPowerup() {
        PlayerPowerupTypes currentType = powerup.GetCurrentType();
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
        powerupGameObject.transform.position = GameManager.instance.GetLastTailTransform().position;
        powerupGameObject.transform.rotation = GameManager.instance.GetLastTailTransform().rotation;

        powerupGameObject.SetActive(true);
        powerup.Respawn();
        StopAllCoroutines();
        StartCoroutine("WaitForUnspawnDelay");
    }

    private IEnumerator WaitForUnspawnDelay() {
        yield return new WaitForSeconds(unspawnDelay);
        powerupGameObject.SetActive(false);
        StartCoroutine("WaitForSpawnDelay");
    }
}
