﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTailSpawner : MonoBehaviour {
    
    public float tailRepeatFactor = 0.1f;
    public float startLength = 0.8f;
    public float lengthIncreaseFactor = 0.4f, thinSize = 0.5f;
    public Color gameOverColor;
    public Material normalMaterial, invincibiltyMaterial;

    private Snake snake;
    private const float lifespan = 0.1f;
    private List<SnakeTail> snakeTailList;
    private bool thinPowerupEnabled = false, currentTailThin = false, lastTailThin = false;
    private MeshRenderer snakeHeadLowerMeshRenderer, snakeHeadUpperRenderer;
    private Animator snakeThinAnimator;
    private bool invincible = false;
    private Color initialColor;

    public void Init(Snake snake) {
        this.snake = snake;
        snakeTailList = new List<SnakeTail>();
        snakeHeadLowerMeshRenderer = transform.Find("Head").Find("Lower Head").GetComponent<MeshRenderer>();
        snakeHeadUpperRenderer = transform.Find("Head").Find("Upper Head").GetComponent<MeshRenderer>();
        snakeThinAnimator = GetComponent<Animator>();
        initialColor = snakeHeadLowerMeshRenderer.material.color;

        InvokeRepeating("SpawnCollider", tailRepeatFactor, tailRepeatFactor);
        InvokeRepeating("PopTail", startLength, lifespan);
    }

    public void ThinPowerupActive(float duration) {
        StartCoroutine(WaitForThinPowerupDuration(duration));
    }

    public void InvincibilityPowerupActive(float duration) {
        StartCoroutine(WaitForInvincibilityPowerupDuration(duration));
    }

    public void IncreaseSnakeLength() {
        CancelInvoke("PopTail");
        if (currentTailThin) {
            InvokeRepeating("PopTail", lengthIncreaseFactor, lifespan / 2);
        } else {
            InvokeRepeating("PopTail", lengthIncreaseFactor, lifespan);
        }
    }

    public void Stop() {
        snakeThinAnimator.enabled = false;
        StartGameOverAnimation();
        CancelInvoke("SpawnCollider");
        CancelInvoke("PopTail");
        snakeThinAnimator.SetTrigger("ThinOff");
        snake.NotifyPowerupWoreOff(false);
        thinPowerupEnabled = false;
        StopAllCoroutines();
    }

    public void Resume() {
        snakeThinAnimator.enabled = true;
        CancelInvoke("SpawnCollider");
        CancelInvoke("PopTail");
        InvokeRepeating("SpawnCollider", 0, tailRepeatFactor);
        InvokeRepeating("PopTail", 0, lifespan);
        transform.localScale = new Vector3(1f, 1f, 1f);

        snakeHeadLowerMeshRenderer.material.color = initialColor;
        snakeHeadUpperRenderer.material.color = initialColor;
        foreach (SnakeTail snakeTail in snakeTailList) {
            snakeTail.StartGameOverAnimation(initialColor);
        }
    }

    public bool IsThinPowerupEnabled() {
        return thinPowerupEnabled;
    }

    public Transform GetLastTailTransform() {
        return snakeTailList[0].transform;
    }

    private void SpawnCollider() {
        //TODO: could be improved somehow by not using GetComponent
        SnakeTail newSnakeTail = ObjectPool.sharedInstance.GetPooledObject().GetComponent<SnakeTail>();
        newSnakeTail.transform.position = transform.position;
        newSnakeTail.transform.rotation = transform.rotation;
        newSnakeTail.gameObject.SetActive(true);
        newSnakeTail.Init(thinPowerupEnabled, invincible);
        snakeTailList.Add(newSnakeTail);
    }

    private void PopTail() {
        currentTailThin = snakeTailList[0].IsInThinMode();
        snakeTailList[0].gameObject.SetActive(false);
        snakeTailList.RemoveAt(0);

        if (currentTailThin != lastTailThin) {
            if (currentTailThin) {
                CancelInvoke("PopTail");
                InvokeRepeating("PopTail", 0, lifespan / 2);
            } else {
                CancelInvoke("PopTail");
                InvokeRepeating("PopTail", 0, lifespan);
            }
        }

        lastTailThin = currentTailThin;
    }

    private void StartGameOverAnimation() {
        snakeHeadLowerMeshRenderer.material.color = gameOverColor;
        snakeHeadUpperRenderer.material.color = gameOverColor;
        foreach (SnakeTail snakeTail in snakeTailList) {
            snakeTail.StartGameOverAnimation(gameOverColor);
        }
    }

    private IEnumerator WaitForThinPowerupDuration(float duration) {
        thinPowerupEnabled = true;
        snakeThinAnimator.SetTrigger("ThinOn");
        transform.localScale = new Vector3(thinSize, thinSize, thinSize);
        CancelInvoke("SpawnCollider");
        InvokeRepeating("SpawnCollider", 0, tailRepeatFactor / 2);

        yield return new WaitForSeconds(duration);

        CancelInvoke("SpawnCollider");
        InvokeRepeating("SpawnCollider", 0, tailRepeatFactor);
        snakeThinAnimator.SetTrigger("ThinOff");
        thinPowerupEnabled = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
        snake.NotifyPowerupWoreOff(true);
    }

    private IEnumerator WaitForInvincibilityPowerupDuration(float duration) {
        invincible = true;
        snakeHeadUpperRenderer.material = invincibiltyMaterial;
        snakeHeadLowerMeshRenderer.material = invincibiltyMaterial;
        foreach ( SnakeTail snakeTail in snakeTailList ) {
            snakeTail.StartInvincibilityMaterial();
        }

        yield return new WaitForSeconds(duration);

        foreach ( SnakeTail snakeTail in snakeTailList ) {
            snakeTail.StopInvincibilityMaterial();
        }
        snakeHeadLowerMeshRenderer.material = normalMaterial;
        snakeHeadUpperRenderer.material = normalMaterial;
        invincible = false;
    }
}
