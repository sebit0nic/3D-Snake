using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatSectionManager : MonoBehaviour {

    private HatSelectButton[] hatSelectButtons;

    public void Init(SavedData savedData) {
        hatSelectButtons = GetComponentsInChildren<HatSelectButton>();

        for (int i = 0; i < hatSelectButtons.Length; i++) {
            hatSelectButtons[i].SetPriceText(savedData.IsPurchaseableUnlocked(0, i), savedData.GetPurchaseablePrice(0, i));
        }
    }

    public void UpdateHatSelectButton(SavedData savedData, int index) {
        hatSelectButtons[index].SetPriceText(savedData.IsPurchaseableUnlocked(0, index), savedData.GetPurchaseablePrice(0, index));
    }
}
