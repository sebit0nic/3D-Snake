using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles individual snake tail prefab logic.
/// </summary>
public class SnakeTail : MonoBehaviour {

    public Material normalMaterial, invincibiltyMaterial;

    private const float safetyspan = 0.5f;
    private const float thinSize = 0.5f;
    private SphereCollider thisCollider;
    private Vector3 thinPowerupScale;
    private MeshRenderer thisRenderer;
    private bool thinMode;

    private void Awake() {
        thisCollider = GetComponent<SphereCollider>();
        thinPowerupScale = new Vector3( thinSize, thinSize, thinSize );
        thisRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void Init( bool thinPowerupEnabled, bool invincibilityPowerupEnabled ) {
        thinMode = thinPowerupEnabled;
        thisCollider.enabled = false;

        if( thinMode ) {
            transform.localScale = thinPowerupScale;
        } else {
            transform.localScale = Vector3.one;
        }

        if( invincibilityPowerupEnabled ) {
            thisRenderer.material = invincibiltyMaterial;
        } else {
            thisRenderer.material = normalMaterial;
        }
        StartCoroutine( WaitSafetyRoutine() );
    }

    private void OnDisable() {
        thisCollider.enabled = false;
        StopCoroutine( WaitSafetyRoutine() );
    }

    /// <summary>
    /// Change color of snake tail after game over has happened.
    /// </summary>
    public void StartGameOverAnimation( Color gameOverColor ) {
        thisRenderer.material.color = gameOverColor;
    }

    /// <summary>
    /// Change snake tail material to invincibility material as powerup was collected.
    /// </summary>
    public void StartInvincibilityMaterial() {
        thisRenderer.material = invincibiltyMaterial;
    }

    /// <summary>
    /// Invincibility powerup has worn off so change the material back to normal.
    /// </summary>
    public void StopInvincibilityMaterial() {
        thisRenderer.material = normalMaterial;
    }

    /// <summary>
    /// Check if this snake tail is in thin mode due to the powerup.
    /// </summary>
    public bool IsInThinMode() {
        return thinMode;
    }

    /// <summary>
    /// Wait a few milliseconds to activate the collider so that the head of the snake doesn't
    /// immediately collide with the new tail.
    /// </summary>
    public IEnumerator WaitSafetyRoutine() {
        yield return new WaitForSeconds( safetyspan );
        thisCollider.enabled = true;
    }
}
