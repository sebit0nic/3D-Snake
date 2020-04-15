using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Google Play achievement progress
/// </summary>
public class AchievementManager : MonoBehaviour {

    public string[] achievementUnlockTokens;
    public PlayStoreManager playStoreManager;

    private const int snakeNoviceScore = 50;
    private const int snakeMasterScore = 100;
    private const int snakeKingScore = 150;
    private const int collectorCount = 12;

    /// <summary>
    /// Notification that the player has collected a fruit and score has increased during gameplay.
    /// </summary>
    public void NotifyCurrentScoreIncreased( int currentScore ) {
        if( currentScore == snakeNoviceScore ) {
            playStoreManager.UnlockAchievement( achievementUnlockTokens[(int) AchievementType.SNAKE_NOVICE] );
        }
        if( currentScore == snakeMasterScore ) {
            playStoreManager.UnlockAchievement( achievementUnlockTokens[(int) AchievementType.SNAKE_MASTER] );
        }
        if( currentScore == snakeKingScore ) {
            playStoreManager.UnlockAchievement( achievementUnlockTokens[(int) AchievementType.SNAKE_KING] );
        }
    }

    /// <summary>
    /// Notification that the player has bought something in the shop.
    /// </summary>
    public void NotifyPurchaseableBought( int count ) {
        if( count >= collectorCount ) {
            playStoreManager.UnlockAchievement( achievementUnlockTokens[(int) AchievementType.COLLECTOR] );
        }
    }

    /// <summary>
    /// Notification that player has upgraded a powerup to the max level in the shop.
    /// </summary>
    public void NotifyPowerupAtMaxLevel() {
        playStoreManager.UnlockAchievement( achievementUnlockTokens[(int) AchievementType.UPGRADER] );
    }

    /// <summary>
    /// Notification that the player has unlocked every single item in the shop.
    /// </summary>
    public void NotifyEverythingUnlocked() {
        playStoreManager.UnlockAchievement( achievementUnlockTokens[(int) AchievementType.HOARDER] );
    }
}
