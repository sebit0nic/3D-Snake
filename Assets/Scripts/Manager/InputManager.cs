using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private SnakeMovement snakeMovement;

    private void Start() {
        snakeMovement = GameObject.Find("Snake").GetComponent<SnakeMovement>();
    }

    public void OnPauseButtonClicked() {
        GameManager.instance.GamePaused();
    }

    public void OnRetryButtonClicked() {
        GameManager.instance.GameRetry();
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
