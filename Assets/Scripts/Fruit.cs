using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {

    public float rendererShowDelay = 0.25f;
    public GameObject fruitRenderer;
    
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag.Equals("Snake Tail")) {
            GameManager.instance.FruitSpawnedInPlayer();
        }
    }

    public void Respawn(bool correction) {
        if (!correction) {
            StartCoroutine(WaitForShowDelay());
        }
    }

    private IEnumerator WaitForShowDelay() {
        fruitRenderer.SetActive(false);
        yield return new WaitForSeconds(rendererShowDelay);
        fruitRenderer.SetActive(true);
    }
}
