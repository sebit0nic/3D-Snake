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

    public void Init(Snake snake) {
        this.snake = snake;
        snakeTailList = new List<GameObject>();

        InvokeRepeating("SpawnCollider", tailRepeatFactor, tailRepeatFactor);
        InvokeRepeating("PopTail", startLength, lifespan);
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

    public void IncreaseSnakeLength() {
        CancelInvoke("PopTail");
        InvokeRepeating("PopTail", lengthIncreaseFactor, lifespan);
    }
}
