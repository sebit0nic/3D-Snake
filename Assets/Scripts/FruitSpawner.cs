using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {

    public GameObject fruitPrefab;
    public float minRandomRotation = 25, maxRandomRotation = 270;

    private GameObject fruitGameobject;
    private Fruit fruit;

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
            //TODO: Add random, smaller deviation to the x/z axis so that it does not always spawn right in front of the player
            switch ( randomDirection ) {
                case 0:
                    fruitGameobject.transform.Rotate(randomRotation, 0, 0);
                    Debug.Log("Rotation gain [x]: " + randomRotation.ToString());
                    break;
                case 1:
                    fruitGameobject.transform.Rotate(0, 0, randomRotation);
                    Debug.Log("Rotation gain [z]: " + randomRotation.ToString());
                    break;
            }
        }

        fruit.Respawn(correction);
    }
}
