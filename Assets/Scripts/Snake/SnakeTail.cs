using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : MonoBehaviour {

    private float safetyspan = 0.1f;
    private SphereCollider thisCollider;

    private void Awake() {
        thisCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable() {
        thisCollider.enabled = false;
        StartCoroutine(WaitSafetyRoutine());
    }

    private void OnDisable() {
        thisCollider.enabled = false;
        StopCoroutine(WaitSafetyRoutine());
    }

    public IEnumerator WaitSafetyRoutine() {
        yield return new WaitForSeconds(safetyspan);
        thisCollider.enabled = true;
    }
}
