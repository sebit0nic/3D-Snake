using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private int finalScore = 0;
    private int totalScore = 0;

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
        }
    }

    public int GetCurrentScore() {
        return finalScore;
    }

    public int GetTotalScore() {
        return totalScore;
    }
}
