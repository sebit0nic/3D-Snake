using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCircleEffect : MonoBehaviour {

    private Animator thisAnimator;

    private void Start() {
        thisAnimator = GetComponent<Animator>();
    }

    public void NotifyFruitCollected() {
        transform.LookAt(Camera.main.transform);
        thisAnimator.SetTrigger("OnCollected");
    }
}
