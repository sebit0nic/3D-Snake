using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets the color for each UI element of the Game Scene according to the current style.
/// </summary>
public class GameUIColorManager : MonoBehaviour, IUIColorManager {

    public Image[] uiButtonImages;
    public Text[] uiButtonTexts;
    public Image screenTransitionImage;
    public Image pauseScreenImage;

    private const float pauseScreenAlpha = 0.5f;

    public void SetUIColor( Color color ) {
        foreach( Image img in uiButtonImages ) {
            img.color = color;
        }

        foreach( Text text in uiButtonTexts ) {
            text.color = color;
        }

        screenTransitionImage.color = color;
        pauseScreenImage.color = new Color( color.r, color.g, color.b, pauseScreenAlpha );
    }
}
