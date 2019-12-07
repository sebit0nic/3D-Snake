using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    private FruitSpawner fruitSpawner;
    private ScoreManager scoreManager;
    private GuiManager guiManager;
    private PowerupSpawner powerupSpawner;
    private Snake snake;

    private bool paused;

    private void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }

    private void Start() {
        fruitSpawner = GetComponentInChildren<FruitSpawner>();
        scoreManager = GetComponentInChildren<ScoreManager>();
        guiManager = GetComponentInChildren<GuiManager>();
        powerupSpawner = GetComponentInChildren<PowerupSpawner>();
        snake = GameObject.Find("Snake").GetComponent<Snake>();
    }

    public void PlayerCollectedFruit() {
        fruitSpawner.SpawnNewFruit(false);
        scoreManager.IncreaseScore();
        powerupSpawner.UpdateActualCollectedFruit(scoreManager.GetCurrentScore());
        fruitSpawner.SetMoveFruitTowardsPlayer(false);
    }

    public PowerupType PlayerCollectedPowerup() {
        PowerupType collectedType = powerupSpawner.CollectPowerup();
        guiManager.ShowPowerupText(collectedType);
        guiManager.SetPowerupDuration(powerupSpawner.GetPowerupDuration());
        return collectedType;
    }

    public void PlayerMagnetTouchedFruit() {
        fruitSpawner.SetMoveFruitTowardsPlayer(true);
    }

    public void PowerupWoreOff() {
        guiManager.HidePowerupText();
        powerupSpawner.ResumeSpawning();
        fruitSpawner.SetMoveFruitTowardsPlayer(false);
    }

    public void PlayerTouchedTail() {
        Time.timeScale = 0;
        //TODO: implement all stop functions instead of stopping time
        fruitSpawner.Stop();
        powerupSpawner.Stop();
        snake.Stop();
        guiManager.ShowGameOverScreen();
    }

    public void GamePaused() {
        paused = !paused;
        if (paused) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    public void GameRetry() {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public Transform GetCurrentSnakePosition() {
        return snake.GetCurrentPosition();
    }

    public float GetPowerupDuration() {
        return powerupSpawner.GetPowerupDuration();
    }
}
