using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : MonoBehaviour {

    public GameObject hatSection, colorSection, powerupSection;
    public Image hatButtonImage, colorButtonImage, powerupButtonImage;
    public Image hatButtonIcon, colorButtonIcon, powerupButtonIcon;
    public Button buySelectButton;
    public Text buyText, priceText, selectText, totalScoreText;
    public ShopSectionManager shopSectionManager;
    public ScreenTransition screenTransition;
    public GameObject[] hatPreviewModels;
    public Animator hatAnimator;

    private int selectedPurchaseableIndex, selectedSectionIndex;
    private SaveLoadManager saveLoadManager;
    private SavedData savedData;
    private StyleManager styleManager;
    private PlayStoreManager playStoreManager;
    private AchievementManager achievementManager;
    private bool buyMode;

    private void Start() {
        saveLoadManager = GetComponentInChildren<SaveLoadManager>();
        savedData = saveLoadManager.LoadData();
        shopSectionManager.Init(savedData);
        styleManager = GetComponentInChildren<StyleManager>();
        styleManager.Init(savedData);
        playStoreManager = GetComponentInChildren<PlayStoreManager>();
        playStoreManager.Init();
        achievementManager = GetComponentInChildren<AchievementManager>();

        selectedPurchaseableIndex = 0;
        selectedSectionIndex = 0;
        totalScoreText.text = savedData.totalScore.ToString().PadLeft(5, '0');
        hatPreviewModels[(int) savedData.GetSelectedHatType()].SetActive(true);
        shopSectionManager.PurchaseableSelected(selectedSectionIndex, (int)savedData.GetSelectedHatType());
        ShowSection(selectedSectionIndex);
        
        achievementManager.NotifyPurchaseableBought(savedData.GetPurchaseableBoughtCount());
        if ( savedData.IsPowerupAtMaxLevel() ) {
            achievementManager.NotifyPowerupAtMaxLevel();
        }
        if ( savedData.IsEverythingUnlocked() ) {
            achievementManager.NotifyEverythingUnlocked();
        }
    }

    public void ShowSection(int index) {
        selectedSectionIndex = index;
        ShopSection selectedShopSection = (ShopSection) index;
        switch ( selectedShopSection ) {
            case ShopSection.HATS:
                DisableAllSections();
                hatSection.SetActive(true);
                hatButtonImage.color = savedData.GetColorByPurchaseableColorType(PurchaseableColorType.BASE);
                hatButtonIcon.color = Color.white;
                shopSectionManager.PurchaseableSelected(selectedSectionIndex, (int) savedData.GetSelectedHatType());
                PurchaseableObjectSelected((int) savedData.GetSelectedHatType());
                break;
            case ShopSection.COLORSCHEME:
                DisableAllSections();
                colorSection.SetActive(true);
                colorButtonImage.color = savedData.GetColorByPurchaseableColorType(PurchaseableColorType.BASE);
                colorButtonIcon.color = Color.white;
                shopSectionManager.PurchaseableSelected(selectedSectionIndex, (int) savedData.GetSelectedColorType());
                PurchaseableObjectSelected((int)savedData.GetSelectedColorType());
                break;
            case ShopSection.POWERUPS:
                DisableAllSections();
                powerupSection.SetActive(true);
                powerupButtonImage.color = savedData.GetColorByPurchaseableColorType(PurchaseableColorType.BASE);
                powerupButtonIcon.color = Color.white;
                PurchaseableObjectSelected(0);
                break;
        }

        HideAllHatPreviewModels();
        hatPreviewModels[(int)savedData.GetSelectedHatType()].SetActive(true);
        styleManager.Init(savedData);
    }

    public void PurchaseableObjectSelected(int index) {
        selectedPurchaseableIndex = index;
        shopSectionManager.PurchaseableButtonPressed(selectedSectionIndex, selectedPurchaseableIndex);
        ShopSection selectedShopSection = (ShopSection) selectedSectionIndex;

        if (selectedShopSection == ShopSection.POWERUPS) {
            if (savedData.IsPurchaseableUnlocked(selectedSectionIndex, selectedPurchaseableIndex)) {
                buySelectButton.interactable = false;
                buyText.gameObject.SetActive(true);
                priceText.gameObject.SetActive(false);
                selectText.gameObject.SetActive(false);
            } else if (savedData.totalScore >= savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex)) {
                buySelectButton.interactable = true;
                priceText.text = savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex).ToString();
                buyText.gameObject.SetActive(true);
                priceText.gameObject.SetActive(true);
                selectText.gameObject.SetActive(false);
            } else {
                buySelectButton.interactable = false;
                priceText.text = savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex).ToString();
                buyText.gameObject.SetActive(true);
                priceText.gameObject.SetActive(true);
                selectText.gameObject.SetActive(false);
            }
            buyMode = true;
        } else {
            if (savedData.IsPurchaseableUnlocked(selectedSectionIndex, selectedPurchaseableIndex)) {
                buySelectButton.interactable = true;
                priceText.text = savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex).ToString();
                buyMode = false;
                buyText.gameObject.SetActive(false);
                priceText.gameObject.SetActive(false);
                selectText.gameObject.SetActive(true);
            } else if ( savedData.totalScore >= savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex) ) {
                buySelectButton.interactable = true;
                priceText.text = savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex).ToString();
                buyMode = true;
                buyText.gameObject.SetActive(true);
                priceText.gameObject.SetActive(true);
                selectText.gameObject.SetActive(false);
            } else {
                buySelectButton.interactable = false;
                priceText.text = savedData.GetPurchaseablePrice(selectedSectionIndex, selectedPurchaseableIndex).ToString();
                buyMode = true;
                buyText.gameObject.SetActive(true);
                priceText.gameObject.SetActive(true);
                selectText.gameObject.SetActive(false);
            }
        }

        switch ( selectedShopSection ) {
            case ShopSection.HATS:
                HideAllHatPreviewModels();
                hatPreviewModels[selectedPurchaseableIndex].SetActive(true);
                break;
            case ShopSection.COLORSCHEME:
                styleManager.InitByIndex(savedData, selectedPurchaseableIndex);
                break;
        }
    }

    public void BuySelectPurchaseable() {
        if (buyMode) {
            savedData.UnlockPurchaseable(selectedSectionIndex, selectedPurchaseableIndex);
            totalScoreText.text = savedData.totalScore.ToString().PadLeft(5, '0');
            PurchaseableObjectSelected(selectedPurchaseableIndex);

            achievementManager.NotifyPurchaseableBought(savedData.GetPurchaseableBoughtCount());
            if (savedData.IsPowerupAtMaxLevel()) {
                achievementManager.NotifyPowerupAtMaxLevel();
            }
            if (savedData.IsEverythingUnlocked()) {
                achievementManager.NotifyEverythingUnlocked();
            }
        }
        if ((ShopSection)selectedSectionIndex != ShopSection.POWERUPS) {
            savedData.SelectPurchaseable(selectedSectionIndex, selectedPurchaseableIndex);
            shopSectionManager.PurchaseableSelected(selectedSectionIndex, selectedPurchaseableIndex);
        }

        saveLoadManager.SaveData(savedData);
        shopSectionManager.UpdatePurchaseableSelectButton(savedData, selectedSectionIndex, selectedPurchaseableIndex);
    }

    public void ChangeScreen(int toScreenID) {
        screenTransition.StartScreenTransition(toScreenID);
    }

    public void ShowHatAnimation() {
        hatAnimator.SetTrigger("OnShow");
    }

    private void DisableAllSections() {
        hatSection.SetActive(false);
        colorSection.SetActive(false);
        powerupSection.SetActive(false);
        hatButtonImage.color = Color.white;
        colorButtonImage.color = Color.white;
        powerupButtonImage.color = Color.white;
        hatButtonIcon.color = savedData.GetColorByPurchaseableColorType(PurchaseableColorType.BASE);
        colorButtonIcon.color = savedData.GetColorByPurchaseableColorType(PurchaseableColorType.BASE);
        powerupButtonIcon.color = savedData.GetColorByPurchaseableColorType(PurchaseableColorType.BASE);
    }

    private void HideAllHatPreviewModels() {
        foreach (GameObject go in hatPreviewModels) {
            go.SetActive(false);
        }
    }
}
