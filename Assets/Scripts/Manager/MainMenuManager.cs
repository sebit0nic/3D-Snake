﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles communication between different managers in the main menu.
/// </summary>
public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager instance = null;
    public ScreenTransition screenTransition;
    public Toggle soundButton, cameraButton, orientationButton;
    public Toggle soundButton_portrait, cameraButton_portrait, orientationButton_portrait;
    public GameObject landscapeCamera, portraitCamera;
    public GameObject landscapeGUI, portraitGUI;
    
    private SaveLoadManager saveLoadManager;
    private StyleManager styleManager;
    private SavedData savedData;
    private PlayStoreManager playStoreManager;
    private SoundManager soundManager;
    private WaitForSeconds screenOrientationWait;

    private const string privacyPolicyLink = "https://www.privacypolicygenerator.info/live.php?token=XHLfKYMTFcs0emh5p7caBpGhSZNSlEH2";
    private const float screenRotationDelay = 0.75f;

    private void Awake() {
        if( instance == null ) {
            instance = this;
        }
    }

    private void Start() {
        saveLoadManager = GetComponentInChildren<SaveLoadManager>();
        savedData = saveLoadManager.LoadData();
        styleManager = GetComponentInChildren<StyleManager>();
        styleManager.Init( savedData );
        playStoreManager = GetComponentInChildren<PlayStoreManager>();
        soundManager = GetComponentInChildren<SoundManager>();
        soundManager.Init( saveLoadManager );

        soundButton.isOn = saveLoadManager.GetSoundStatus() != 0;
        cameraButton.isOn = saveLoadManager.GetCameraStatus() != 0;
        orientationButton.isOn = saveLoadManager.GetScreenOrientationStatus() != 0;
        soundButton_portrait.isOn = saveLoadManager.GetSoundStatus() != 0;
        cameraButton_portrait.isOn = saveLoadManager.GetCameraStatus() != 0;
        orientationButton_portrait.isOn = saveLoadManager.GetScreenOrientationStatus() != 0;

        playStoreManager.Init();
        playStoreManager.SignIn();

        screenOrientationWait = new WaitForSeconds( screenRotationDelay );

        if( saveLoadManager.GetScreenOrientationStatus() == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            Screen.orientation = ScreenOrientation.Landscape;
            landscapeCamera.SetActive( true );
            landscapeGUI.SetActive( true );
        } else {
            Screen.orientation = ScreenOrientation.Portrait;
            portraitCamera.SetActive( true );
            portraitGUI.SetActive( true );
        }
    }

    /// <summary>
    /// Change the scene to something else.
    /// </summary>
    public void SwitchScreen( ScreenType screenType ) {
        if( screenType == ScreenType.SHOP_MENU ) {
            if( saveLoadManager.GetScreenOrientationStatus() == (int) ScreenOrientationStatus.SCREEN_PORTRAIT ) {
                screenType = ScreenType.SHOP_MENU_PORTRAIT;
            }
        }
        screenTransition.StartScreenTransition( (int) screenType );
    }

    /// <summary>
    /// Toggle the sound on or off.
    /// </summary>
    public void ToggleButtonSoundPressed() {
        if( saveLoadManager.GetScreenOrientationStatus() == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            saveLoadManager.SetSoundStatus( soundButton.isOn ? 1 : 0 );
        } else {
            saveLoadManager.SetSoundStatus( soundButton_portrait.isOn ? 1 : 0 );
        }
        
        soundManager.Init( saveLoadManager );
        soundManager.PlaySound( SoundEffectType.SOUND_BUTTON, false );

        if( (SoundStatus) saveLoadManager.GetSoundStatus() == SoundStatus.SOUND_OFF ) {
            soundManager.StopAllSound();
        } else {
            soundManager.ResumeMusicLoop();
        }
    }

    /// <summary>
    /// Toggle the camera rotation on or off.
    /// </summary>
    public void ToggleButtonCameraPressed() {
        if( saveLoadManager.GetScreenOrientationStatus() == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            saveLoadManager.SetCameraStatus( cameraButton.isOn ? 1 : 0 );
        } else {
            saveLoadManager.SetCameraStatus( cameraButton_portrait.isOn ? 1 : 0 );
        }
        
        soundManager.PlaySound( SoundEffectType.SOUND_BUTTON, false );
    }

    /// <summary>
    /// Toggle the screen orientation between landscape and portrait.
    /// </summary>
    public void ToggleScreenRotation() {
        StartCoroutine( WaitForScreenRotation() );

        soundManager.PlaySound( SoundEffectType.SOUND_BUTTON, false );
    }

    /// <summary>
    /// Open the privacy policy in a web-browser.
    /// </summary>
    public void OpenPrivacyPolicyWebsite() {
        Application.OpenURL( privacyPolicyLink );
    }

    /// <summary>
    /// Coroutine to wait a few milliseconds to let the screen adjust to the new orientation before showing the GUI again.
    /// </summary>
    private IEnumerator WaitForScreenRotation() {
        if( saveLoadManager.GetScreenOrientationStatus() == (int) ScreenOrientationStatus.SCREEN_LANDSCAPE ) {
            Screen.orientation = ScreenOrientation.Portrait;
            saveLoadManager.SetScreenOrientationStatus( (int) ScreenOrientationStatus.SCREEN_PORTRAIT );
            soundButton_portrait.isOn = saveLoadManager.GetSoundStatus() != 0;
            cameraButton_portrait.isOn = saveLoadManager.GetCameraStatus() != 0;
            orientationButton_portrait.isOn = saveLoadManager.GetScreenOrientationStatus() != 0;
            landscapeCamera.SetActive( false );
            landscapeGUI.SetActive( false );
            portraitCamera.SetActive( true );

            yield return screenOrientationWait;

            portraitGUI.SetActive( true );
        } else {
            Screen.orientation = ScreenOrientation.Landscape;
            saveLoadManager.SetScreenOrientationStatus( (int) ScreenOrientationStatus.SCREEN_LANDSCAPE );
            soundButton.isOn = saveLoadManager.GetSoundStatus() != 0;
            cameraButton.isOn = saveLoadManager.GetCameraStatus() != 0;
            orientationButton.isOn = saveLoadManager.GetScreenOrientationStatus() != 0;
            portraitCamera.SetActive( false );
            portraitGUI.SetActive( false );
            landscapeCamera.SetActive( true );

            yield return screenOrientationWait;

            landscapeGUI.SetActive( true );
        }
    }
}
