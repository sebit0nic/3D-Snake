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

    private const string animationShowToken = "OnShow";
    private const string loginFailString = "Login to Google Play failed...";
    private const string postFailString = "Could not post score....";

    public void Init() {
        playStoreNotificationText = playStoreNotification.GetComponentInChildren<Text>();
        PlayGamesPlatform.Activate();
    }

    public void SignIn() {
        Social.localUser.Authenticate((bool success) => {
            if (!success) {
                playStoreNotificationText.text = loginFailString;
                playStoreNotification.SetTrigger(animationShowToken);
            }
        });
    }

    public void UnlockAchievement(string unlockToken) {
        Social.ReportProgress(unlockToken, 100, (bool success) => { });
    }

    public void PostScore(int highscore, int totalscore) {
        Social.ReportScore(highscore, highscoreTag, (bool success) => {
            if (!success) {
                playStoreNotificationText.text = postFailString;
                playStoreNotification.SetTrigger(animationShowToken);
            }
        });
        Social.ReportScore(totalscore, totalscoreTag, (bool success) => {
            if (!success) {
                playStoreNotificationText.text = postFailString;
                playStoreNotification.SetTrigger(animationShowToken);
            }
        });
    }

    public void ShowLeaderboard() {
        Social.ShowLeaderboardUI();
    }
}
