using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : MonoBehaviour {

    public GameObject hatSection, colorSection, powerupSection;
    public Image hatButtonImage, colorButtonImage, powerupButtonImage;
    public Button buySelectButton;
    public Text buySelectText, totalScoreText;
    public HatSectionManager hatSectionManager;

    private int selectedPurchaseableIndex, selectedSectionIndex;
    private SaveLoadManager saveLoadManager;
    private SavedData savedData;
    private bool buyMode;

    private void Start() {
        saveLoadManager = GetComponent<SaveLoadManager>();
        savedData = saveLoadManager.LoadData();
        hatSectionManager.Init(savedData);

        selectedPurchaseableIndex = 0;
        selectedSectionIndex = 0;
        totalScoreText.text = savedData.totalScore.ToString().PadLeft(5, '0');
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
        //TODO: powerup objects need different handling, so check for that
        if (savedData.IsPurchaseableUnlocked(selectedSectionIndex, selectedPurchaseableIndex)) {
            buySelectText.text = "Select";
            buySelectButton.interactable = true;
            buyMode = false;
        } else if (savedData.totalScore >= savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex)) {
            buySelectText.text = "Buy";
            buySelectButton.interactable = true;
            buyMode = true;
        } else {
            buySelectText.text = "Buy";
            buySelectButton.interactable = false;
            buyMode = true;
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
        hatSectionManager.UpdateHatSelectButton(savedData, selectedPurchaseableIndex);
    }

    private void DisableAllSections() {
        hatSection.SetActive(false);
        colorSection.SetActive(false);
        powerupSection.SetActive(false);
        hatButtonImage.color = Color.white;
        colorButtonImage.color = Color.white;
        powerupButtonImage.color = Color.white;
    }
}
