using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI changes for purchaseable buttons.
/// </summary>
public class PurchaseableButton : MonoBehaviour {

    public Vector2 checkMarkPositionOffset;

    private int sectionIndex, purchaseableIndex;
    private Text nameText;
    private Image progressBar;
    private Image buttonImage;

    private float progressBarValue;
    private const string nameTextKey = "Name";
    private const string progressBarKey = "Progress Bar";

    public void Init( int sectionIndex, int purchaseableIndex, bool isPowerup ) {
        this.sectionIndex = sectionIndex;
        this.purchaseableIndex = purchaseableIndex;
        nameText = transform.Find( nameTextKey ).GetComponent<Text>();
        buttonImage = GetComponent<Image>();

        if( isPowerup ) {
            progressBar = transform.Find( progressBarKey ).GetComponent<Image>();
        }
    }

    /// <summary>
    /// Set the name for powerup purchaseable buttons.
    /// </summary>
    public void SetNameText( SavedData savedData ) {
        nameText.text = savedData.GetPurchaseableName(sectionIndex, purchaseableIndex);
    }

    /// <summary>
    /// Update the fill amount of powerup purchaseable buttons progress bar.
    /// </summary>
    public void SetProgressBar( SavedData savedData ) {
        progressBarValue = (float) savedData.GetCurrentLevel( purchaseableIndex ) / savedData.GetMaxLevel( purchaseableIndex );
        progressBar.fillAmount = progressBarValue;
    }

    /// <summary>
    /// Set the color of a purchaseable button after the button has been pressed.
    /// </summary>
    public void SetColor( Color color ) {
        buttonImage.color = color;
    }

    /// <summary>
    /// Reset the position of the checkmark when a purchaseable has been selected.
    /// </summary>
    public void SetSelected( RectTransform checkMark ) {
        checkMark.SetParent( this.transform );
        checkMark.anchoredPosition = checkMarkPositionOffset;
    }
}
