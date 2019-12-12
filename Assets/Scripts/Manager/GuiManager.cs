using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {

    public GameObject pauseButton;
    public GameObject steerRightButton, steerLeftButton;
    public GameObject gameOverScreen;
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

    public void ShowPowerupText(PowerupType collectedType) {
        switch ( collectedType ) {
            case PowerupType.INVINCIBILTY:
                powerupText.text = "INVINCIBILITY";
                break;
            case PowerupType.MAGNET:
                powerupText.text = "MAGNET";
                break;
            case PowerupType.THIN:
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
