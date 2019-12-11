using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text finalScoreText;

    private int score = 0;

    public void IncreaseScore() {
        score++;
    }

    public void SetFinalScoreText() {
        string finalScoreString = score.ToString();
        finalScoreText.text = finalScoreString.PadLeft(3, '0');
    }

    public int GetCurrentScore() {
        return score;
    }
}
