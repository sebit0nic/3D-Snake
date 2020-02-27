using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager instance = null;

    private ScreenTransition screenTransition;

    private void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }

    private void Start() {
        screenTransition = GameObject.Find("GUI").GetComponentInChildren<ScreenTransition>();
    }

    public void SwitchScreen(ScreenType screenType) {
        screenTransition.StartScreenTransition((int) screenType);
    }
}
