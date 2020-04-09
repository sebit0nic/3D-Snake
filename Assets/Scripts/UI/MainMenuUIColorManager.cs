using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIColorManager : MonoBehaviour, IUIColorManager {

    public Image[] uiButtonImages;
    public Image screenTransitionImage;
    public Text privacyPolicyText;

    public void SetUIColor(Color color) {
        foreach ( Image img in uiButtonImages ) {
            img.color = color;
        }

        screenTransitionImage.color = color;
        privacyPolicyText.color = color;
    }
}
