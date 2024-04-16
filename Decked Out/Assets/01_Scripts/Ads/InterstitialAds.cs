using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;

    private void Awake()
    {
#if UNITY_IOS
                adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
                adUnitId = androidAdUnitId;
#endif
    }

    public void Initialize()
    {
        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Interstitial Ad Completed");
        // Call the method in EndGameSplashManager to proceed after ad completion
        FindObjectOfType<EndGameSplashManager>().OnInterstitialAdCompleted();
    }

    public void ShowInterstitialAd()
    {
        if (Advertisement.IsReady(adUnitId))
        {
            Advertisement.Show(adUnitId, this);
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet");
        }
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial Ad Loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }
}
