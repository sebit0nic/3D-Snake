using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSectionManager : MonoBehaviour {

    public GameObject hatSection, colorSection, powerupSection;

    private PurchaseableButton[] hatSelectButtons;
    private PurchaseableButton[] colorSelectButtons;
    private PurchaseableButton[] powerupSelectButtons;

    public void Init(SavedData savedData) {
        hatSelectButtons = hatSection.GetComponentsInChildren<PurchaseableButton>();
        colorSelectButtons = colorSection.GetComponentsInChildren<PurchaseableButton>();
        powerupSelectButtons = powerupSection.GetComponentsInChildren<PurchaseableButton>();

        for ( int i = 0; i < hatSelectButtons.Length; i++ ) {
            hatSelectButtons[i].Init((int)ShopSection.HATS, i, false);
            hatSelectButtons[i].SetPriceText(savedData);
        }

        for ( int i = 0; i < colorSelectButtons.Length; i++ ) {
            colorSelectButtons[i].Init((int) ShopSection.COLORSCHEME, i, false);
            colorSelectButtons[i].SetPriceText(savedData);
        }

        for ( int i = 0; i < powerupSelectButtons.Length; i++ ) {
            powerupSelectButtons[i].Init((int) ShopSection.POWERUPS, i, true);
            powerupSelectButtons[i].SetPriceText(savedData);
            powerupSelectButtons[i].SetProgressBar(savedData);
        }
    }

    public void UpdatePurchaseableSelectButton(SavedData savedData, int sectionIndex, int purchaseableIndex) {
        ShopSection currentSection = (ShopSection) sectionIndex;
        switch ( currentSection ) {
            case ShopSection.HATS:
                hatSelectButtons[purchaseableIndex].SetPriceText(savedData);
                break;
            case ShopSection.COLORSCHEME:
                colorSelectButtons[purchaseableIndex].SetPriceText(savedData);
                break;
            case ShopSection.POWERUPS:
                powerupSelectButtons[purchaseableIndex].SetPriceText(savedData);
                powerupSelectButtons[purchaseableIndex].SetProgressBar(savedData);
                break;
        }
    }
}
