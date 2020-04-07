using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSectionManager : MonoBehaviour {

    public GameObject hatSection, colorSection, powerupSection;
    public Color buttonPressedColor;
    public RectTransform checkMark;
    public GameObject[] sectionTexts;

    private PurchaseableButton[] hatSelectButtons;
    private PurchaseableButton[] colorSelectButtons;
    private PurchaseableButton[] powerupSelectButtons;
    private ShopSection currentSection;

    public void Init(SavedData savedData) {
        hatSelectButtons = hatSection.GetComponentsInChildren<PurchaseableButton>();
        colorSelectButtons = colorSection.GetComponentsInChildren<PurchaseableButton>();
        powerupSelectButtons = powerupSection.GetComponentsInChildren<PurchaseableButton>();

        for (int i = 0; i < hatSelectButtons.Length; i++) {
            hatSelectButtons[i].Init((int)ShopSection.HATS, i, false);
            hatSelectButtons[i].SetNameText(savedData);
        }

        for (int i = 0; i < colorSelectButtons.Length; i++) {
            colorSelectButtons[i].Init((int) ShopSection.COLORSCHEME, i, false);
            colorSelectButtons[i].SetNameText(savedData);
        }

        for (int i = 0; i < powerupSelectButtons.Length; i++) {
            powerupSelectButtons[i].Init((int) ShopSection.POWERUPS, i, true);
            powerupSelectButtons[i].SetNameText(savedData);
            powerupSelectButtons[i].SetProgressBar(savedData);
        }

        sectionTexts[0].SetActive(true);
    }

    public void UpdateSectionText(int sectionIndex) {
        foreach(GameObject go in sectionTexts) {
            go.SetActive(false);
        }

        sectionTexts[sectionIndex].SetActive(true);
    }

    public void UpdatePurchaseableSelectButton(SavedData savedData, int sectionIndex, int purchaseableIndex) {
        currentSection = (ShopSection) sectionIndex;
        if (currentSection == ShopSection.POWERUPS) {
            powerupSelectButtons[purchaseableIndex].SetProgressBar(savedData);
        }
    }

    public void PurchaseableButtonPressed(int sectionIndex, int purchaseableIndex) {
        currentSection = (ShopSection) sectionIndex;
        switch (currentSection) {
            case ShopSection.HATS:
                for (int i = 0; i < hatSelectButtons.Length; i++) {
                    hatSelectButtons[i].SetColor(Color.white);
                }
                hatSelectButtons[purchaseableIndex].SetColor(buttonPressedColor);
                break;
            case ShopSection.COLORSCHEME:
                for (int i = 0; i < colorSelectButtons.Length; i++) {
                    colorSelectButtons[i].SetColor(Color.white);
                }
                colorSelectButtons[purchaseableIndex].SetColor(buttonPressedColor);
                break;
            case ShopSection.POWERUPS:
                for (int i = 0; i < powerupSelectButtons.Length; i++) {
                    powerupSelectButtons[i].SetColor(Color.white);
                }
                powerupSelectButtons[purchaseableIndex].SetColor(buttonPressedColor);
                break;
        }
    }

    public void PurchaseableSelected(int sectionIndex, int purchaseableIndex) {
        currentSection = (ShopSection) sectionIndex;
        switch ( currentSection ) {
            case ShopSection.HATS:
                hatSelectButtons[purchaseableIndex].SetSelected(checkMark);
                break;
            case ShopSection.COLORSCHEME:
                colorSelectButtons[purchaseableIndex].SetSelected(checkMark);
                break;
        }
    }
}
