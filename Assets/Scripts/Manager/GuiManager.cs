﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {

    public GameObject pauseButton, pauseMenu;
    public GameObject steerRightButton, steerLeftButton;
    public GameObject gameOverScreen;
    public GameObject newHighscoreCrown;
    public ScreenTransition screenTransition;
    public Text finalScoreText, totalScoreText;
    public Image powerupIcon;
    public Image powerupDurationImage;
    public Animator collectCircleEffect;
    public float scoreCountDuration;

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
        collectCircleEffect.transform.LookAt(Camera.main.transform);
        collectCircleEffect.SetTrigger("OnCollected");
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

    public void ShowGameOverScreen(int finalScore, int totalScore, bool newHighscore) {
        gameOverScreen.SetActive(true);
        StartCoroutine(OnShowFinalScore(finalScore, totalScore, newHighscore));
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

    private IEnumerator OnShowFinalScore(int finalScore, int totalScore, bool newHighscore) {
        int tempTotalScore = totalScore - finalScore;
        totalScoreString = tempTotalScore.ToString();
        totalScoreText.text = totalScoreString.PadLeft(5, '0');

        int tempScore = 0;
        float countTempo = scoreCountDuration / finalScore;
        while (tempScore < finalScore) {
            tempScore++;
            finalScoreString = tempScore.ToString();
            finalScoreText.text = finalScoreString.PadLeft(3, '0');
            yield return new WaitForSeconds(countTempo);
        }

        tempScore = finalScore;
        finalScoreString = tempScore.ToString();
        finalScoreText.text = finalScoreString.PadLeft(3, '0');
        if (newHighscore) {
            newHighscoreCrown.SetActive(true);
        }

        StartCoroutine(OnShowTotalScore(finalScore, totalScore));
    }

    private IEnumerator OnShowTotalScore(int finalScore, int totalScore) {
        int tempScore = totalScore - finalScore;
        float countTempo = scoreCountDuration / finalScore;
        while (tempScore < totalScore) {
            tempScore++;
            totalScoreString = tempScore.ToString();
            totalScoreText.text = totalScoreString.PadLeft(5, '0');
            yield return new WaitForSeconds(countTempo);
        }

        tempScore = totalScore;
        totalScoreString = tempScore.ToString();
        totalScoreText.text = totalScoreString.PadLeft(5, '0');
    }
}
