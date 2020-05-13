using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles communication between different managers in the game scene.
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Snake snake;
    public CameraController cameraController;

    private FruitSpawner fruitSpawner;
    private ScoreManager scoreManager;
    private GuiManager guiManager;
    private PowerupSpawner powerupSpawner;
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
        if( instance == null ) {
            instance = this;
        }
    }

    private void Start() {
        Application.targetFrameRate = 60;
        saveLoadManager = GetComponentInChildren<SaveLoadManager>();
        savedData = saveLoadManager.LoadData();

        fruitSpawner = GetComponentInChildren<FruitSpawner>();
        if( saveLoadManager.GetTutorialStatus() ) {
            fruitSpawner.Init();
        }
        scoreManager = GetComponentInChildren<ScoreManager>();
        scoreManager.Init( savedData );
        guiManager = GetComponentInChildren<GuiManager>();
        guiManager.Init( saveLoadManager );
        powerupSpawner = GetComponentInChildren<PowerupSpawner>();
        powerupSpawner.Init( savedData );
        cameraController.Init( (CameraStatus) saveLoadManager.GetCameraStatus(), saveLoadManager.GetScreenOrientationStatus() );
        styleManager = GetComponentInChildren<StyleManager>();
        styleManager.Init( savedData );
        playStoreManager = GetComponentInChildren<PlayStoreManager>();
        playStoreManager.Init();
        achievementManager = GetComponentInChildren<AchievementManager>();
        adManager = GetComponentInChildren<AdManager>();
        soundManager = GetComponentInChildren<SoundManager>();
        soundManager.Init( saveLoadManager );
        soundManager.PlaySound( SoundEffectType.SOUND_SLITHER, false );
        soundManager.PlaySound( SoundEffectType.SOUND_AMBIENCE, false );
    }

    /// <summary>
    /// Tutorial on first play has been completed so start spawning fruit.
    /// </summary>
    public void TutorialDone() {
        fruitSpawner.Init();
        fruitSpawner.SpawnNewFruit( false );
    }

    /// <summary>
    /// Player has collected fruit so do all logic based on it.
    /// </summary>
    public void PlayerCollectedFruit() {
        fruitSpawner.SpawnNewFruit( false );
        scoreManager.IncreaseScore();
        powerupSpawner.UpdateActualCollectedFruit( scoreManager.GetCurrentScore() );
        fruitSpawner.SetMoveFruitTowardsPlayer( false );
        guiManager.FruitCollected( scoreManager.GetCurrentScore() );
        achievementManager.NotifyCurrentScoreIncreased( scoreManager.GetCurrentScore() );
        soundManager.PlaySound( SoundEffectType.SOUND_EAT, true );
    }

    /// <summary>
    /// Player collected powerup so find out which random one it is.
    /// </summary>
    public PlayerPowerupTypes PlayerCollectedPowerup() {
        soundManager.StopSound( SoundEffectType.SOUND_SLITHER );
        soundManager.PlaySound( SoundEffectType.SOUND_POWERUP_COLLECT, false );

        PlayerPowerupTypes collectedType = powerupSpawner.CollectPowerup( soundManager );
        guiManager.ShowPowerupIcon( collectedType );
        guiManager.SetPowerupDuration( powerupSpawner.GetPowerupDuration() );
        return collectedType;
    }

    /// <summary>
    /// Magnet collider of snake touched a fruit so move towards snake.
    /// </summary>
    public void PlayerMagnetTouchedFruit() {
        fruitSpawner.SetMoveFruitTowardsPlayer( true );
    }

    /// <summary>
    /// Any type of powerup has worn off so reset everything necessary. If "resumeSpawning" is set, then new powerups should be spawned after some time.
    /// </summary>
    public void PowerupWoreOff( bool resumeSpawning ) {
        guiManager.HidePowerupText();

        soundManager.StopSound( SoundEffectType.SOUND_THIN );
        soundManager.StopSound( SoundEffectType.SOUND_MAGNET );
        soundManager.StopSound( SoundEffectType.SOUND_INVINCIBILITY );
        soundManager.PlaySound( SoundEffectType.SOUND_SLITHER, false );
        soundManager.PlaySound( SoundEffectType.SOUND_POWERUP_WORE_OFF, false );

        if( resumeSpawning ) {
            powerupSpawner.ResumeSpawning();
        }
        fruitSpawner.SetMoveFruitTowardsPlayer( false );
    }

    /// <summary>
    /// Snake touched tail, so do everything for game over (or ad placement).
    /// </summary>
    public void PlayerTouchedTail() {
        fruitSpawner.Stop();
        powerupSpawner.Stop();
        snake.Stop();
        guiManager.ToggleHUD(false);

        soundManager.StopSound( SoundEffectType.SOUND_SLITHER );
        soundManager.PlaySound( SoundEffectType.SOUND_TAIL_EAT, false );

        if( scoreManager.GetCurrentScore() > scoreManager.GetMinRevivalScore() && adManager.IsAdAvailable() && saveLoadManager.GetTutorialStatus() ) {
            cameraController.Stop( true );
            guiManager.ShowAdScreen();
        } else {
            if( scoreManager.CheckDailyPlayReward( saveLoadManager ) ) {
                cameraController.Stop( false );
                scoreManager.ClaimDailyPlayReward( saveLoadManager, savedData );
                scoreManager.FinalizeScore( savedData );
                guiManager.ShowGameOverScreen( soundManager, scoreManager.GetCurrentScore(), scoreManager.GetTotalScore(), scoreManager.IsNewHighscore(), true, savedData.IsSomethingPurchaseable() );
            } else {
                cameraController.Stop( false );
                scoreManager.FinalizeScore( savedData );
                guiManager.ShowGameOverScreen( soundManager, scoreManager.GetCurrentScore(), scoreManager.GetTotalScore(), scoreManager.IsNewHighscore(), false, savedData.IsSomethingPurchaseable() );
            }
        }
    }

    /// <summary>
    /// Game over screen has been shown, so do all the heavy post processing (like posting scores) for it.
    /// </summary>
    public void GameOverScreenShown() {
        saveLoadManager.SetTutorialStatus( (int) TutorialStatus.TUTORIAL_DONE );
        saveLoadManager.SaveData( savedData );
        playStoreManager.PostScore( savedData.highscore, savedData.totalScore );
    }

    /// <summary>
    /// Change the scene to something else (or reset current scene).
    /// </summary>
    public void SwitchScreen( ScreenType screenType ) {
        guiManager.ShowScreenTransition( (int) screenType );
    }

    /// <summary>
    /// On Android, share screenshot of game over screen.
    /// </summary>
    public void ShareScreen() {
        string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
        ScreenCapture.CaptureScreenshot( screenshotName );

        new NativeShare().AddFile( screenShotPath, null ).SetSubject( shareSubject ).SetText( shareText1 + scoreManager.GetCurrentScore() + shareText2 ).Share();
    }

    /// <summary>
    /// Pause game.
    /// </summary>
    public void GamePaused() {
        paused = !paused;
        if( paused ) {
            Time.timeScale = 0;
            guiManager.TogglePauseMenu( true );
            soundManager.PlaySound( SoundEffectType.SOUND_PAUSE_START, false );
        } else {
            Time.timeScale = 1;
            guiManager.TogglePauseMenu( false );
            soundManager.PlaySound( SoundEffectType.SOUND_PAUSE_END, false );
        }
    }

    /// <summary>
    /// Ad has been watched by player so resume the game.
    /// </summary>
    public void GameResumedAfterAd() {
        soundManager.PlaySound( SoundEffectType.SOUND_INVINCIBILITY, false );
        cameraController.Resume();
        guiManager.HideAdScreen();
        guiManager.ToggleHUD(true);
        fruitSpawner.Resume();
        snake.Resume();
    }

    /// <summary>
    /// Ad placement has been skipped or failed so proceed with game over.
    /// </summary>
    public void AdSkippedOrFailed() {
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
