using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement of the planet prefab on each scene.
/// </summary>
public class Planet : MonoBehaviour {

    public float rotationSpeed = 5f;

    [SerializeField]
    private bool rotating = false;

    private void Update() {
        if( rotating ) {
            transform.RotateAround( Vector3.zero, transform.up, Time.deltaTime * rotationSpeed );
        }
    }

    /// <summary>
    /// Set rotating if the player has triggered a game over.
    /// </summary>
    public void SetRotating( bool value ) {
        rotating = value;
    }
}
