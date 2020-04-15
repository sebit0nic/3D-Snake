using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles communication between different managers in the main menu.
/// </summary>
public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager instance = null;
    public ScreenTransition screenTransition;
    public Toggle soundButton, cameraButton;
    
    private SaveLoadManager saveLoadManager;
    private StyleManager styleManager;
    private SavedData savedData;
    private PlayStoreManager playStoreManager;
    private SoundManager soundManager;

    private const string privacyPolicyLink = "https://www.privacypolicygenerator.info/live.php?token=XHLfKYMTFcs0emh5p7caBpGhSZNSlEH2";

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
        soundManager.PlaySound( SoundEffectType.SOUND_AMBIENCE, false );

        soundButton.isOn = saveLoadManager.GetSoundStatus() != 0;
        cameraButton.isOn = saveLoadManager.GetCameraStatus() != 0;

        playStoreManager.Init();
        playStoreManager.SignIn();
    }

    /// <summary>
    /// Change the scene to something else.
    /// </summary>
    public void SwitchScreen( ScreenType screenType ) {
        screenTransition.StartScreenTransition( (int) screenType );
    }

    /// <summary>
    /// Toggle the sound on or off.
    /// </summary>
    public void ToggleButtonSoundPressed() {
        saveLoadManager.SetSoundStatus( soundButton.isOn ? 1 : 0 );
        soundManager.Init( saveLoadManager );
        soundManager.PlaySound( SoundEffectType.SOUND_BUTTON, false );

        if ( (SoundStatus) saveLoadManager.GetSoundStatus() == SoundStatus.SOUND_OFF ) {
            soundManager.StopAllSound();
        }  else {
            soundManager.PlaySound( SoundEffectType.SOUND_AMBIENCE, false );
        }
    }

    /// <summary>
    /// Toggle the camera rotation on or off.
    /// </summary>
    public void ToggleButtonCameraPressed() {
        saveLoadManager.SetCameraStatus( cameraButton.isOn ? 1 : 0 );
        soundManager.PlaySound( SoundEffectType.SOUND_BUTTON, false );
    }

    /// <summary>
    /// Open the privacy policy in a web-browser.
    /// </summary>
    public void OpenPrivacyPolicyWebsite() {
        Application.OpenURL( privacyPolicyLink );
    }
}
