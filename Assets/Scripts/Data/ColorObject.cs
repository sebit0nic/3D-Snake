using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorObject {

    public string name;
    public int price;
    public bool unlocked;
    public bool selected;
    public PlayerColorTypes type;

    public void Unlock() {
        unlocked = true;
    }

    public void SetSelected(bool value) {
        selected = value;
    }

    public bool IsUnlocked() {
        return unlocked;
    }

    public bool IsSelected() {
        return selected;
    }

    public int GetPrice() {
        return price;
    }
}
