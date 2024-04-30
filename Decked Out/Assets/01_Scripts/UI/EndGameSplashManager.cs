using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSplashManager : MonoBehaviour
{
    public GameObject splashScreen;
    public string gameScene;
    public string mainMenuScene;
    public string deckbuilderScene;
    private WaveManager wave;
    public Castle castleGameObject;
    public CardRandoEngine cardRandoEngine;
    public EnemyKillTracker enemyKillTracker;

    private InterstitialAds interstitialAds; // Reference to the InterstitialAds script

    public void Initialize()
    {
        wave = ServiceLocator.Get<WaveManager>();
        splashScreen.SetActive(false);
        castleGameObject = ServiceLocator.Get<Castle>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();

        // Find InterstitialAds GameObject by name
        GameObject adsManager = GameObject.Find("AdsManager");
        if (adsManager != null)
        {
            interstitialAds = adsManager.GetComponent<InterstitialAds>();
        }
        else
        {
            Debug.LogError("Failed to find InterstitialAds GameObject by name.");
        }
    }

    public void Death()
    {
        splashScreen.SetActive(true);
        enemyKillTracker.EndGame();
    }

    public void Retry()
    {
        if (interstitialAds != null)
        {
            // Load the interstitial ad
            interstitialAds.LoadAd();
        }
        else
        {
            Debug.LogError("InterstitialAds reference is null.");
        }
    }

    // This method will be called by InterstitialAds script after the ad is loaded
    public void ShowInterstitialAd()
    {
        if (interstitialAds != null)
        {
            // Show the interstitial ad
            interstitialAds.ShowAd();
        }
        else
        {
            Debug.LogError("InterstitialAds reference is null.");
        }
    }

    // This method will be called by InterstitialAds script after the ad is closed
    public void RestartGameAfterAd()
    {
        castleGameObject.ResetHealth();
        splashScreen.SetActive(false);
        cardRandoEngine.NewWave();
    }

    public void MainMenu()
    {
        wave.StopWave();
        var loadSceneTask = SceneManager.LoadSceneAsync(mainMenuScene);
    }

    public void Deckbuilder()
    {
        Debug.Log("Deckbuilding not yet implemented");
        //SceneManager.LoadScene(deckbuilderScene);
    }
}
