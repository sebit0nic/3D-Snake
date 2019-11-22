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
    }

    public void PlayerCollectedFruit() {
        fruitSpawner.SpawnNewFruit(false);
        scoreManager.IncreaseScore();
        powerupSpawner.CheckSpawnConditions(scoreManager.GetCurrentScore());
    }

    public void FruitSpawnedInPlayer() {
        fruitSpawner.SpawnNewFruit(true);
    }

    public Powerup.PowerupType PlayerCollectedPowerup() {
        return powerupSpawner.CollectPowerup();
    }

    public void PowerupSpawnedInPlayer() {
        powerupSpawner.CorrectPowerupPosition();
    }

    public void PlayerTouchedTail() {
        Time.timeScale = 0;
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
}
