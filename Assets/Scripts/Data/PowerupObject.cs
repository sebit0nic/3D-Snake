using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerupObject {

    public string name;
    public int startPrice;
    public int priceIncreaseFactor;
    public int currentLevel;
    public PlayerPowerupTypes powerupType;

}
