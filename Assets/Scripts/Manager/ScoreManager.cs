using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    private int score = 0;

    private void Start() {
        scoreText.text = "0";
    }

    public void IncreaseScore() {
        score++;
        scoreText.text = score.ToString();
    }

    public int GetCurrentScore() {
        return score;
    }
}
