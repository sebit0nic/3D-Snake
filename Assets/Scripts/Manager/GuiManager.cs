using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour {

    public GameObject retryButton, gameoverText;

    public void ShowGameOverScreen() {
        retryButton.SetActive(true);
        gameoverText.SetActive(true);
    }
}
