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

    public void UnlockHatObject(PlayerHatTypes type) {
        foreach(HatObject hatObject in unlockedHats) {
            if (hatObject.type == type) {
                hatObject.SetUnlocked();
            }
        }
    }

    public void UnlockColorObject(PlayerColorTypes type) {
        foreach ( ColorObject colorObject in unlockedColors ) {
            if ( colorObject.type == type ) {
                colorObject.SetUnlocked();
            }
        }
    }

    public void UnlockPowerupObject(PlayerPowerupTypes type) {
        foreach ( PowerupObject powerupObject in unlockedPowerups ) {
            if ( powerupObject.type == type ) {
                powerupObject.UnlockNextPowerupLevel();
            }
        }
    }
}
