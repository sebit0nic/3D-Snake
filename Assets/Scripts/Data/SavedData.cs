using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all the data that is being saved on disk
/// </summary>
[System.Serializable]
public class SavedData {
    
    public List<HatObject> hatObjectList = new List<HatObject>();
    public List<ColorObject> colorObjectList = new List<ColorObject>();
    public List<PowerupObject> powerupObjectList = new List<PowerupObject>();
    public int highscore;
    public int totalScore;
    public PlayerHatTypes currentHat;
    public PlayerColorTypes currentColor;

    private ShopSection currentShopSection;

    public SavedData( List<HatObject> standardHatObjects, List<ColorObject> standardColorObjects, List<PowerupObject> standardPowerupObjects ) {
        hatObjectList = standardHatObjects;
        colorObjectList = standardColorObjects;
        powerupObjectList = standardPowerupObjects;
        highscore = 0;
        totalScore = 999;
        currentHat = PlayerHatTypes.TYPE_DEFAULT;
        currentColor = PlayerColorTypes.COLOR_CLASSIC;
    }

    /// <summary>
    /// Unlock a certain purchaseable by the players request.
    /// </summary>
    public void UnlockPurchaseable( int sectionIndex, int purchaseableIndex ) {
        currentShopSection = (ShopSection) sectionIndex;
        switch( currentShopSection ) {
            case ShopSection.HATS:
                totalScore -= hatObjectList[purchaseableIndex].GetPrice();
                hatObjectList[purchaseableIndex].Unlock();
                break;
            case ShopSection.COLORSCHEME:
                totalScore -= colorObjectList[purchaseableIndex].GetPrice();
                colorObjectList[purchaseableIndex].Unlock();
                break;
            case ShopSection.POWERUPS:
                totalScore -= powerupObjectList[purchaseableIndex].GetPrice();
                powerupObjectList[purchaseableIndex].Unlock();
                break;
        }
    }

    /// <summary>
    /// Change the currently selected purchaseable by the players request.
    /// </summary>
    public void SelectPurchaseable( int sectionIndex, int purchaseableIndex ) {
        currentShopSection = (ShopSection) sectionIndex;
        switch( currentShopSection ) {
            case ShopSection.HATS:
                currentHat = (PlayerHatTypes) purchaseableIndex;
                break;
            case ShopSection.COLORSCHEME:
                currentColor = (PlayerColorTypes) purchaseableIndex;
                break;
        }
    }

    public bool IsPurchaseableUnlocked( int sectionIndex, int purchaseableIndex ) {
        currentShopSection = (ShopSection) sectionIndex;
        switch( currentShopSection ) {
            case ShopSection.HATS:
                return hatObjectList[purchaseableIndex].IsUnlocked();
            case ShopSection.COLORSCHEME:
                return colorObjectList[purchaseableIndex].IsUnlocked();
            case ShopSection.POWERUPS:
                return powerupObjectList[purchaseableIndex].IsUnlocked();
            default:
                return false;
        }
    }

    public int GetPurchaseablePrice( int sectionIndex, int purchaseableIndex ) {
        currentShopSection = (ShopSection) sectionIndex;
        switch( currentShopSection ) {
            case ShopSection.HATS:
                return hatObjectList[purchaseableIndex].GetPrice();
            case ShopSection.COLORSCHEME:
                return colorObjectList[purchaseableIndex].GetPrice();
            case ShopSection.POWERUPS:
                return powerupObjectList[purchaseableIndex].GetPrice();
            default:
                return 0;
        }
    }

    public string GetPurchaseableName( int sectionIndex, int purchaseableIndex ) {
        currentShopSection = (ShopSection) sectionIndex;
        switch( currentShopSection ) {
            case ShopSection.HATS:
                return hatObjectList[purchaseableIndex].GetName();
            case ShopSection.COLORSCHEME:
                return colorObjectList[purchaseableIndex].GetName();
            case ShopSection.POWERUPS:
                return powerupObjectList[purchaseableIndex].GetName();
            default:
                return "DUMMY";
        }
    }

