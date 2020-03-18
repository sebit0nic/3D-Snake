using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseableButton : MonoBehaviour {

    private int sectionIndex, purchaseableIndex;
    private Text nameText;
    private Image progressBar;
    private Image buttonImage;

    private float progressBarValue;

    public void Init(int sectionIndex, int purchaseableIndex, bool isPowerup) {
        this.sectionIndex = sectionIndex;
        this.purchaseableIndex = purchaseableIndex;
        nameText = transform.Find("Name").GetComponent<Text>();
        buttonImage = GetComponent<Image>();

        if (isPowerup) {
            progressBar = transform.Find("Progress Bar").GetComponent<Image>();
        }
    }

    public void SetNameText(SavedData savedData) {
        nameText.text = savedData.GetPurchaseableName(sectionIndex, purchaseableIndex);
    }

    public void SetProgressBar(SavedData savedData) {
        progressBarValue = (float) savedData.GetCurrentLevel(purchaseableIndex) / savedData.GetMaxLevel(purchaseableIndex);
        progressBar.fillAmount = progressBarValue;
    }

    public void SetColor(Color color) {
        buttonImage.color = color;
    }

    public void SetSelected(RectTransform checkMark) {
        checkMark.SetParent(this.transform);
        checkMark.anchoredPosition = new Vector2(-125, 125);
    }
}
