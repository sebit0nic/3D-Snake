using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollider : MonoBehaviour {

    public SnakeColliderType colliderType;
    public float originalColliderRadius, powerupColliderRadius;

    private SnakeCollision snakeCollision;
    private SphereCollider sphereCollider;

    private void Start() {
        snakeCollision = GetComponentInParent<SnakeCollision>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        snakeCollision.Collide(other, colliderType);
    }

    public void OnMagnetPowerupStart() {
        sphereCollider.radius = powerupColliderRadius;
    }

    public void OnMagnetPowerupEnd() {
        sphereCollider.radius = originalColliderRadius;
    }

    public SnakeColliderType GetColliderType() {
        return colliderType;
    }
}
