using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorObject {

    private string name { get; }
    private int price { get; }
    private bool unlocked { get; set; }

    public ColorObject(string name, int price, bool unlocked) {
        this.name = name;
        this.price = price;
        this.unlocked = unlocked;
    }
}
