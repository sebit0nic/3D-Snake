using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {

    public GameObject fruitPrefab;
    public float moveTowardsPlayerSpeed;

    [Header("Normal Random Rotation")]
    public float minRandomRotation;
    public float maxRandomRotation;
    public float minRandomRotationClamp;
    public float maxRandomRotationClamp;

    [Header("Small Random Rotation")]
    public float minRandomRotationSmall;
    public float maxRandomRotationSmall;
    public float minRandomRotationSmallClamp;
    public float maxRandomRotationSmallClamp;

    [Header("Correction Random Rotation")]
    public float minRandomRotationCorrection;
    public float maxRandomRotationCorrection;

    [Header("Difficulty")]
    public int increaseDifficultyFrequency;
    public float increaseDifficultyRate;

    private GameObject fruitGameobject;
    private Fruit fruit;
    private int collectedFruit;
    private bool moveFruitTowardsPlayer = false;

    private void Start() {
        Vector3 fruitRotation = new Vector3();
        fruitRotation.Set(Random.Range(minRandomRotation, maxRandomRotation), Random.Range(minRandomRotation, maxRandomRotation), Random.Range(minRandomRotation, maxRandomRotation));
        fruitGameobject = Instantiate(fruitPrefab, Vector3.zero, Quaternion.Euler(fruitRotation));
        fruit = fruitGameobject.GetComponent<Fruit>();
    }

    private void Update() {
        if (moveFruitTowardsPlayer) {
            fruitGameobject.transform.rotation = Quaternion.Lerp(fruitGameobject.transform.rotation, GameManager.instance.GetCurrentSnakePosition().rotation, Time.deltaTime * moveTowardsPlayerSpeed);
        }
    }

    public void SpawnNewFruit(bool correction) {
        if (correction) {
            fruitGameobject.transform.Rotate(Random.Range(minRandomRotationCorrection, maxRandomRotationCorrection), 0, Random.Range(minRandomRotationCorrection, maxRandomRotationCorrection));
        } else {
            int randomDirection = Random.Range(0, 2);
            float randomRotation = Random.Range(minRandomRotation, maxRandomRotation);
            float randomRotationSmall = Random.Range(minRandomRotationSmall, maxRandomRotationSmall);
            switch ( randomDirection ) {
                case 0:
                    fruitGameobject.transform.Rotate(randomRotation, 0, randomRotationSmall);
                    break;
                case 1:
                    fruitGameobject.transform.Rotate(randomRotationSmall, 0, randomRotation);
                    break;
            }

            collectedFruit++;

            if ( collectedFruit % increaseDifficultyFrequency == 0 ) {
                minRandomRotation += increaseDifficultyRate;
                maxRandomRotation += increaseDifficultyRate;
                minRandomRotationSmall -= increaseDifficultyRate;
                maxRandomRotationSmall += increaseDifficultyRate;

                minRandomRotation = Mathf.Clamp(minRandomRotation, 0, minRandomRotationClamp);
                maxRandomRotation = Mathf.Clamp(maxRandomRotation, 0, maxRandomRotationClamp);
                minRandomRotationSmall = Mathf.Clamp(minRandomRotationSmall, minRandomRotationSmallClamp, 0);
                maxRandomRotationSmall = Mathf.Clamp(maxRandomRotationSmall, 0, maxRandomRotationSmallClamp);
            }
        }
        
        fruit.Respawn(correction);
    }

    public void SetMoveFruitTowardsPlayer(bool value) {
        moveFruitTowardsPlayer = value;
        fruit.SetIgnoreSnakeTailCollision(value);
    }
}
