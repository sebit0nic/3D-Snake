using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HatObject {

    public string name { get; }
    public int price { get; }
    public bool unlocked { get; set; }

    public HatObject(string name, int price, bool unlocked) {
        this.name = name;
        this.price = price;
        this.unlocked = unlocked;
    }
}
