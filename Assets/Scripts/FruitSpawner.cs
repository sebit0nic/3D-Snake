using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {

    public GameObject fruitPrefab;

    [Header("Normal Random Rotation")]
    public float minRandomRotation = 70;
    public float maxRandomRotation = 180;

    [Header("Small Random Rotation")]
    public float minRandomRotationSmall = 5;
    public float maxRandomRotationSmall = 50;
    public int increaseDifficultyFrequency = 5;

    private GameObject fruitGameobject;
    private Fruit fruit;
    private int collectedFruit = 0;

    private void Start() {
        Vector3 fruitRotation = new Vector3();
        fruitRotation.Set(Random.Range(minRandomRotation, maxRandomRotation), Random.Range(minRandomRotation, maxRandomRotation), Random.Range(minRandomRotation, maxRandomRotation));
        fruitGameobject = Instantiate(fruitPrefab, Vector3.zero, Quaternion.Euler(fruitRotation));
        fruit = fruitGameobject.GetComponent<Fruit>();
    }

    public void SpawnNewFruit(bool correction) {
        if (correction) {
            fruitGameobject.transform.Rotate(Random.Range(5, 10), 0, Random.Range(5, 10));
        } else {
            int randomDirection = Random.Range(0, 2);
            float randomRotation = Random.Range(minRandomRotation, maxRandomRotation);
            float randomRotationSmall = Random.Range(minRandomRotationSmall, maxRandomRotationSmall);
            switch ( randomDirection ) {
                case 0:
                    fruitGameobject.transform.Rotate(randomRotation, 0, 0);
                    fruitGameobject.transform.Rotate(0, 0, randomRotationSmall);
                    break;
                case 1:
                    fruitGameobject.transform.Rotate(0, 0, randomRotation);
                    fruitGameobject.transform.Rotate(randomRotationSmall, 0, 0);
                    break;
            }

            collectedFruit++;

            if ( collectedFruit % increaseDifficultyFrequency == 0 ) {
                //TODO: increase minRandomRotation and maxRandomRotation until clamped value
            }
        }
        
        fruit.Respawn(correction);
    }
}
