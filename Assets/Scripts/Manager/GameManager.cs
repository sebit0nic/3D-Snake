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
    private StyleManager styleManager;
    private PlayStoreManager playStoreManager;
    private AchievementManager achievementManager;
    private AdManager adManager;
    private SoundManager soundManager;
    private SaveLoadManager saveLoadManager;
    private SavedData savedData;

    private bool paused;
    private const string screenshotName = "screenshot.png";
    private const string shareSubject = "Snake Planet";
    private const string shareText1 = "My score at #snakeplanet is ";
    private const string shareText2 = " ! Try to beat that!";

    private void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }

    private void Start() {
        saveLoadManager = GetComponentInChildren<SaveLoadManager>();
        savedData = saveLoadManager.LoadData();

        fruitSpawner = GetComponentInChildren<FruitSpawner>();
        if (saveLoadManager.GetTutorialStatus()) {
            fruitSpawner.Init();
        }
        scoreManager = GetComponentInChildren<ScoreManager>();
        scoreManager.Init(savedData);
        guiManager = GetComponentInChildren<GuiManager>();
        guiManager.Init(saveLoadManager.GetTutorialStatus());
        powerupSpawner = GetComponentInChildren<PowerupSpawner>();
        powerupSpawner.Init(savedData);
        snake = GameObject.Find("Snake").GetComponent<Snake>();
        cameraController = GameObject.Find("Environment").GetComponentInChildren<CameraController>();
        cameraController.Init((CameraStatus)saveLoadManager.GetCameraStatus());
        styleManager = GetComponentInChildren<StyleManager>();
        styleManager.Init(savedData);
        playStoreManager = GetComponentInChildren<PlayStoreManager>();
        playStoreManager.Init();
        achievementManager = GetComponentInChildren<AchievementManager>();
        adManager = GetComponentInChildren<AdManager>();
        soundManager = GetComponentInChildren<SoundManager>();
        soundManager.Init(saveLoadManager);
        soundManager.PlaySound(SoundEffectType.SOUND_SLITHER, false);
        soundManager.PlaySound(SoundEffectType.SOUND_AMBIENCE, false);
    }

    public void TutorialDone() {
        fruitSpawner.Init();
        fruitSpawner.SpawnNewFruit(false);
    }

    public void PlayerCollectedFruit() {
        fruitSpawner.SpawnNewFruit(false);
        scoreManager.IncreaseScore();
        powerupSpawner.UpdateActualCollectedFruit(scoreManager.GetCurrentScore());
        fruitSpawner.SetMoveFruitTowardsPlayer(false);
        guiManager.FruitCollected();
        achievementManager.NotifyCurrentScoreIncreased(scoreManager.GetCurrentScore());
        soundManager.PlaySound(SoundEffectType.SOUND_EAT, true);
    }

    public PlayerPowerupTypes PlayerCollectedPowerup() {
        PlayerPowerupTypes collectedType = powerupSpawner.CollectPowerup();

        soundManager.StopSound(SoundEffectType.SOUND_SLITHER);
        soundManager.PlaySound(SoundEffectType.SOUND_POWERUP_COLLECT, false);
        switch (collectedType) {
            case PlayerPowerupTypes.INVINCIBILTY:
                soundManager.PlaySound(SoundEffectType.SOUND_INVINCIBILITY, false);
                break;
            case PlayerPowerupTypes.MAGNET:
                soundManager.PlaySound(SoundEffectType.SOUND_MAGNET, false);
                break;
            case PlayerPowerupTypes.THIN:
                soundManager.PlaySound(SoundEffectType.SOUND_THIN, false);
                break;
        }

        guiManager.ShowPowerupIcon(collectedType);
        guiManager.SetPowerupDuration(powerupSpawner.GetPowerupDuration());
        return collectedType;
    }

    public void PlayerMagnetTouchedFruit() {
        fruitSpawner.SetMoveFruitTowardsPlayer(true);
    }

    public void PowerupWoreOff(bool resumeSpawning) {
        guiManager.HidePowerupText();

        soundManager.StopSound(SoundEffectType.SOUND_THIN);
        soundManager.StopSound(SoundEffectType.SOUND_MAGNET);
        soundManager.StopSound(SoundEffectType.SOUND_INVINCIBILITY);
        soundManager.PlaySound(SoundEffectType.SOUND_SLITHER, false);
        soundManager.PlaySound(SoundEffectType.SOUND_POWERUP_WORE_OFF, false);

        if (resumeSpawning) {
            powerupSpawner.ResumeSpawning();
        }
        fruitSpawner.SetMoveFruitTowardsPlayer(false);
    }

    public void PlayerTouchedTail() {
        fruitSpawner.Stop();
        powerupSpawner.Stop();
        snake.Stop();
        guiManager.HideHUD();

        soundManager.StopSound(SoundEffectType.SOUND_SLITHER);
        soundManager.PlaySound(SoundEffectType.SOUND_TAIL_EAT, false);

        if (scoreManager.GetCurrentScore() > scoreManager.GetMinRevivalScore() && adManager.IsAdAvailable() && saveLoadManager.GetTutorialStatus()) {
            guiManager.ShowAdScreen();
        } else {
            if (scoreManager.CheckDailyPlayReward(saveLoadManager)) {
                cameraController.Stop();
                scoreManager.ClaimDailyPlayReward(saveLoadManager, savedData);
                scoreManager.FinalizeScore(savedData);
                guiManager.ShowGameOverScreen(soundManager, scoreManager.GetCurrentScore(), scoreManager.GetTotalScore(), scoreManager.IsNewHighscore(), true, savedData.IsSomethingPurchaseable());
            } else {
                cameraController.Stop();
                scoreManager.FinalizeScore(savedData);
                guiManager.ShowGameOverScreen(soundManager, scoreManager.GetCurrentScore(), scoreManager.GetTotalScore(), scoreManager.IsNewHighscore(), false, savedData.IsSomethingPurchaseable());
            }
        }
    }

    public void GameOverScreenShown() {
        saveLoadManager.SetTutorialStatus((int) TutorialStatus.TUTORIAL_DONE);
        saveLoadManager.SaveData(savedData);
        playStoreManager.PostScore(savedData.highscore, savedData.totalScore);
    }

    public void SwitchScreen(ScreenType screenType) {
        guiManager.ShowScreenTransition((int) screenType);
    }

    public void ShareScreen() {
        string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
        ScreenCapture.CaptureScreenshot(screenshotName);

        new NativeShare().AddFile(screenShotPath, null).SetSubject("Snake Planet").SetText(shareText1 + scoreManager.GetCurrentScore() + shareText2).Share();
    }

    public void GamePaused() {
        paused = !paused;
        if (paused) {
            Time.timeScale = 0;
            guiManager.TogglePauseMenu(true);
            soundManager.PlaySound(SoundEffectType.SOUND_PAUSE_START, false);
        } else {
            Time.timeScale = 1;
            guiManager.TogglePauseMenu(false);
            soundManager.PlaySound(SoundEffectType.SOUND_PAUSE_END, false);
        }
    }

    public void GameResumedAfterAd() {
        adManager.SetAdAvailable(false);
        guiManager.HideAdScreen();
        guiManager.ShowHUD();
        fruitSpawner.Resume();
        snake.Resume();
    }

    public void AdSkippedOrFailed() {
        adManager.SetAdAvailable(false);
        PlayerTouchedTail();
        guiManager.HideAdScreen();
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
