using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollider : MonoBehaviour {

    public SnakeColliderType snakeColliderType;

    private SnakeCollision snakeCollision;

    private void Start() {
        snakeCollision = GetComponentInParent<SnakeCollision>();
    }

    private void OnTriggerEnter(Collider other) {
        snakeCollision.Collide(other, snakeColliderType);
    }
}
