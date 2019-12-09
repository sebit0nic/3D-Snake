using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

    private SnakeMovement snakeMovement;
    private SnakeCollision snakeCollision;
    private SnakeTailSpawner snakeTailSpawner;

    [Header("Debug Settings")]
    public bool invincible = false;

    private void Start() {
        snakeMovement = GetComponent<SnakeMovement>();
        snakeCollision = GetComponent<SnakeCollision>();
        snakeTailSpawner = GetComponent<SnakeTailSpawner>();

        snakeMovement.Init(this);
        snakeCollision.Init(this);
        snakeTailSpawner.Init(this);
    }

    public void NotifyFruitEaten() {
        GameManager.instance.PlayerCollectedFruit();
        snakeTailSpawner.IncreaseSnakeLength();
    }

    public void NotifyTailTouched() {
        if (!invincible) {
            GameManager.instance.PlayerTouchedTail();
        }
    }

    public void NotifyPowerupCollected() {
        switch ( GameManager.instance.PlayerCollectedPowerup() ) {
            case PowerupType.INVINCIBILTY:
                snakeCollision.InvincibilityPowerupActive(GameManager.instance.GetPowerupDuration());
                break;
            case PowerupType.MAGNET:
                snakeCollision.MagnetPowerupActive(GameManager.instance.GetPowerupDuration());
                break;
            case PowerupType.THIN:
                snakeTailSpawner.ThinPowerupActive(GameManager.instance.GetPowerupDuration());
                break;
        }
    }

    public void NotifyFruitTouchedByMagnet() {
        GameManager.instance.PlayerMagnetTouchedFruit();
    }

    public void NotifyPowerupWoreOff() {
        GameManager.instance.PowerupWoreOff();
    }

    public void NotifySnakeTailGameOverAnimationDone() {
        GameManager.instance.SnakeTailGameOverAnimationEnded();
    }

    public void Stop() {
        snakeMovement.Stop();
        snakeCollision.Stop();
        snakeTailSpawner.Stop();
    }

    public Transform GetCurrentPosition() {
        return snakeMovement.GetCurrentPosition();
    }
}
