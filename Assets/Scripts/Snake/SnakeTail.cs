using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : MonoBehaviour {

    private const float safetyspan = 0.1f;
    private const float thinSize = 0.5f;
    private SphereCollider thisCollider;
    private Vector3 thinPowerupScale;
    private MeshRenderer thisRenderer;
    private bool thinMode;

    private void Awake() {
        thisCollider = GetComponent<SphereCollider>();
        thinPowerupScale = new Vector3(thinSize, thinSize, thinSize);
        thisRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void Init(bool thinPowerupEnabled) {
        thinMode = thinPowerupEnabled;
        thisCollider.enabled = false;

        if (thinMode) {
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
        thisRenderer.material.color = gameOverColor;
    }

    public bool IsInThinMode() {
        return thinMode;
    }

    public IEnumerator WaitSafetyRoutine() {
        yield return new WaitForSeconds(safetyspan);
        thisCollider.enabled = true;
    }
}
