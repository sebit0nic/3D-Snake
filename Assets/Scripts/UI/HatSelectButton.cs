using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatSelectButton : MonoBehaviour {

    private Text priceText;

    private void Start() {
        priceText = GetComponentInChildren<Text>();
    }

    public void SetPriceText(bool unlocked, int price) {
        if (unlocked) {
            priceText.text = "";
        } else {
            priceText.text = price.ToString().PadLeft(4, '0');
        }
    }
}
