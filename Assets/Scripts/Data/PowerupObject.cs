using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerupObject {

    public string name;
    public int price;
    public int priceIncreaseFactor;
    public int currentLevel, maxLevel;
    public PlayerPowerupTypes type;

    public void Unlock() {
        if (currentLevel < maxLevel) {
            currentLevel++;
        }
    }

    public bool IsUnlocked() {
        return currentLevel >= maxLevel;
    }

    public int GetPrice() {
        return price + priceIncreaseFactor * currentLevel;
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public int GetMaxLevel() {
        return maxLevel;
    }

    public PlayerPowerupTypes GetPowerupType() {
        return type;
    }

    public string GetName() {
        return name;
    }
}
