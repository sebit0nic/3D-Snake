using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayStoreManager : MonoBehaviour {

    public Animator playStoreNotification;

    private Text playStoreNotificationText;

    private const string highscoreTag = "CgkIv_LIqYYREAIQAQ";
    private const string totalscoreTag = "CgkIv_LIqYYREAIQAg";

    private const string loginFailString = "Login to Google Play failed...";
    private const string loadFailString = "Could not load leaderboards...";

    private void Start() {
        playStoreNotificationText = playStoreNotification.GetComponentInChildren<Text>();
        PlayGamesPlatform.Activate();
    }

    public void SignIn() {
        Social.localUser.Authenticate((bool success) => {
            if (!success) {
                playStoreNotificationText.text = loginFailString;
                playStoreNotification.SetTrigger("OnShow");
            }
        });
    }

    public void UnlockAchievement() {

    }

    public void PostScore(int highscore, int totalscore) {
        Social.ReportScore(highscore, highscoreTag, (bool success) => {});
        Social.ReportScore(totalscore, totalscoreTag, (bool success) => { });
    }

    public void ShowLeaderboard() {
        Social.ShowLeaderboardUI();
    }
}
