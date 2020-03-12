using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour {

    public string[] achievementUnlockTokens;
    public PlayStoreManager playStoreManager;

    private const int snakeNoviceScore = 50;
    private const int snakeMasterScore = 100;
    private const int snakeKingScore = 150;
    private const int collectorCount = 10;

    public void NotifyCurrentScoreIncreased(int currentScore) {
        if (currentScore >= snakeNoviceScore) {
            playStoreManager.UnlockAchievement(achievementUnlockTokens[(int) AchievementType.SNAKE_NOVICE]);
        }
        if (currentScore >= snakeMasterScore) {
            playStoreManager.UnlockAchievement(achievementUnlockTokens[(int) AchievementType.SNAKE_MASTER]);
        }
        if (currentScore >= snakeKingScore) {
            playStoreManager.UnlockAchievement(achievementUnlockTokens[(int) AchievementType.SNAKE_KING]);
        }
    }

    public void NotifyPurchaseableBought(int count) {
        if (count >= collectorCount) {
            playStoreManager.UnlockAchievement(achievementUnlockTokens[(int) AchievementType.COLLECTOR]);
        }
    }

    public void NotifyPowerupAtMaxLevel() {
        playStoreManager.UnlockAchievement(achievementUnlockTokens[(int) AchievementType.UPGRADER]);
    }

    public void NotifyEverythingUnlocked() {
        playStoreManager.UnlockAchievement(achievementUnlockTokens[(int) AchievementType.HOARDER]);
    }
}
