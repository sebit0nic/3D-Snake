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

    public float[] baseColor;
    public float[] planetColor;
    public float[] particleColor;

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

    public string GetName() {
        return name;
    }

    public Color GetColorByColorType(PurchaseableColorType purchaseableColorType) {
        switch ( purchaseableColorType ) {
            case PurchaseableColorType.BASE:
                return new Color(baseColor[0], baseColor[1], baseColor[2], baseColor[3]);
            case PurchaseableColorType.PLANET:
                return new Color(planetColor[0], planetColor[1], planetColor[2], planetColor[3]);
            case PurchaseableColorType.PARTICLE:
                return new Color(particleColor[0], particleColor[1], particleColor[2], particleColor[3]);
            default:
                return Color.white;
        }
    }
}
