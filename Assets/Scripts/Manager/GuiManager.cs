using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all UI elements of the game scene.
/// </summary>
public class GuiManager : MonoBehaviour {

    public GameObject pauseButton, pauseMenu;
    public GameObject steerRightButton, steerLeftButton;
    public GameObject gameOverScreen;
    public GameObject adScreen;
    public Animator adScreenAnimator;
    public GameObject dailyPlayRewardNotification;
    public GameObject newHighscoreCrown;
    public ScreenTransition screenTransition;
    public Text finalScoreText, totalScoreText;
    public Text currentScoreText;
    public Image powerupIcon;
    public Image powerupDurationImage;
    public Animator collectCircleEffect;
    public Animator shopButtonAnimator;
    public Animator currentScoreAnimator;
    public TouchIndicator touchIndicatorLeft, touchIndicatorRight;
    public Transform cameraTransform;
    public float scoreCountDuration;

    [Header("Portrait Variants")]
    public GameObject gameOverScreenPortrait;
    public GameObject dailyPlayRewardNotificationPortrait;
    public GameObject newHighscoreCrownPortrait;
    public Text finalScoreTextPortrait, totalScoreTextPortrait;
    public Animator shopButtonPortraitAnimator;
    public GameObject adScreenPortrait;
    public Animator adScreenAnimatorPortrait;

    [Header("Powerup Icons")]
    public Sprite powerupInvincibilityIcon;
    public Sprite powerupThinIcon;
    public Sprite powerupMagnetIcon;

    private float powerupDuration, currentPowerupDuration;
    private string finalScoreString, totalScoreString, currentScoreString;
    private int tutorialsDone;
    private int screenOrientation;

    private const char padChar = '0';
    private const int padLengthFinalScore = 3;
    private const int padLengthTotalScore = 5;
    private const float baseSoundPitch = 0.1f;
    private const float waitForScreenDuration = 1f;
    private const string collectCircleCollectedTrigger = "OnCollected";
    private const string adScreenHideTrigger = "OnHide";
    private const string currentScoreIncreaseTrigger = "OnIncrease";

    public void Init( SaveLoadManager saveLoadManager ) {
        powerupIcon.enabled = false;
        powerupDurationImage.fillAmount = 0f;
        screenOrientation = saveLoadManager.GetScreenOrientationStatus();

        touchIndicatorLeft.Init( this, saveLoadManager.GetTutorialStatus() );
        touchIndicatorRight.Init( this, saveLoadManager.GetTutorialStatus() );
    }

    private void Update() {
        if( powerupDuration != 0 && currentPowerupDuration != 0 ) {
            currentPowerupDuration -= Time.deltaTime;
            currentPowerupDuration = Mathf.Clamp( currentPowerupDuration, 0, 100 );
            powerupDurationImage.fillAmount = currentPowerupDuration / powerupDuration;
        }
    }

    /// <summary>
    /// Fruit has been collected by player.
    /// </summary>
    public void FruitCollected( int currentScore ) {
        collectCircleEffect.transform.LookAt( cameraTransform );
        collectCircleEffect.SetTrigger( collectCircleCollectedTrigger );

        currentScoreString = currentScore.ToString();
        currentScoreText.text = currentScoreString.PadLeft( padLengthFinalScore, padChar );
        currentScoreAnimator.SetTrigger( currentScoreIncreaseTrigger );
    }

    /// <summary>
    /// Show/hide pause menu according to "value".
    /// </summary>
    public void TogglePauseMenu( bool value ) {
        pauseMenu.SetActive( value );
    }

    /// <summary>
    /// Show/hide the HUD according to "value".
    /// </summary>
    public void ToggleHUD( bool value ) {
        pauseButton.SetActive( value );
        steerRightButton.SetActive( value );
        steerLeftButton.SetActive( value );
        powerupIcon.gameObject.SetActive( value );
        powerupDurationImage.gameObject.SetActive( value );
        currentScoreText.gameObject.SetActive( value );
    }

    /// <summary>
    /// Player touched tail and has no ad left to watch so show game over screen.
    /// </summary>
    public void ShowGameOverScreen( SoundManager soundManager, int finalScore, int totalScore, bool newHighscore, bool dailyPlayRewardActive, bool isSomethingPurchaseable ) {
        if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            dailyPlayRewardNotification.gameObject.SetActive( dailyPlayRewardActive );
            shopButtonAnimator.enabled = isSomethingPurchaseable;
        } else {
            shopButtonPortraitAnimator.enabled = isSomethingPurchaseable;
            dailyPlayRewardNotificationPortrait.gameObject.SetActive( dailyPlayRewardActive );
        }
        
