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
        StartCoroutine(WaitForPowerupDuration(duration));
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Fruit")) {
            snake.NotifyFruitEaten();
        }

        if (other.gameObject.tag.Equals("Snake Tail") && !invincible) {
            snake.NotifyTailTouched();
        }

        if (other.gameObject.tag.Equals("Powerup")) {
            snake.NotifyPowerupCollected();
        }
    }

    private IEnumerator WaitForPowerupDuration(float duration) {
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
        snake.NotifyPowerupWoreOff();
    }
}
