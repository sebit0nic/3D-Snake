using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager instance = null;
    public Toggle soundButton, cameraButton;

    private ScreenTransition screenTransition;
    private SaveLoadManager saveLoadManager;
    private StyleManager styleManager;
    private SavedData savedData;
    private PlayStoreManager playStoreManager;

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
        playStoreManager = GetComponentInChildren<PlayStoreManager>();

        soundButton.isOn = saveLoadManager.GetSoundStatus() != 0;
        cameraButton.isOn = saveLoadManager.GetCameraStatus() != 0;

        playStoreManager.Init();
        playStoreManager.SignIn();
    }

    public void SwitchScreen(ScreenType screenType) {
        screenTransition.StartScreenTransition((int) screenType);
    }

    public void ToggleButtonSoundPressed() {
        saveLoadManager.SetSoundStatus(soundButton.isOn ? 1 : 0);
    }

    public void ToggleButtonCameraPressed() {
        saveLoadManager.SetCameraStatus(cameraButton.isOn ? 1 : 0);
    }
}
