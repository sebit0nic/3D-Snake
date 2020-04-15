using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets color for each UI element of Shop Scene according to the current style.
/// </summary>
public class ShopUIColorManager : MonoBehaviour, IUIColorManager {

    public Image[] uiButtonImages;
    public Text[] uiTexts;
    public Image screenTransitionImage;

    public void SetUIColor( Color color ) {
        foreach( Image img in uiButtonImages ) {
            img.color = color;
        }

        foreach( Text text in uiTexts ) {
            text.color = color;
        }

        screenTransitionImage.color = color;
    }
}
