using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedData {
    
    public List<HatObject> unlockedHats = new List<HatObject>();
    public List<ColorObject> unlockedColors = new List<ColorObject>();
    public List<PowerupObject> unlockedPowerups = new List<PowerupObject>();
    public int highscore;
    public int totalScore;
    public PlayerHatTypes currentHat;
    public PlayerColorTypes currentColor;

    private ShopSection currentShopSection;

    public SavedData(List<HatObject> standardHatObjects, List<ColorObject> standardColorObjects, List<PowerupObject> standardPowerupObjects) {
        unlockedHats = standardHatObjects;
        unlockedColors = standardColorObjects;
        unlockedPowerups = standardPowerupObjects;
        highscore = 0;
        totalScore = 0;
        currentHat = PlayerHatTypes.TYPE_DEFAULT;
        currentColor = PlayerColorTypes.COLOR_DEFAULT;
    }

    public void UnlockPurchaseable(int sectionIndex, int purchaseableIndex) {
        currentShopSection = (ShopSection) sectionIndex;
        switch ( currentShopSection ) {
            case ShopSection.HATS:
                totalScore -= unlockedHats[purchaseableIndex].GetPrice();
                unlockedHats[purchaseableIndex].Unlock();
                break;
            case ShopSection.COLORSCHEME:
                totalScore -= unlockedColors[purchaseableIndex].GetPrice();
                unlockedColors[purchaseableIndex].Unlock();
                break;
            case ShopSection.POWERUPS:
                totalScore -= unlockedPowerups[purchaseableIndex].GetPrice();
                unlockedPowerups[purchaseableIndex].Unlock();
                break;
        }
    }

    public void SelectPurchaseable(int sectionIndex, int purchaseableIndex) {
        currentShopSection = (ShopSection) sectionIndex;
        switch ( currentShopSection ) {
            case ShopSection.HATS:
                currentHat = (PlayerHatTypes) purchaseableIndex;
                break;
            case ShopSection.COLORSCHEME:
                currentColor = (PlayerColorTypes) purchaseableIndex;
                break;
        }
    }

    public bool IsPurchaseableUnlocked(int sectionIndex, int purchaseableIndex) {
        currentShopSection = (ShopSection) sectionIndex;
        switch ( currentShopSection ) {
            case ShopSection.HATS:
                return unlockedHats[purchaseableIndex].IsUnlocked();
            case ShopSection.COLORSCHEME:
                return unlockedColors[purchaseableIndex].IsUnlocked();
            case ShopSection.POWERUPS:
                return unlockedPowerups[purchaseableIndex].IsUnlocked();
            default:
                return false;
        }
    }

    public PlayerHatTypes GetSelectedHatType() {
        return currentHat;
    }

    public PlayerColorTypes GetSelectedColorType() {
        return currentColor;
    }

    public int GetPurchaseablePrice(int sectionIndex, int purchaseableIndex) {
        currentShopSection = (ShopSection) sectionIndex;
        switch ( currentShopSection ) {
            case ShopSection.HATS:
                return unlockedHats[purchaseableIndex].GetPrice();
            case ShopSection.COLORSCHEME:
                return unlockedColors[purchaseableIndex].GetPrice();
            case ShopSection.POWERUPS:
                return unlockedPowerups[purchaseableIndex].GetPrice();
            default:
                return 0;
        }
    }

    public string GetPurchaseableName(int sectionIndex, int purchaseableIndex) {
        currentShopSection = (ShopSection) sectionIndex;
        switch ( currentShopSection ) {
            case ShopSection.HATS:
                return unlockedHats[purchaseableIndex].GetName();
            case ShopSection.COLORSCHEME:
                return unlockedColors[purchaseableIndex].GetName();
            case ShopSection.POWERUPS:
                return unlockedPowerups[purchaseableIndex].GetName();
            default:
                return "DUMMY";
        }
    }

    public int GetCurrentLevel(int sectionIndex, int purchaseableIndex) {
        currentShopSection = (ShopSection) sectionIndex;
        if (currentShopSection == ShopSection.HATS || currentShopSection == ShopSection.COLORSCHEME) {
            Debug.Log("ERROR: HATS and COLORSCHEMES do not have levels!");
            return 0;
        } else {
            return unlockedPowerups[purchaseableIndex].GetCurrentLevel();
        }
    }

    public int GetMaxLevel(int sectionIndex, int purchaseableIndex) {
        currentShopSection = (ShopSection) sectionIndex;
        if ( currentShopSection == ShopSection.HATS || currentShopSection == ShopSection.COLORSCHEME ) {
            Debug.Log("ERROR: HATS and COLORSCHEMES do not have levels!");
            return 0;
        } else {
            return unlockedPowerups[purchaseableIndex].GetMaxLevel();
        }
    }

    public List<PowerupObject> GetUnlockedPowerups() {
        return unlockedPowerups;
    }

    public int GetTotalScore() {
        return totalScore;
    }

    public void SetTotalScore(int totalScore) {
        this.totalScore = totalScore;
    }

    public void SetHighscore(int highscore) {
        this.highscore = highscore;
    }
}
