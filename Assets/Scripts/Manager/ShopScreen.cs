using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles communication between different managers in the shop scene.
/// </summary>
public class ShopScreen : MonoBehaviour {

    public static ShopScreen instance = null;

    public GameObject hatSection, colorSection, powerupSection;
    public Image hatButtonImage, colorButtonImage, powerupButtonImage;
    public Image hatButtonIcon, colorButtonIcon, powerupButtonIcon;
    public Button buySelectButton;
    public Text buyText, priceText, selectText, totalScoreText;
    public ShopSectionManager shopSectionManager;
    public PowerupPreview powerupPreview;
    public ScreenTransition screenTransition;
    public GameObject[] hatPreviewModels;
    public Animator hatAnimator;

    private ShopSection selectedShopSection;
    private int selectedPurchaseableIndex, selectedSectionIndex;
    private SaveLoadManager saveLoadManager;
    private SavedData savedData;
    private StyleManager styleManager;
    private PlayStoreManager playStoreManager;
    private AchievementManager achievementManager;
    private SoundManager soundManager;
    private bool buyMode;
    private string totalScoreString;

    private const char padChar = '0';
    private const int padLeftAmount = 5;
    private const string hatAnimatorTrigger = "OnShow";
    private const int totalScoreDecreaseSpeed = 20;

    private void Awake() {
        if( instance == null ) {
            instance = this;
        }
    }

    private void Start() {
        saveLoadManager = GetComponentInChildren<SaveLoadManager>();
        savedData = saveLoadManager.LoadData();
        shopSectionManager.Init( savedData );
        styleManager = GetComponentInChildren<StyleManager>();
        styleManager.Init( savedData );
        playStoreManager = GetComponentInChildren<PlayStoreManager>();
        playStoreManager.Init();
        achievementManager = GetComponentInChildren<AchievementManager>();
        soundManager = GetComponentInChildren<SoundManager>();
        soundManager.Init( saveLoadManager );
        soundManager.PlaySound( SoundEffectType.SOUND_AMBIENCE, false );

        selectedPurchaseableIndex = 0;
        selectedSectionIndex = 0;
        totalScoreText.text = savedData.totalScore.ToString().PadLeft( padLeftAmount, padChar );
        hatPreviewModels[(int) savedData.GetSelectedHatType()].SetActive( true );
        shopSectionManager.PurchaseableSelected( selectedSectionIndex, (int) savedData.GetSelectedHatType() );
        ShowSection( selectedSectionIndex );
        CheckAchievementConditions();
    }

    /// <summary>
    /// Show the selected section with its purchaseables.
    /// </summary>
    public void ShowSection( int index ) {
        selectedSectionIndex = index;
        selectedShopSection = (ShopSection) index;
        DisableAllSections();
        powerupPreview.DisableAllPreviews();

        switch( selectedShopSection ) {
            case ShopSection.HATS:
                hatSection.SetActive( true );
                hatButtonImage.color = savedData.GetColorByPurchaseableColorType( PurchaseableColorType.BASE );
                hatButtonIcon.color = Color.white;
                shopSectionManager.PurchaseableSelected( selectedSectionIndex, (int) savedData.GetSelectedHatType() );
                PurchaseableObjectSelected( (int) savedData.GetSelectedHatType() );
                break;
            case ShopSection.COLORSCHEME:
                colorSection.SetActive( true );
                colorButtonImage.color = savedData.GetColorByPurchaseableColorType( PurchaseableColorType.BASE );
                colorButtonIcon.color = Color.white;
                shopSectionManager.PurchaseableSelected( selectedSectionIndex, (int) savedData.GetSelectedColorType() );
                PurchaseableObjectSelected( (int) savedData.GetSelectedColorType() );
                break;
            case ShopSection.POWERUPS:
                powerupSection.SetActive( true );
                powerupButtonImage.color = savedData.GetColorByPurchaseableColorType( PurchaseableColorType.BASE );
                powerupButtonIcon.color = Color.white;
                PurchaseableObjectSelected( 0 );
                powerupPreview.Show( PlayerPowerupTypes.INVINCIBILTY );
                break;
        }

        shopSectionManager.UpdateSectionText( index );
        HideAllHatPreviewModels();
        hatPreviewModels[(int) savedData.GetSelectedHatType()].SetActive( true );
        styleManager.Init( savedData );
    }

