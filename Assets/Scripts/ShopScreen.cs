using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : MonoBehaviour {

    public GameObject hatSection, colorSection, powerupSection;
    public Image hatButtonImage, colorButtonImage, powerupButtonImage;

    private ShopSection selectedShopSection;
    private PlayerHatTypes selectedHat;
    private PlayerColorTypes selectedColor;
    private PlayerPowerupTypes selectedPowerup;

    private void Start() {
        ShowSection(0);
        selectedShopSection = ShopSection.HATS;
        selectedHat = PlayerHatTypes.TYPE_DEFAULT;
        selectedColor = PlayerColorTypes.COLOR_DEFAULT;
        selectedPowerup = PlayerPowerupTypes.INVINCIBILTY;
    }

    public void ShowSection(int index) {
        selectedShopSection = (ShopSection) index;
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
    }

    public void PurchaseableObjectSelected(int index) {
        switch ( selectedShopSection ) {
            case ShopSection.HATS:
                selectedHat = (PlayerHatTypes) index;
                break;
            case ShopSection.COLORSCHEME:
                selectedColor = (PlayerColorTypes) index;
                break;
            case ShopSection.POWERUPS:
                selectedPowerup = (PlayerPowerupTypes) index;
                break;
        }
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
