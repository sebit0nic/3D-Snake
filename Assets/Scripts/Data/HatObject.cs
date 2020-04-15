﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data for a hat object
/// </summary>
[System.Serializable]
public class HatObject {

    public string name;
    public int price;
    public bool unlocked;
    public PlayerHatTypes type;

    public void Unlock() {
        unlocked = true;
    }

    public bool IsUnlocked() {
        return unlocked;
    }

    public int GetPrice() {
        return price;
    }

    public string GetName() {
        return name;
    }
}