    /// <summary>
    /// Purchaseable object has been clicked on by player.
    /// </summary>
    public void PurchaseableObjectSelected( int index ) {
        selectedPurchaseableIndex = index;
        shopSectionManager.PurchaseableButtonPressed( selectedSectionIndex, selectedPurchaseableIndex );
        selectedShopSection = (ShopSection) selectedSectionIndex;

        // Pretend like this terrible section doesn't exist...
        if( selectedShopSection == ShopSection.POWERUPS ) {
            if( savedData.IsPurchaseableUnlocked( selectedSectionIndex, selectedPurchaseableIndex ) ) {
                buySelectButton.interactable = false;
                buyText.gameObject.SetActive( true );
                priceText.gameObject.SetActive( false );
                selectText.gameObject.SetActive( false );
            } else if( savedData.totalScore >= savedData.GetPurchaseablePrice( selectedSectionIndex, selectedPurchaseableIndex ) ) {
                buySelectButton.interactable = true;
                priceText.text = savedData.GetPurchaseablePrice( selectedSectionIndex, selectedPurchaseableIndex ).ToString();
                buyText.gameObject.SetActive( true );
                priceText.gameObject.SetActive( true );
                selectText.gameObject.SetActive( false );
            } else {
                buySelectButton.interactable = false;
                priceText.text = savedData.GetPurchaseablePrice( selectedSectionIndex, selectedPurchaseableIndex ).ToString();
                buyText.gameObject.SetActive( true );
                priceText.gameObject.SetActive( true );
                selectText.gameObject.SetActive( false );
            }
            buyMode = true;
        } else {
            if( savedData.IsPurchaseableUnlocked( selectedSectionIndex, selectedPurchaseableIndex ) ) {
                buySelectButton.interactable = true;
                priceText.text = savedData.GetPurchaseablePrice( selectedSectionIndex, selectedPurchaseableIndex ).ToString();
                buyMode = false;
                buyText.gameObject.SetActive( false );
                priceText.gameObject.SetActive( false );
                selectText.gameObject.SetActive( true );
            } else if( savedData.totalScore >= savedData.GetPurchaseablePrice( selectedSectionIndex, selectedPurchaseableIndex ) ) {
                buySelectButton.interactable = true;
                priceText.text = savedData.GetPurchaseablePrice( selectedSectionIndex, selectedPurchaseableIndex ).ToString();
                buyMode = true;
                buyText.gameObject.SetActive( true );
                priceText.gameObject.SetActive( true );
                selectText.gameObject.SetActive( false );
            } else {
                buySelectButton.interactable = false;
                priceText.text = savedData.GetPurchaseablePrice( selectedSectionIndex, selectedPurchaseableIndex ).ToString();
                buyMode = true;
                buyText.gameObject.SetActive( true );
                priceText.gameObject.SetActive( true );
                selectText.gameObject.SetActive( false );
            }
        }

        switch( selectedShopSection ) {
            case ShopSection.HATS:
                HideAllHatPreviewModels();
                hatPreviewModels[selectedPurchaseableIndex].SetActive( true );
                break;
            case ShopSection.COLORSCHEME:
                styleManager.InitByIndex( savedData, selectedPurchaseableIndex );
                break;
        }
    }

