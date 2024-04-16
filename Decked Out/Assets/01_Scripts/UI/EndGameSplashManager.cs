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

    // Reference to AdsManager
    public AdsManager adsManager;

    public void Initialize()
    {
        wave = ServiceLocator.Get<WaveManager>();
        splashScreen.SetActive(false);
        castleGameObject = ServiceLocator.Get<Castle>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
    }

    public void Death()
    {
        splashScreen.SetActive(true);
        enemyKillTracker.EndGame();
    }

    public void Retry()
    {
        // Show interstitial ad
        adsManager.ShowInterstitialAd();
    }

    // Called when interstitial ad is completed
    public void OnInterstitialAdCompleted()
    {
        // Reset health, hide splash screen, and load next round
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
