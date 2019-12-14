using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    private Animator animator;
    private GuiManager guiManager;
    private int toSceneID;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Init(GuiManager guiManager) {
        this.guiManager = guiManager;
    }

    public void StartScreenTransition(int sceneID) {
        toSceneID = sceneID;
        animator.SetTrigger("OnTransition");
    }

    public void ScreenTransitionFinished() {
        SceneManager.LoadScene(toSceneID);
    }
}
