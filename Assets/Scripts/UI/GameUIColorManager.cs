using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIColorManager : MonoBehaviour, IUIColorManager {

    public Image[] uiButtonImages;
    public Text[] uiButtonTexts;
    public Image screenTransitionImage;
    public Image pauseScreenImage;

    public void SetUIColor(Color color) {
        foreach(Image img in uiButtonImages) {
            img.color = color;
        }

        screenTransitionImage.color = color;
        pauseScreenImage.color = new Color(color.r, color.g, color.b, 0.5f);
    }
}
