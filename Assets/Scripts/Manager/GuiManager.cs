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
    public Text powerupText;
    public Image powerupDurationImage;

    private float powerupDuration, currentPowerupDuration;
    private string finalScoreString, totalScoreString;

    private void Start() {
        powerupText.text = " ";
        powerupDurationImage.fillAmount = 0f;
    }

    private void Update() {
        if (powerupDuration != 0 && currentPowerupDuration != 0) {
            currentPowerupDuration -= Time.deltaTime;
            currentPowerupDuration = Mathf.Clamp(currentPowerupDuration, 0, 100);
            powerupDurationImage.fillAmount = currentPowerupDuration / powerupDuration;
        }
    }

    public void TogglePauseMenu(bool value) {
        pauseMenu.SetActive(value);
    }

    public void HideHUD() {
        pauseButton.SetActive(false);
        steerRightButton.SetActive(false);
        steerLeftButton.SetActive(false);
    }

    public void ShowGameOverScreen(int finalScore, int totalScore) {
        gameOverScreen.SetActive(true);
        finalScoreString = finalScore.ToString();
        totalScoreString = totalScore.ToString();
        finalScoreText.text = finalScoreString.PadLeft(3, '0');
        totalScoreText.text = totalScoreString.PadLeft(6, '0');
    }

    public void ShowScreenTransition(int sceneID) {
        screenTransition.StartScreenTransition(sceneID);
    }

    public void ShowPowerupText(PlayerPowerupTypes collectedType) {
        switch ( collectedType ) {
            case PlayerPowerupTypes.INVINCIBILTY:
                powerupText.text = "INVINCIBILITY";
                break;
            case PlayerPowerupTypes.MAGNET:
                powerupText.text = "MAGNET";
                break;
            case PlayerPowerupTypes.THIN:
                powerupText.text = "THIN";
                break;
        }
    }

    public void SetPowerupDuration(float powerupDuration) {
        this.powerupDuration = powerupDuration;
        currentPowerupDuration = powerupDuration;
    }

    public void HidePowerupText() {
        powerupText.text = " ";
    }
}
