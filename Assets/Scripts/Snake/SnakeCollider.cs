using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollider : MonoBehaviour {

    public SnakeColliderType colliderType;

    private SnakeCollision snakeCollision;
    private SphereCollider sphereCollider;

    private void Start() {
        snakeCollision = GetComponentInParent<SnakeCollision>();
        sphereCollider = GetComponent<SphereCollider>();

        if ( colliderType == SnakeColliderType.MAGNET ) {
            sphereCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        snakeCollision.Collide(other, colliderType);
    }

    public void OnMagnetPowerupStart() {
        if (colliderType == SnakeColliderType.MAGNET) {
            sphereCollider.enabled = true;
        }
    }

    public void OnMagnetPowerupEnd() {
        if ( colliderType == SnakeColliderType.MAGNET ) {
            sphereCollider.enabled = false;
        }
    }

    public SnakeColliderType GetColliderType() {
        return colliderType;
    }
}
