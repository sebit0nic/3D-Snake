using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollision : MonoBehaviour {

    private Snake snake;
    private bool invincible = false;

    public void Init(Snake snake) {
        this.snake = snake;
    }

    public void InvincibilityPowerupActive(float duration) {
        StartCoroutine(WaitForInvincibilityPowerupDuration(duration));
    }

    public void Collide(Collider other, SnakeColliderType snakeColliderType) {
        switch ( snakeColliderType ) {
            case SnakeColliderType.COLLECTIBLE:
                if ( other.gameObject.tag.Equals("Fruit") ) {
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
}
