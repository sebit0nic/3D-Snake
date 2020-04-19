using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

/// <summary>
/// Handles all incoming requests for Ad placement.
/// </summary>
public class AdManager : MonoBehaviour, IUnityAdsListener {

    public Button adButton;

    private const string gameId = "3527725";
    private const string myPlacementId = "rewardedVideo";
    private bool adAvailable;

    private void Start() {
        adAvailable = true;

        adButton.interactable = Advertisement.IsReady( myPlacementId );
        if( adButton ) {
            adButton.onClick.AddListener( ShowRewardedVideo );
        }

        Advertisement.AddListener( this );
        Advertisement.Initialize( gameId );
    }

    /// <summary>
    /// Play the rewarded ad.
    /// </summary>
    private void ShowRewardedVideo() {
        Advertisement.Show( myPlacementId );
    }
    
    /// <summary>
    /// Check if ad is ready and if so, set the ad button enabled.
    /// </summary>
    public void OnUnityAdsReady( string placementId ) {
        if( placementId == myPlacementId ) {
            adButton.interactable = true;
        }
    }

    /// <summary>
    /// Rewarded ad has finished playing, so check if it was successful.
    /// </summary>
    public void OnUnityAdsDidFinish( string placementId, ShowResult showResult ) {
        adAvailable = false;

        if( showResult == ShowResult.Finished ) {
            GameManager.instance.GameResumedAfterAd();
        } else if( showResult == ShowResult.Skipped || showResult == ShowResult.Failed ) {
            GameManager.instance.AdSkippedOrFailed();
        }
    }

    public void OnUnityAdsDidError( string message ) {
        return;
    }

    public void OnUnityAdsDidStart( string placementId ) {
        return;
    }

    public bool IsAdAvailable() {
        return adAvailable;
    }

    public void SetAdAvailable( bool value ) {
        adAvailable = value;
    }
}
