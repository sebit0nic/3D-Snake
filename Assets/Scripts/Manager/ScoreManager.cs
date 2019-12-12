using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private int finalScore = 0;
    private int totalScore = 0;

    private void Start() {
        //TODO: get totalScore from saved score
    }

    public void IncreaseScore() {
        finalScore++;
        totalScore++;
    }

    public int GetCurrentScore() {
        return finalScore;
    }

    public int GetTotalScore() {
        return totalScore;
    }
}
