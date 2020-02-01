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
        currentLevel++;
    }

    public bool IsUnlocked() {
        return false;
    }

    public int GetPrice() {
        return price + priceIncreaseFactor * currentLevel;
    }
}
