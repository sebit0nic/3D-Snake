using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

    public GameObject powerupPrefab;
    public int minimumCollectedFruitToSpawn;
    public AnimationCurve spawnProbabilityCurve;

    private int fruitCounter;
    private bool canSpawn = true;

    public void CheckSpawnConditions(int actualCollectedFruit) {
        if (actualCollectedFruit >= minimumCollectedFruitToSpawn ) {
            fruitCounter++;

            float spawnProbability = spawnProbabilityCurve.Evaluate(fruitCounter);
            float randomValue = Random.Range(1f, 100f);

            if ( randomValue < spawnProbability ) {
                Debug.Log("###New powerup spawned; Probability <" + spawnProbability + "> RandomValue <" + randomValue + "> FruitCounter <" + fruitCounter + ">");
                fruitCounter = 0;
            } else {
                Debug.Log("No powerup spawned; Probability <" + spawnProbability + "> RandomValue <" + randomValue + "> FruitCounter <" + fruitCounter + ">");
            }
        }
    }
}