        StartCoroutine( OnWaitForGameOverScreen( soundManager, finalScore, totalScore, newHighscore ) );
    }

    /// <summary>
    /// Player has an ad left so show the ad decision screen.
    /// </summary>
    public void ShowAdScreen() {
        StartCoroutine( OnWaitForAdScreen() );
    }

    /// <summary>
    /// Player changes scene.
    /// </summary>
    public void ShowScreenTransition( int sceneID ) {
        screenTransition.StartScreenTransition( sceneID );
    }

    /// <summary>
    /// Player has collected a powerup so show the right icon on the HUD.
    /// </summary>
    public void ShowPowerupIcon( PlayerPowerupTypes collectedType ) {
        powerupIcon.enabled = true;
        switch( collectedType ) {
            case PlayerPowerupTypes.INVINCIBILTY:
                powerupIcon.sprite = powerupInvincibilityIcon;
                break;
            case PlayerPowerupTypes.MAGNET:
                powerupIcon.sprite = powerupMagnetIcon;
                break;
            case PlayerPowerupTypes.THIN:
                powerupIcon.sprite = powerupThinIcon;
                break;
        }
    }

    /// <summary>
    /// Set the duration so that the fill amount is right on the powerup duration sprite.
    /// </summary>
    public void SetPowerupDuration( float powerupDuration ) {
        this.powerupDuration = powerupDuration;
        currentPowerupDuration = powerupDuration;
    }

    /// <summary>
    /// Hide all powerup information after it has worn off.
    /// </summary>
    public void HidePowerupText() {
        powerupIcon.enabled = false;
        powerupDurationImage.fillAmount = 0;
        powerupDuration = 0;
        currentPowerupDuration = 0;
    }

    /// <summary>
    /// Hide the ad decision screen after the player has watched it.
    /// </summary>
    public void HideAdScreen() {
        if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            adScreenAnimator.SetTrigger( adScreenHideTrigger );
        } else {
            adScreenAnimatorPortrait.SetTrigger( adScreenHideTrigger );
        }
    }

    /// <summary>
    /// Player has pressed both direction buttons for long enough so end the tutorial.
    /// </summary>
    public void TouchIndicatorTutorialDone() {
        tutorialsDone++;
        if( tutorialsDone == 2 ) {
            GameManager.instance.TutorialDone();
        }
    }

    /// <summary>
    /// Coroutine to wait a few milliseconds before showing the game over screen.
    /// </summary>
    private IEnumerator OnWaitForGameOverScreen( SoundManager soundManager, int finalScore, int totalScore, bool newHighscore ) {
        yield return new WaitForSeconds( waitForScreenDuration );
        if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            gameOverScreen.SetActive(true);
        } else {
            gameOverScreenPortrait.SetActive(true);
        }
        
        GameManager.instance.GameOverScreenShown();
        StartCoroutine( OnShowFinalScore( soundManager, finalScore, totalScore, newHighscore ) );
    }

    /// <summary>
    /// Coroutine to wait a few milliseconds before showing the ad decision screen.
    /// </summary>
    private IEnumerator OnWaitForAdScreen() {
        yield return new WaitForSeconds( waitForScreenDuration );
        if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            adScreen.SetActive(true);
        } else {
            adScreenPortrait.SetActive(true);
        }
    }

    /// <summary>
    /// Coroutine to count up the final score.
    /// </summary>
    private IEnumerator OnShowFinalScore( SoundManager soundManager, int finalScore, int totalScore, bool newHighscore ) {
        int tempTotalScore = totalScore - finalScore;
        totalScoreString = tempTotalScore.ToString();
        if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            totalScoreText.text = totalScoreString.PadLeft( padLengthTotalScore, padChar );
        } else {
            totalScoreTextPortrait.text = totalScoreString.PadLeft( padLengthTotalScore, padChar );
        }
        
        int tempScore = 0;
        float countTempo = scoreCountDuration / finalScore;
        while( tempScore < finalScore ) {
            tempScore++;
            finalScoreString = tempScore.ToString();
            if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
                finalScoreText.text = finalScoreString.PadLeft( padLengthFinalScore, padChar );
            } else {
                finalScoreTextPortrait.text = finalScoreString.PadLeft( padLengthFinalScore, padChar );
            }
            soundManager.PlaySoundWithPitch( SoundEffectType.SOUND_POINTS_UP, baseSoundPitch + countTempo * tempScore );
            yield return new WaitForSeconds( countTempo );
        }

        tempScore = finalScore;
        finalScoreString = tempScore.ToString();
        if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            finalScoreText.text = finalScoreString.PadLeft( padLengthFinalScore, padChar );
        } else {
            finalScoreTextPortrait.text = finalScoreString.PadLeft( padLengthFinalScore, padChar );
        }
        
        if( newHighscore ) {
            if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
                newHighscoreCrown.SetActive( true );
            } else {
                newHighscoreCrownPortrait.SetActive( true );
            }
            
            soundManager.PlaySound( SoundEffectType.SOUND_NEW_HIGHSCORE, false );
        }

        StartCoroutine( OnShowTotalScore( soundManager, finalScore, totalScore ) );
    }

    /// <summary>
    /// Coroutine to count up the total score.
    /// </summary>
    private IEnumerator OnShowTotalScore( SoundManager soundManager, int finalScore, int totalScore ) {
        int tempScore = totalScore - finalScore;
        float countTempo = scoreCountDuration / finalScore;
        while( tempScore < totalScore ) {
            tempScore++;
            totalScoreString = tempScore.ToString();
            if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
                totalScoreText.text = totalScoreString.PadLeft(padLengthTotalScore, padChar);
            } else {
                totalScoreTextPortrait.text = totalScoreString.PadLeft(padLengthTotalScore, padChar);
            }
            soundManager.PlaySoundWithPitch( SoundEffectType.SOUND_TOTAL_POINTS_UP, baseSoundPitch + countTempo * tempScore );
            yield return new WaitForSeconds( countTempo );
        }

        tempScore = totalScore;
        totalScoreString = tempScore.ToString();
        if( screenOrientation == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            totalScoreText.text = totalScoreString.PadLeft(padLengthTotalScore, padChar);
        } else {
            totalScoreTextPortrait.text = totalScoreString.PadLeft(padLengthTotalScore, padChar);
        }
    }
}
