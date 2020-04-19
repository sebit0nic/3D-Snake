using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all score related logic in the game scene.
/// </summary>
public class ScoreManager : MonoBehaviour {

    private int finalScore = 0;
    private int totalScore = 0;
    private bool newHighscore;

    private const int minRevivalScore = 20;
    private const int dailyPlayRewardScore = 100;

    public void Init( SavedData savedData ) {
        totalScore = savedData.GetTotalScore();
    }

    /// <summary>
    /// Increase score after player collected fruit.
    /// </summary>
    public void IncreaseScore() {
        finalScore++;
        totalScore++;
    }

    /// <summary>
    /// Write scores to the savedData object so that it can be saved persistently afterwards.
    /// </summary>
    public void FinalizeScore( SavedData savedData ) {
        savedData.SetTotalScore( totalScore );
        if( finalScore > savedData.highscore ) {
            savedData.SetHighscore( finalScore );
            newHighscore = true;
        }
    }

    /// <summary>
    /// Check if the player gets a daily play reward, so check if the last time he got one was yesterday.
    /// </summary>
    public bool CheckDailyPlayReward( SaveLoadManager saveLoadManager ) {
        System.TimeSpan timeDiff = System.DateTime.Now - saveLoadManager.GetLastRewardTime();
        return timeDiff.TotalHours >= 24;
    }

    /// <summary>
    /// Award the player a daily play reward and set the next reward timestamp to the next day.
    /// </summary>
    public void ClaimDailyPlayReward( SaveLoadManager saveLoadManager, SavedData savedData ) {
        saveLoadManager.SetLastRewardTime( System.DateTime.Today.ToString() );
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
