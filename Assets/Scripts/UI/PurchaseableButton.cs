using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseableButton : MonoBehaviour {

    private int sectionIndex, purchaseableIndex;
    private Text nameText;
    private Image progressBar;

    private float progressBarValue;

    public void Init(int sectionIndex, int purchaseableIndex, bool isPowerup) {
        this.sectionIndex = sectionIndex;
        this.purchaseableIndex = purchaseableIndex;
        nameText = transform.Find("Name").GetComponent<Text>();

        if (isPowerup) {
            progressBar = transform.Find("Progress Bar").GetComponent<Image>();
        }
    }

    public void SetNameText(SavedData savedData) {
        nameText.text = savedData.GetPurchaseableName(sectionIndex, purchaseableIndex);
    }

    public void SetProgressBar(SavedData savedData) {
        progressBarValue = (float) savedData.GetCurrentLevel(sectionIndex, purchaseableIndex) / savedData.GetMaxLevel(sectionIndex, purchaseableIndex);
        progressBar.fillAmount = progressBarValue;
    }
}
