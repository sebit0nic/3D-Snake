using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsListener {

    public Button adButton;

    private const string gameId = "3527725";
    private const string myPlacementId = "rewardedVideo";
    private bool adAvailable;

    private void Start() {
        adAvailable = true;

        adButton.interactable = Advertisement.IsReady(myPlacementId);
        if (adButton) {
            adButton.onClick.AddListener(ShowRewardedVideo);
        }

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId);
    }

    private void ShowRewardedVideo() {
        Advertisement.Show(myPlacementId);
    }
    
    public void OnUnityAdsReady(string placementId) {
        if (placementId == myPlacementId) {
            adButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
        if (showResult == ShowResult.Finished) {
            GameManager.instance.GameResumedAfterAd();
        } else if (showResult == ShowResult.Skipped || showResult == ShowResult.Failed) {
            GameManager.instance.AdSkippedOrFailed();
        }
    }

    public void OnUnityAdsDidError(string message) {
        Debug.LogWarning("The ad did not finish due to an error.");
    }

    public void OnUnityAdsDidStart(string placementId) {
        Debug.LogWarning("Ad started.");
    }

    public bool IsAdAvailable() {
        return adAvailable;
    }

    public void SetAdAvailable(bool value) {
        adAvailable = value;
    }
}
