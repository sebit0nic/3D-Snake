using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : MonoBehaviour {

    private float safetyspan = 0.1f;
    private SphereCollider thisCollider;
    private SnakeTailSpawner snakeTailSpawner;
    private Vector3 thinPowerupScale;
    private MeshRenderer thisRenderer;

    private void Awake() {
        thisCollider = GetComponent<SphereCollider>();
        snakeTailSpawner = GameObject.Find("Snake").GetComponent<SnakeTailSpawner>();
        thinPowerupScale = new Vector3(0.5f, 0.5f, 0.5f);
        thisRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void OnEnable() {
        thisCollider.enabled = false;
        if (snakeTailSpawner.IsThinPowerupEnabled()) {
            transform.localScale = thinPowerupScale;
        } else {
            transform.localScale = Vector3.one;
        }
        StartCoroutine(WaitSafetyRoutine());
    }

    private void OnDisable() {
        thisCollider.enabled = false;
        StopCoroutine(WaitSafetyRoutine());
    }

    public void StartGameOverAnimation(Color gameOverColor) {
        //thisAnimator.SetTrigger("OnBlink");
        thisRenderer.material.color = gameOverColor;
    }

    public IEnumerator WaitSafetyRoutine() {
        yield return new WaitForSeconds(safetyspan);
        thisCollider.enabled = true;
    }
}