    public Color GetColorByPurchaseableColorType( PurchaseableColorType purchaseableColorType ) {
        return colorObjectList[(int) currentColor].GetColorByColorType( purchaseableColorType );
    }

    public Color GetColorByPurchaseableColorIndex( PurchaseableColorType purchaseableColorType, int index ) {
        return colorObjectList[index].GetColorByColorType( purchaseableColorType );
    }

    public int GetCurrentLevel( int purchaseableIndex ) {
        return powerupObjectList[purchaseableIndex].GetCurrentLevel();
    }

    public int GetMaxLevel( int purchaseableIndex ) {
        return powerupObjectList[purchaseableIndex].GetMaxLevel();
    }

    /// <summary>
    /// Check if there is any purchaseable that can be bought at the moment so that a notification
    /// can be shown on the game over screen.
    /// </summary>
    public bool IsSomethingPurchaseable() {
        for( int i = 1; i < hatObjectList.Count; i++ ) {
            if( hatObjectList[i].GetPrice() <= totalScore && !hatObjectList[i].IsUnlocked() ) {
                return true;
            }
        }

        for (int i = 1; i < colorObjectList.Count; i++) {
            if (colorObjectList[i].GetPrice() <= totalScore && !colorObjectList[i].IsUnlocked()) {
                return true;
            }
        }

        foreach( PowerupObject powerupObject in powerupObjectList ) {
            if( powerupObject.GetPrice() <= totalScore && !powerupObject.IsUnlocked() ) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Return the number of purchaseables bought at the moment so that the achievement progress
    /// can be updated.
    /// </summary>
    public int GetPurchaseableBoughtCount() {
        int purchaseableBoughtCount = 0;
        foreach( HatObject hatObject in hatObjectList ) {
            if( hatObject.IsUnlocked() ) {
                purchaseableBoughtCount++;
            }
        }

        foreach( ColorObject colorObject in colorObjectList ) {
            if( colorObject.IsUnlocked() ) {
                purchaseableBoughtCount++;
            }
        }
        
        foreach( PowerupObject powerupObject in powerupObjectList ) {
            purchaseableBoughtCount += powerupObject.GetCurrentLevel();
        }
        return purchaseableBoughtCount;
    }

    /// <summary>
    /// Check if every single purchaseable is bought so that the achievement progress can be updated.
    /// </summary>
    public bool IsEverythingUnlocked() {
        int purchaseableCount = 0;
        int purchaseableBoughtCount = 0;
        foreach( HatObject hatObject in hatObjectList ) {
            purchaseableCount++;
            if( hatObject.IsUnlocked() ) {
                purchaseableBoughtCount++;
            }
        }

        foreach( ColorObject colorObject in colorObjectList ) {
            purchaseableCount++;
            if( colorObject.IsUnlocked() ) {
                purchaseableBoughtCount++;
            }
        }

        foreach( PowerupObject powerupObject in powerupObjectList ) {
            purchaseableCount += powerupObject.GetMaxLevel();
            purchaseableBoughtCount += powerupObject.GetCurrentLevel();
        }
        return purchaseableCount == purchaseableBoughtCount;
    }

    /// <summary>
    /// Check if a powerup is fully unlocked so that the achievement progress can be updated.
    /// </summary>
    public bool IsPowerupAtMaxLevel() {
        foreach( PowerupObject powerupObject in powerupObjectList ) {
            if( powerupObject.GetCurrentLevel() == powerupObject.GetMaxLevel() ) {
                return true;
            }
        }
        return false;
    }

    public void SetTotalScore( int totalScore ) {
        this.totalScore = totalScore;
    }

    public void SetHighscore( int highscore ) {
        this.highscore = highscore;
    }

    public PlayerHatTypes GetSelectedHatType() {
        return currentHat;
    }

    public PlayerColorTypes GetSelectedColorType() {
        return currentColor;
    }

    public List<PowerupObject> GetUnlockedPowerups() {
        return powerupObjectList;
    }

    public int GetTotalScore() {
        return totalScore;
    }
}
