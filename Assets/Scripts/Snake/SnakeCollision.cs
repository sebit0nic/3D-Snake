using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollision : MonoBehaviour {

    private SnakeCollider collectibleCollider;
    private Snake snake;
    private bool invincible = false;

    public void Init(Snake snake) {
        this.snake = snake;

        SnakeCollider[] colliderList = GetComponentsInChildren<SnakeCollider>();
        foreach(SnakeCollider col in colliderList) {
            if (col.GetColliderType() == SnakeColliderType.COLLECTIBLE) {
                collectibleCollider = col;
                break;
            }
        }
    }

    public void InvincibilityPowerupActive(float duration) {
        StartCoroutine(WaitForInvincibilityPowerupDuration(duration));
    }

    public void MagnetPowerupActive(float duration) {
        StartCoroutine(WaitForMagnetPowerupDuration(duration));
    }

    public void Collide(Collider other, SnakeColliderType snakeColliderType) {
        switch ( snakeColliderType ) {
            case SnakeColliderType.COLLECTIBLE:
                if ( other.gameObject.tag.Equals("Fruit") ) {
                    //TODO: let fruit move gradually towards player, not immediately collected
                    snake.NotifyFruitEaten();
                }
                if ( other.gameObject.tag.Equals("Powerup") ) {
                    snake.NotifyPowerupCollected();
                }
                break;
            case SnakeColliderType.TAIL:
                if ( other.gameObject.tag.Equals("Snake Tail") && !invincible ) {
                    snake.NotifyTailTouched();
                }
                break;
        }
    }

    private IEnumerator WaitForInvincibilityPowerupDuration(float duration) {
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
        snake.NotifyPowerupWoreOff();
    }

    private IEnumerator WaitForMagnetPowerupDuration(float duration) {
        collectibleCollider.OnMagnetPowerupStart();
        yield return new WaitForSeconds(duration);
        collectibleCollider.OnMagnetPowerupEnd();
        snake.NotifyPowerupWoreOff();
    }
}
