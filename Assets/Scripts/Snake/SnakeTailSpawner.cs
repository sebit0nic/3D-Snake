using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTailSpawner : MonoBehaviour {
    
    public float tailRepeatFactor = 0.1f;
    public float startLength = 0.8f;
    public float lengthIncreaseFactor = 0.4f;

    private Snake snake;
    private float lifespan = 0.1f;
    private List<GameObject> snakeTailList;
    private bool thinPowerupEnabled = false;

    public void Init(Snake snake) {
        this.snake = snake;
        snakeTailList = new List<GameObject>();

        InvokeRepeating("SpawnCollider", tailRepeatFactor, tailRepeatFactor);
        InvokeRepeating("PopTail", startLength, lifespan);
    }

    public void ThinPowerupActive(float duration) {
        StartCoroutine(WaitForThinPowerupDuration(duration));
    }

    public void IncreaseSnakeLength() {
        CancelInvoke("PopTail");
        if (thinPowerupEnabled) {
            InvokeRepeating("PopTail", lengthIncreaseFactor, lifespan / 2);
        } else {
            InvokeRepeating("PopTail", lengthIncreaseFactor, lifespan);
        }
    }

    public bool IsThinPowerupEnabled() {
        return thinPowerupEnabled;
    }

    private void SpawnCollider() {
        GameObject newSnakeTail = ObjectPool.sharedInstance.GetPooledObject();
        newSnakeTail.transform.position = transform.position;
        newSnakeTail.transform.rotation = transform.rotation;
        newSnakeTail.SetActive(true);
        snakeTailList.Add(newSnakeTail);
    }

    private void PopTail() {
        snakeTailList[0].SetActive(false);
        snakeTailList.RemoveAt(0);
    }

    private IEnumerator WaitForThinPowerupDuration(float duration) {
        thinPowerupEnabled = true;
        CancelInvoke("SpawnCollider");
        CancelInvoke("PopTail");
        InvokeRepeating("SpawnCollider", 0, tailRepeatFactor / 2);
        InvokeRepeating("PopTail", 0, lifespan / 2);

        yield return new WaitForSeconds(duration);

        CancelInvoke("SpawnCollider");
        CancelInvoke("PopTail");
        InvokeRepeating("SpawnCollider", 0, tailRepeatFactor);
        InvokeRepeating("PopTail", 0, lifespan);
        thinPowerupEnabled = false;
        snake.NotifyPowerupWoreOff();
    }
}
