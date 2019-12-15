using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private SnakeMovement snakeMovement;
    private ShareImageCanvas shareImageCanvas;

    private void Start() {
        snakeMovement = GameObject.Find("Snake").GetComponent<SnakeMovement>();
        shareImageCanvas = new ShareImageCanvas();
    }

    public void OnPauseButtonClicked() {
        GameManager.instance.GamePaused();
    }

    public void OnRetryButtonPressed() {
        GameManager.instance.SwitchScreen(ScreenType.GAME);
    }

    public void OnShopButtonPressed() {
        GameManager.instance.SwitchScreen(ScreenType.SHOP_MENU);
    }

    public void OnHighScoreButtonPressed() {
        //TODO: implement highscore boards
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
