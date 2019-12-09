using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTailSpawner : MonoBehaviour {
    
    public float tailRepeatFactor = 0.1f;
    public float startLength = 0.8f;
    public float lengthIncreaseFactor = 0.4f;

    private Snake snake;
    private float lifespan = 0.1f;
    private List<SnakeTail> snakeTailList;
    private bool thinPowerupEnabled = false;

    public void Init(Snake snake) {
        this.snake = snake;
        snakeTailList = new List<SnakeTail>();

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

    public void Stop() {
        StartTailGameOverAnimation();
        CancelInvoke("SpawnCollider");
        CancelInvoke("PopTail");
        StopAllCoroutines();
    }

    public void TailGameOverAnimationDone() {
        snake.NotifySnakeTailGameOverAnimationDone();
    }

    public bool IsThinPowerupEnabled() {
        return thinPowerupEnabled;
    }

    private void SpawnCollider() {
        //TODO: could be improved somehow by not using GetComponent
        SnakeTail newSnakeTail = ObjectPool.sharedInstance.GetPooledObject().GetComponent<SnakeTail>();
        newSnakeTail.transform.position = transform.position;
        newSnakeTail.transform.rotation = transform.rotation;
        newSnakeTail.gameObject.SetActive(true);
        snakeTailList.Add(newSnakeTail);
    }

    private void PopTail() {
        snakeTailList[0].gameObject.SetActive(false);
        snakeTailList.RemoveAt(0);
    }

    private void StartTailGameOverAnimation() {
        foreach (SnakeTail snakeTail in snakeTailList) {
            snakeTail.StartGameOverAnimation();
        }
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
