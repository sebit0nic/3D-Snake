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
        //TODO: decrease totalScore by price
        ShopSection shopSection = (ShopSection) sectionIndex;
        switch ( shopSection ) {
            case ShopSection.HATS:
                unlockedHats[purchaseableIndex].Unlock();
                break;
            case ShopSection.COLORSCHEME:
                unlockedColors[purchaseableIndex].Unlock();
                break;
            case ShopSection.POWERUPS:
                unlockedPowerups[purchaseableIndex].Unlock();
                break;
        }
    }

    public void SelectPurchaseable(int sectionIndex, int purchaseableIndex) {
        ShopSection shopSection = (ShopSection) sectionIndex;
        switch ( shopSection ) {
            case ShopSection.HATS:
                foreach(HatObject hatObject in unlockedHats) {
                    hatObject.SetSelected(false);
                }
                unlockedHats[purchaseableIndex].SetSelected(true);
                break;
            case ShopSection.COLORSCHEME:
                foreach (ColorObject colorObject in unlockedColors) {
                    colorObject.SetSelected(false);
                }
                unlockedColors[purchaseableIndex].SetSelected(true);
                break;
        }
    }

    public bool IsPurchaseableUnlocked(int sectionIndex, int purchaseableIndex) {
        ShopSection shopSection = (ShopSection) sectionIndex;
        switch ( shopSection ) {
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

    public bool IsPurchaseableSelected(int sectionIndex, int purchaseableIndex) {
        ShopSection shopSection = (ShopSection) sectionIndex;
        switch ( shopSection ) {
            case ShopSection.HATS:
                return unlockedHats[purchaseableIndex].IsSelected();
            case ShopSection.COLORSCHEME:
                return unlockedColors[purchaseableIndex].IsSelected();
            case ShopSection.POWERUPS:
                return false;
            default:
                return false;
        }
    }

    public int GetPurchaseablePrice(int sectionIndex, int purchaseableIndex) {
        ShopSection shopSection = (ShopSection) sectionIndex;
        switch ( shopSection ) {
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
}
