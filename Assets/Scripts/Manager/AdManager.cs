using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour {

    private bool adAvailable;

    private void Start() {
        adAvailable = true;
    }

    public void ShowAd() {
        //TODO: do something
    }

    public bool IsAdAvailable() {
        return adAvailable;
    }

    public void SetAdAvailable(bool value) {
        adAvailable = value;
    }
}
