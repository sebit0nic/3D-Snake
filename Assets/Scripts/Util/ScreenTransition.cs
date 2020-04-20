using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the screen transition elements in each scene
/// </summary>
public class ScreenTransition : MonoBehaviour {

    private Animator animator;
    private int toSceneID;
    private const string transitionAnimationName = "OnTransition";

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Trigger the animation using the given sceneID
    /// </summary>
    public void StartScreenTransition( int sceneID ) {
        toSceneID = sceneID;
        animator.SetTrigger( transitionAnimationName );
    }

    /// <summary>
    /// Load scene after animation has finished
    /// </summary>
    public void ScreenTransitionFinished() {
        System.GC.Collect();
        SceneManager.LoadScene( toSceneID );
    }
}
