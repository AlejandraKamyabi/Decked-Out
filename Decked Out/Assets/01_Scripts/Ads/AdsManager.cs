using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public InterstitialAds interstitialAds;

    public static AdsManager Instance { get; private set; }

    private void Awake()
    {
        Debug.Log("AdsManager Awake");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize InterstitialAds
        interstitialAds.Initialize();
    }

    // Method to show interstitial ad
    public void ShowInterstitialAd()
    {
        interstitialAds.ShowInterstitialAd();
    }
}
