using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles every incoming collision for a particular snake collider.
/// </summary>
public class SnakeCollider : MonoBehaviour {

    public SnakeColliderType colliderType;

    private SnakeCollision snakeCollision;
    private Collider sphereCollider;

    private void Start() {
        snakeCollision = GetComponentInParent<SnakeCollision>();
        sphereCollider = GetComponent<Collider>();

        if( colliderType == SnakeColliderType.MAGNET ) {
            sphereCollider.enabled = false;
        }
    }

    private void OnTriggerEnter( Collider other ) {
        snakeCollision.Collide( other, colliderType );
    }

    /// <summary>
    /// If this collider is the MAGNET collider, then notify it that the MAGNET powerup is active.
    /// </summary>
    public void OnMagnetPowerupStart() {
        if( colliderType == SnakeColliderType.MAGNET ) {
            sphereCollider.enabled = true;
        }
    }

    /// <summary>
    /// If this collider is the MAGNET collider, then notify it that the MAGNET powerup has worn off.
    /// </summary>
    public void OnMagnetPowerupEnd() {
        if( colliderType == SnakeColliderType.MAGNET ) {
            sphereCollider.enabled = false;
        }
    }

    public SnakeColliderType GetColliderType() {
        return colliderType;
    }
}
