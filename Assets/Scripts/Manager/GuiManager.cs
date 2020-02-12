using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {

    public GameObject pauseButton, pauseMenu;
    public GameObject steerRightButton, steerLeftButton;
    public GameObject gameOverScreen;
    public ScreenTransition screenTransition;
    public Text finalScoreText, totalScoreText;
    public Image powerupIcon;
    public Image powerupDurationImage;
    public CollectCircleEffect collectCircleEffect;

    [Header("Powerup Icons")]
    public Sprite powerupInvincibilityIcon;
    public Sprite powerupThinIcon;
    public Sprite powerupMagnetIcon;

    private float powerupDuration, currentPowerupDuration;
    private string finalScoreString, totalScoreString;

    private void Start() {
        powerupIcon.enabled = false;
        powerupDurationImage.fillAmount = 0f;
    }

    private void Update() {
        if (powerupDuration != 0 && currentPowerupDuration != 0) {
            currentPowerupDuration -= Time.deltaTime;
            currentPowerupDuration = Mathf.Clamp(currentPowerupDuration, 0, 100);
            powerupDurationImage.fillAmount = currentPowerupDuration / powerupDuration;
        }
    }

    public void FruitCollected() {
        collectCircleEffect.NotifyFruitCollected();
    }

    public void TogglePauseMenu(bool value) {
        pauseMenu.SetActive(value);
    }

    public void HideHUD() {
        pauseButton.SetActive(false);
        steerRightButton.SetActive(false);
        steerLeftButton.SetActive(false);
        powerupIcon.gameObject.SetActive(false);
        powerupDurationImage.gameObject.SetActive(false);
    }

    public void ShowGameOverScreen(int finalScore, int totalScore) {
        gameOverScreen.SetActive(true);
        finalScoreString = finalScore.ToString();
        totalScoreString = totalScore.ToString();
        finalScoreText.text = finalScoreString.PadLeft(3, '0');
        totalScoreText.text = totalScoreString.PadLeft(5, '0');
    }

    public void ShowScreenTransition(int sceneID) {
        screenTransition.StartScreenTransition(sceneID);
    }

    public void ShowPowerupIcon(PlayerPowerupTypes collectedType) {
        powerupIcon.enabled = true;
        switch ( collectedType ) {
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

    public void SetPowerupDuration(float powerupDuration) {
        this.powerupDuration = powerupDuration;
        currentPowerupDuration = powerupDuration;
    }

    public void HidePowerupText() {
        powerupIcon.enabled = false;
    }
}