    /// <summary>
    /// Buy or select currently selected purchaseable.
    /// </summary>
    public void BuySelectPurchaseable() {
        if( buyMode ) {
            StartCoroutine( OnShowTotalScoreAfterBuy( savedData.GetTotalScore(), savedData.GetTotalScore() - savedData.GetPurchaseablePrice( selectedSectionIndex, selectedPurchaseableIndex ) ) );
            savedData.UnlockPurchaseable( selectedSectionIndex, selectedPurchaseableIndex );
            PurchaseableObjectSelected( selectedPurchaseableIndex );
            CheckAchievementConditions();
        }
        if( (ShopSection) selectedSectionIndex != ShopSection.POWERUPS ) {
            savedData.SelectPurchaseable( selectedSectionIndex, selectedPurchaseableIndex );
            shopSectionManager.PurchaseableSelected( selectedSectionIndex, selectedPurchaseableIndex );
        }

        saveLoadManager.SaveData( savedData );
        shopSectionManager.UpdatePurchaseableSelectButton( savedData, selectedSectionIndex, selectedPurchaseableIndex );
    }

    /// <summary>
    /// Change the scene to something else.
    /// </summary>
    public void ChangeScreen( int toScreenID ) {
        soundManager.PlaySound( SoundEffectType.SOUND_BUTTON, false );
        screenTransition.StartScreenTransition( toScreenID );
    }

    /// <summary>
    /// Player has selected another hat so let it fall gently on the snakes head.
    /// </summary>
    public void ShowHatAnimation() {
        hatAnimator.SetTrigger( hatAnimatorTrigger );
    }

    /// <summary>
    /// One of the powerup purchaseables has been selected so show the right preview of it.
    /// </summary>
    public void PowerupButtonPressed( int index ) {
        powerupPreview.DisableAllPreviews();
        powerupPreview.Show( (PlayerPowerupTypes) index );
    }

    /// <summary>
    /// Disable all shop sections.
    /// </summary>
    private void DisableAllSections() {
        hatSection.SetActive( false );
        colorSection.SetActive( false );
        powerupSection.SetActive( false );
        hatButtonImage.color = Color.white;
        colorButtonImage.color = Color.white;
        powerupButtonImage.color = Color.white;
        hatButtonIcon.color = savedData.GetColorByPurchaseableColorType( PurchaseableColorType.BASE );
        colorButtonIcon.color = savedData.GetColorByPurchaseableColorType( PurchaseableColorType.BASE );
        powerupButtonIcon.color = savedData.GetColorByPurchaseableColorType( PurchaseableColorType.BASE );
    }

    /// <summary>
    /// Hide all hat preview models.
    /// </summary>
    private void HideAllHatPreviewModels() {
        foreach( GameObject go in hatPreviewModels ) {
            go.SetActive( false );
        }
    }

    /// <summary>
    /// Check conditions of the achievements, maybe we unlocked something.
    /// </summary>
    private void CheckAchievementConditions() {
        achievementManager.NotifyPurchaseableBought( savedData.GetPurchaseableBoughtCount() );
        if( savedData.IsPowerupAtMaxLevel() ) {
            achievementManager.NotifyPowerupAtMaxLevel();
        }
        if( savedData.IsEverythingUnlocked() ) {
            achievementManager.NotifyEverythingUnlocked();
        }
    }

    /// <summary>
    /// Count down totalScore text after a purchase.
    /// </summary>
    private IEnumerator OnShowTotalScoreAfterBuy( int totalScore, int newTotalScore ) {
        totalScoreString = totalScore.ToString();
        totalScoreText.text = totalScoreString.PadLeft( padLeftAmount, padChar );
        
        while( totalScore > newTotalScore ) {
            totalScore -= totalScoreDecreaseSpeed;
            totalScoreString = totalScore.ToString();
            totalScoreText.text = totalScoreString.PadLeft( padLeftAmount, padChar );
            soundManager.PlaySound( SoundEffectType.SOUND_TOTAL_POINTS_UP, false );
            yield return new WaitForFixedUpdate();
        }

        totalScoreString = newTotalScore.ToString();
        totalScoreText.text = totalScoreString.PadLeft( padLeftAmount, padChar );
    }
}
