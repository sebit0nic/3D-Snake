using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {

    public GameObject retryButton, gameoverText, pauseButton, scoreText;
    public GameObject steerRightButton, steerLeftButton;
    public Text powerupText;
    public Image powerupDurationImage;

    private float powerupDuration, currentPowerupDuration;

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
        scoreText.SetActive(false);
        steerRightButton.SetActive(false);
        steerLeftButton.SetActive(false);
    }

    public void ShowGameOverScreen() {
        retryButton.SetActive(true);
        gameoverText.SetActive(true);
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
