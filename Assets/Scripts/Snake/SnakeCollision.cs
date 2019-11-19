using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollision : MonoBehaviour {

    private Snake snake;
    private SnakeMovement snakeMovement;

    public void Init(Snake snake) {
        this.snake = snake;
        snakeMovement = GetComponent<SnakeMovement>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Fruit")) {
            snake.NotifyFruitEaten();
        }

        if (other.gameObject.tag.Equals("Snake Tail")) {
            snake.NotifyTailTouched();
        }
    }
}
