using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles changes to the UI element of the tutorial indicators.
/// </summary>
public class TouchIndicator : MonoBehaviour {

    public float minTouchdownTime;
    public Image indicator, icon, fillLevel;
    public RectTransform fillLevelTransform;

    private Vector3 fillLevelIncreaseFactor;
    private bool tutorialDone;
    private GuiManager guiManager;
    private const string touchIndicatorActiveKey = "OnTouchIndicatorActive";

    public void Init(GuiManager guiManager, bool tutorialDone) {
        fillLevelTransform.localScale = Vector3.zero;
        fillLevelIncreaseFactor = new Vector3( Time.fixedDeltaTime / minTouchdownTime, Time.fixedDeltaTime / minTouchdownTime, Time.fixedDeltaTime / minTouchdownTime );

        this.guiManager = guiManager;
        this.tutorialDone = tutorialDone;

        if( !tutorialDone ) {
            EnableAll();
        }
    }

    /// <summary>
    /// Notify that button to turn left or right is being pressed.
    /// </summary>
    public void SetTouched( bool touched ) {
        if( !tutorialDone ) {
            if( touched ) {
                StartCoroutine( touchIndicatorActiveKey );
            } else {
                StopAllCoroutines();
            }
        }
    }

    /// <summary>
    /// Set the fill amount every fixed update when button is being pressed.
    /// </summary>
    private IEnumerator OnTouchIndicatorActive() {
        while( fillLevelTransform.localScale.x <= 1 ) {
            yield return new WaitForFixedUpdate();
            fillLevelTransform.localScale += fillLevelIncreaseFactor;
        }

        DisableAll();

        tutorialDone = true;
        guiManager.TouchIndicatorTutorialDone();
    }

    private void EnableAll() {
        indicator.enabled = true;
        icon.enabled = true;
        fillLevel.enabled = true;
    }

    private void DisableAll() {
        indicator.enabled = false;
        icon.enabled = false;
        fillLevel.enabled = false;
    }
}