using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public SnakeMovement snakeMovement;
    public PlayStoreManager playStoreManager;
    public SoundManager soundManager;

    public void OnPauseButtonClicked() {
        soundManager.PlaySound(SoundEffectType.SOUND_BUTTON, false);
        GameManager.instance.GamePaused();
    }

    public void OnRetryButtonPressed() {
        soundManager.PlaySound(SoundEffectType.SOUND_BUTTON, false);
        GameManager.instance.SwitchScreen(ScreenType.GAME);
    }

    public void OnShopButtonPressed(bool inMainMenu) {
        soundManager.PlaySound(SoundEffectType.SOUND_BUTTON, false);
        if (inMainMenu) {
            MainMenuManager.instance.SwitchScreen(ScreenType.SHOP_MENU);
        } else {
            GameManager.instance.SwitchScreen(ScreenType.SHOP_MENU);
        }
    }

    public void OnHighScoreButtonPressed() {
        soundManager.PlaySound(SoundEffectType.SOUND_BUTTON, false);
        playStoreManager.ShowLeaderboard();
    }

    public void OnMainMenuButtonPressed() {
        soundManager.PlaySound(SoundEffectType.SOUND_BUTTON, false);
        GameManager.instance.SwitchScreen(ScreenType.MAIN_MENU);
    }
    
    public void OnPlayButtonPressed() {
        soundManager.PlaySound(SoundEffectType.SOUND_BUTTON, false);
        MainMenuManager.instance.SwitchScreen(ScreenType.GAME);
    }

    public void OnShareButtonPressed() {
        soundManager.PlaySound(SoundEffectType.SOUND_BUTTON, false);
        GameManager.instance.ShareScreen();
    }

    public void OnSteerRightButtonPressed() {
        snakeMovement.MoveRight();
    }

    public void OnSteerRightButtonReleased() {
        snakeMovement.MoveRelease(1);
    }

    public void OnSteerLeftButtonPressed() {
        snakeMovement.MoveLeft();
    }

    public void OnSteerLeftButtonReleased() {
        snakeMovement.MoveRelease(-1);
    }
}
