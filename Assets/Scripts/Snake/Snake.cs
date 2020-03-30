using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

    private SnakeMovement snakeMovement;
    private SnakeCollision snakeCollision;
    private SnakeTailSpawner snakeTailSpawner;
    private SnakeHatChooser snakeHatChooser;

    [Header("Debug Settings")]
    public bool invincible = false;

    private void Start() {
        snakeMovement = GetComponent<SnakeMovement>();
        snakeCollision = GetComponent<SnakeCollision>();
        snakeTailSpawner = GetComponent<SnakeTailSpawner>();
        snakeHatChooser = GetComponentInChildren<SnakeHatChooser>();

        snakeMovement.Init(this);
        snakeCollision.Init(this);
        snakeTailSpawner.Init(this);
        snakeHatChooser.Init(GameManager.instance.GetSavedData());
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
            case PlayerPowerupTypes.INVINCIBILTY:
                snakeTailSpawner.InvincibilityPowerupActive(GameManager.instance.GetPowerupDuration());
                snakeCollision.InvincibilityPowerupActive(GameManager.instance.GetPowerupDuration());
                break;
            case PlayerPowerupTypes.MAGNET:
                snakeCollision.MagnetPowerupActive(GameManager.instance.GetPowerupDuration());
                break;
            case PlayerPowerupTypes.THIN:
                snakeTailSpawner.ThinPowerupActive(GameManager.instance.GetPowerupDuration());
                break;
        }
    }

    public void NotifyFruitTouchedByMagnet() {
        GameManager.instance.PlayerMagnetTouchedFruit();
    }

    public void NotifyPowerupWoreOff(bool resumeSpawning) {
        GameManager.instance.PowerupWoreOff(resumeSpawning);
    }

    public void Stop() {
        snakeMovement.Stop();
        snakeCollision.Stop();
        snakeTailSpawner.Stop();
    }

    public void Resume() {
        StartCoroutine(WaitAfterResume());
    }

    public Transform GetCurrentPosition() {
        return snakeMovement.GetCurrentPosition();
    }

    public Transform GetLastTailTransform() {
        return snakeTailSpawner.GetLastTailTransform();
    }

    private IEnumerator WaitAfterResume() {
        yield return new WaitForSeconds(0.5f);
        snakeTailSpawner.InvincibilityPowerupActive(3f);
        snakeCollision.InvincibilityPowerupActive(3f);
        snakeMovement.Resume();
        snakeCollision.Resume();
        snakeTailSpawner.Resume();
    }
}
