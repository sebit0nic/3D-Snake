using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public float rotationSpeed = 5f;

    private bool rotating = false;

    private void Update() {
        if (rotating) {
            transform.RotateAround(Vector3.zero, transform.up, Time.deltaTime * rotationSpeed);
        }
    }

    public void SetRotating(bool value) {
        rotating = value;
    }
}
