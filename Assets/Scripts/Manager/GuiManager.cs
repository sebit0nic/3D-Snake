using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {

    public GameObject retryButton, gameoverText; 
    public Text powerupText;

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

    public void HidePowerupText() {
        powerupText.text = " ";
    }
}
