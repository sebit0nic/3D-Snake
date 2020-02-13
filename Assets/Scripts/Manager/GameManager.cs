using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    private FruitSpawner fruitSpawner;
    private ScoreManager scoreManager;
    private GuiManager guiManager;
    private PowerupSpawner powerupSpawner;
    private Snake snake;
    private CameraController cameraController;

    private SaveLoadManager saveLoadManager;
    private SavedData savedData;

    private bool paused;

    private void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }

    private void Start() {
        saveLoadManager = GetComponent<SaveLoadManager>();
        savedData = saveLoadManager.LoadData();

        fruitSpawner = GetComponentInChildren<FruitSpawner>();
        scoreManager = GetComponentInChildren<ScoreManager>();
        scoreManager.Init(savedData);
        guiManager = GetComponentInChildren<GuiManager>();
        powerupSpawner = GetComponentInChildren<PowerupSpawner>();
        powerupSpawner.Init(savedData);
        snake = GameObject.Find("Snake").GetComponent<Snake>();
        cameraController = GameObject.Find("Environment").GetComponentInChildren<CameraController>();
    }

    public void PlayerCollectedFruit() {
        fruitSpawner.SpawnNewFruit(false);
        scoreManager.IncreaseScore();
        powerupSpawner.UpdateActualCollectedFruit(scoreManager.GetCurrentScore());
        fruitSpawner.SetMoveFruitTowardsPlayer(false);
        guiManager.FruitCollected();
    }

    public PlayerPowerupTypes PlayerCollectedPowerup() {
        PlayerPowerupTypes collectedType = powerupSpawner.CollectPowerup();
        guiManager.ShowPowerupIcon(collectedType);
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
        fruitSpawner.Stop();
        powerupSpawner.Stop();
        snake.Stop();
        guiManager.HideHUD();
        guiManager.ShowGameOverScreen(scoreManager.GetCurrentScore(), scoreManager.GetTotalScore());
        cameraController.Stop();
        scoreManager.FinalizeScore(savedData);
        saveLoadManager.SaveData(savedData);
    }

    public void SwitchScreen(ScreenType screenType) {
        guiManager.ShowScreenTransition((int) screenType);
    }

    public void GamePaused() {
        paused = !paused;
        if (paused) {
            Time.timeScale = 0;
            guiManager.TogglePauseMenu(true);
        } else {
            Time.timeScale = 1;
            guiManager.TogglePauseMenu(false);
        }
    }

    public Transform GetCurrentSnakePosition() {
        return snake.GetCurrentPosition();
    }

    public Transform GetLastTailTransform() {
        return snake.GetLastTailTransform();
    }

    public float GetPowerupDuration() {
        return powerupSpawner.GetPowerupDuration();
    }

    public SavedData GetSavedData() {
        return savedData;
    }
}
