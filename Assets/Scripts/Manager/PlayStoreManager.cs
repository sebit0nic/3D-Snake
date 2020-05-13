using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

/// <summary>
/// Handles all communication with the Google Play Store.
/// </summary>
public class PlayStoreManager : MonoBehaviour {

    private const string highscoreTag = "CgkIv_LIqYYREAIQAQ";
    private const string totalscoreTag = "CgkIv_LIqYYREAIQAg";

    public void Init() {
        PlayGamesPlatform.Activate();
    }

    /// <summary>
    /// Sign in to Google Play (preferably on startup of the game).
    /// </summary>
    public void SignIn() {
        Social.localUser.Authenticate( ( bool success ) => { if( !success ) { } } );
    }

    /// <summary>
    /// Unlock a specific achievement with the given "unlockToken".
    /// </summary>
    public void UnlockAchievement( string unlockToken ) {
        Social.ReportProgress( unlockToken, 100, ( bool success ) => { } );
    }

    /// <summary>
    /// Post the highscore on Google Play leaderboards (does not matter if it's a new highscore, Google checks that in the background).
    /// </summary>
    public void PostScore( int highscore, int totalscore ) {
        Social.ReportScore( highscore, highscoreTag, ( bool success ) => { if( !success ) {} } );
        Social.ReportScore( totalscore, totalscoreTag, ( bool success ) => { if( !success ) { } } );
    }

    /// <summary>
    /// Show the Google Play Leaderboard UI.
    /// </summary>
    public void ShowLeaderboard() {
        Social.ShowLeaderboardUI();
    }
}
