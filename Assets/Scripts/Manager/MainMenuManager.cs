using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager instance = null;

    private ScreenTransition screenTransition;
    private SaveLoadManager saveLoadManager;
    private StyleManager styleManager;
    private SavedData savedData;

    private void Awake() {
        if ( instance == null ) {
            instance = this;
        }
    }

    private void Start() {
        screenTransition = GameObject.Find("GUI").GetComponentInChildren<ScreenTransition>();
        saveLoadManager = GetComponent<SaveLoadManager>();
        savedData = saveLoadManager.LoadData();
        styleManager = GetComponentInChildren<StyleManager>();
        styleManager.Init(savedData);
    }

    public void SwitchScreen(ScreenType screenType) {
        screenTransition.StartScreenTransition((int) screenType);
    }
}
