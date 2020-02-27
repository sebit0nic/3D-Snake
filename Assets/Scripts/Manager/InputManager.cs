using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public SnakeMovement snakeMovement;

    private ShareImageCanvas shareImageCanvas;

    private void Start() {
        shareImageCanvas = new ShareImageCanvas();
    }

    public void OnPauseButtonClicked() {
        GameManager.instance.GamePaused();
    }

    public void OnRetryButtonPressed() {
        GameManager.instance.SwitchScreen(ScreenType.GAME);
    }

    public void OnShopButtonPressed(bool inMainMenu) {
        if (inMainMenu) {
            MainMenuManager.instance.SwitchScreen(ScreenType.SHOP_MENU);
        } else {
            GameManager.instance.SwitchScreen(ScreenType.SHOP_MENU);
        }
    }

    public void OnHighScoreButtonPressed() {
        //TODO: implement highscore boards
    }

    public void OnMainMenuButtonPressed() {
        GameManager.instance.SwitchScreen(ScreenType.MAIN_MENU);
    }
    
    public void OnPlayButtonPressed() {
        MainMenuManager.instance.SwitchScreen(ScreenType.GAME);
    }

    public void OnShareButtonPressed() {
        shareImageCanvas.ShareScreenshotWithText();
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
