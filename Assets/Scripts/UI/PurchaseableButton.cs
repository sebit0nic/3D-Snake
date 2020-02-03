﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseableButton : MonoBehaviour {

    private int sectionIndex, purchaseableIndex;
    private Text priceText;
    private Image progressBar;

    private float progressBarValue;

    public void Init(int sectionIndex, int purchaseableIndex, bool isPowerup) {
        this.sectionIndex = sectionIndex;
        this.purchaseableIndex = purchaseableIndex;
        priceText = transform.Find("Price").GetComponent<Text>();

        if (isPowerup) {
            progressBar = transform.Find("Progress Bar").GetComponent<Image>();
        }
    }

    public void SetPriceText(SavedData savedData) {
        if ( savedData.IsPurchaseableUnlocked(sectionIndex, purchaseableIndex) ) {
            priceText.text = "";
        } else {
            priceText.text = savedData.GetPurchaseablePrice(sectionIndex, purchaseableIndex).ToString().PadLeft(4, '0');
        }
    }

    public void SetProgressBar(SavedData savedData) {
        progressBarValue = (float) savedData.GetCurrentLevel(sectionIndex, purchaseableIndex) / savedData.GetMaxLevel(sectionIndex, purchaseableIndex);
        progressBar.fillAmount = progressBarValue;
    }
}
