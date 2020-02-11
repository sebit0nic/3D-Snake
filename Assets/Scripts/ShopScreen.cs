﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : MonoBehaviour {

    public GameObject hatSection, colorSection, powerupSection;
    public Image hatButtonImage, colorButtonImage, powerupButtonImage;
    public Button buySelectButton;
    public Text buySelectText, totalScoreText;
    public ShopSectionManager shopSectionManager;
    public ScreenTransition screenTransition;

    public GameObject[] hatPreviewModels;

    private int selectedPurchaseableIndex, selectedSectionIndex;
    private SaveLoadManager saveLoadManager;
    private SavedData savedData;
    private bool buyMode;

    private void Start() {
        saveLoadManager = GetComponent<SaveLoadManager>();
        savedData = saveLoadManager.LoadData();
        shopSectionManager.Init(savedData);

        selectedPurchaseableIndex = 0;
        selectedSectionIndex = 0;
        totalScoreText.text = savedData.totalScore.ToString().PadLeft(5, '0');
        hatPreviewModels[(int) savedData.GetSelectedHatType()].SetActive(true);
        ShowSection(selectedSectionIndex);
    }

    public void ShowSection(int index) {
        selectedSectionIndex = index;
        ShopSection selectedShopSection = (ShopSection) index;
        switch ( selectedShopSection ) {
            case ShopSection.HATS:
                DisableAllSections();
                hatSection.SetActive(true);
                hatButtonImage.color = Color.grey;
                break;
            case ShopSection.COLORSCHEME:
                DisableAllSections();
                colorSection.SetActive(true);
                colorButtonImage.color = Color.grey;
                break;
            case ShopSection.POWERUPS:
                DisableAllSections();
                powerupSection.SetActive(true);
                powerupButtonImage.color = Color.grey;
                break;
        }
        PurchaseableObjectSelected(0);
    }

    public void PurchaseableObjectSelected(int index) {
        selectedPurchaseableIndex = index;
        ShopSection selectedShopSection = (ShopSection) selectedSectionIndex;

        if (selectedShopSection == ShopSection.POWERUPS) {
            if (savedData.IsPurchaseableUnlocked(selectedSectionIndex, selectedPurchaseableIndex)) {
                buySelectButton.interactable = false;
            } else if (savedData.totalScore >= savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex)) {
                buySelectButton.interactable = true;
            } else {
                buySelectButton.interactable = false;
            }
            buySelectText.text = "Buy";
            buyMode = true;
        } else {
            if ( savedData.IsPurchaseableUnlocked(selectedSectionIndex, selectedPurchaseableIndex) ) {
                buySelectText.text = "Select";
                buySelectButton.interactable = true;
                buyMode = false;
            } else if ( savedData.totalScore >= savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex) ) {
                buySelectText.text = "Buy";
                buySelectButton.interactable = true;
                buyMode = true;
            } else {
                buySelectText.text = "Buy";
                buySelectButton.interactable = false;
                buyMode = true;
            }
        }
    }

    public void BuySelectPurchaseable() {
        if (buyMode) {
            savedData.UnlockPurchaseable(selectedSectionIndex, selectedPurchaseableIndex);
            totalScoreText.text = savedData.totalScore.ToString().PadLeft(5, '0');
            PurchaseableObjectSelected(selectedPurchaseableIndex);
        } else {
            savedData.SelectPurchaseable(selectedSectionIndex, selectedPurchaseableIndex);
        }
        saveLoadManager.SaveData(savedData);
        shopSectionManager.UpdatePurchaseableSelectButton(savedData, selectedSectionIndex, selectedPurchaseableIndex);
        HideAllHatPreviewModels();
        hatPreviewModels[(int) savedData.GetSelectedHatType()].SetActive(true);
    }

    public void ChangeScreen(int toScreenID) {
        screenTransition.StartScreenTransition(toScreenID);
    }

    private void DisableAllSections() {
        hatSection.SetActive(false);
        colorSection.SetActive(false);
        powerupSection.SetActive(false);
        hatButtonImage.color = Color.white;
        colorButtonImage.color = Color.white;
        powerupButtonImage.color = Color.white;
    }

    private void HideAllHatPreviewModels() {
        foreach (GameObject go in hatPreviewModels) {
            go.SetActive(false);
        }
    }
}
