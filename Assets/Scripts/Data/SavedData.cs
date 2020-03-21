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
        currentColor = PlayerColorTypes.COLOR_CLASSIC;
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

    public Color GetColorByPurchaseableColorType(PurchaseableColorType purchaseableColorType) {
        return unlockedColors[(int) currentColor].GetColorByColorType(purchaseableColorType);
    }

    public Color GetColorByPurchaseableColorIndex(PurchaseableColorType purchaseableColorType, int index) {
        return unlockedColors[index].GetColorByColorType(purchaseableColorType);
    }

    public int GetCurrentLevel(int purchaseableIndex) {
        return unlockedPowerups[purchaseableIndex].GetCurrentLevel();
    }

    public int GetMaxLevel(int purchaseableIndex) {
        return unlockedPowerups[purchaseableIndex].GetMaxLevel();
    }

    public PlayerHatTypes GetSelectedHatType() {
        return currentHat;
    }

    public PlayerColorTypes GetSelectedColorType() {
        return currentColor;
    }

    public List<PowerupObject> GetUnlockedPowerups() {
        return unlockedPowerups;
    }

    public int GetTotalScore() {
        return totalScore;
    }

    public int GetPurchaseableBoughtCount() {
        int purchaseableBoughtCount = 0;
        foreach(HatObject hatObject in unlockedHats) {
            if (hatObject.IsUnlocked()) {
                purchaseableBoughtCount++;
            }
        }

        foreach(ColorObject colorObject in unlockedColors) {
            if (colorObject.IsUnlocked()) {
                purchaseableBoughtCount++;
            }
        }
        
        foreach(PowerupObject powerupObject in unlockedPowerups) {
            purchaseableBoughtCount += powerupObject.GetCurrentLevel();
        }
        return purchaseableBoughtCount;
    }

    public bool IsEverythingUnlocked() {
        int purchaseableCount = 0;
        int purchaseableBoughtCount = 0;
        foreach ( HatObject hatObject in unlockedHats ) {
            purchaseableCount++;
            if (hatObject.IsUnlocked()) {
                purchaseableBoughtCount++;
            }
        }

        foreach ( ColorObject colorObject in unlockedColors ) {
            purchaseableCount++;
            if (colorObject.IsUnlocked()) {
                purchaseableBoughtCount++;
            }
        }

        foreach ( PowerupObject powerupObject in unlockedPowerups ) {
            purchaseableCount += powerupObject.GetMaxLevel();
            purchaseableBoughtCount += powerupObject.GetCurrentLevel();
        }
        return purchaseableCount == purchaseableBoughtCount;
    }

    public bool IsPowerupAtMaxLevel() {
        foreach ( PowerupObject powerupObject in unlockedPowerups ) {
            if (powerupObject.GetCurrentLevel() == powerupObject.GetMaxLevel()) {
                return true;
            }
        }
        return false;
    }

    public void SetTotalScore(int totalScore) {
        this.totalScore = totalScore;
    }

    public void SetHighscore(int highscore) {
        this.highscore = highscore;
    }
}
