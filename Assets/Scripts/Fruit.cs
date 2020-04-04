using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {

    public float rendererShowDelay = 0.25f;
    public GameObject fruitRenderer, indicatorRenderer;

    private FruitSpawner fruitSpawner;
    private bool ignoreSnakeTailCollision = false;

    private void Awake() {
        fruitSpawner = GameObject.Find("Game Manager").GetComponentInChildren<FruitSpawner>();
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Snake Tail") && !ignoreSnakeTailCollision) {
            fruitSpawner.SpawnNewFruit(true);
        }
    }

    public void Respawn(bool correction) {
        if (!correction) {
            StartCoroutine(WaitForShowDelay());
        }
    }

    private IEnumerator WaitForShowDelay() {
        fruitRenderer.SetActive(false);
        indicatorRenderer.SetActive(false);
        yield return new WaitForSeconds(rendererShowDelay);
        fruitRenderer.SetActive(true);
        indicatorRenderer.SetActive(true);
    }
    
    public void SetIgnoreSnakeTailCollision(bool value) {
        ignoreSnakeTailCollision = value;
    } 
}
