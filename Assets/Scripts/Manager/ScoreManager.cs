using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private int finalScore = 0;
    private int totalScore = 0;
    private bool newHighscore;

    private const int minRevivalScore = 20;
    private const int dailyPlayRewardScore = 100;

    public void Init(SavedData savedData) {
        totalScore = savedData.GetTotalScore();
    }

    public void IncreaseScore() {
        finalScore++;
        totalScore++;
    }

    public void FinalizeScore(SavedData savedData) {
        savedData.SetTotalScore(totalScore);
        if (finalScore > savedData.highscore) {
            savedData.SetHighscore(finalScore);
            newHighscore = true;
        }
    }

    public bool CheckDailyPlayReward(SaveLoadManager saveLoadManager) {
        System.TimeSpan timeDiff = System.DateTime.Now - saveLoadManager.GetLastRewardTime();
        return timeDiff.TotalHours >= 24;
    }

    public void ClaimDailyPlayReward(SaveLoadManager saveLoadManager, SavedData savedData) {
        saveLoadManager.SetLastRewardTime(System.DateTime.Today.ToString());
        totalScore += dailyPlayRewardScore;
    }
    
    public int GetCurrentScore() {
        return finalScore;
    }

    public int GetTotalScore() {
        return totalScore;
    }

    public int GetMinRevivalScore() {
        return minRevivalScore;
    }

    public bool IsNewHighscore() {
        return newHighscore;
    }
}
