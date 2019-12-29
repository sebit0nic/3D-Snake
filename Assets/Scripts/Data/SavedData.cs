using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedData {

    public List<HatObject> unlockedHats = new List<HatObject>();
    public List<ColorObject> unlockedColors = new List<ColorObject>();
    public List<int> unlockedPowerups = new List<int>();
    public int highscore;
    public int totalScore;
    public PlayerHatTypes currentHat;
    public PlayerColorTypes currentColor;

    public SavedData() {
        //TODO: fill unlockedHats, unlockedColors and unlockedPowerups with standard values
        highscore = 0;
        totalScore = 0;
        currentHat = PlayerHatTypes.TYPE_DEFAULT;
        currentColor = PlayerColorTypes.COLOR_DEFAULT;
    }
}
