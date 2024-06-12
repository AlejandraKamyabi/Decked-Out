using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSplashManager : MonoBehaviour
{
    public GameObject splashScreen;
    public string gameScene;
    public string mainMenuScene;
    public string deckbuilderScene;
    private WaveManager wave_m;
    public Castle castleGameObject;
    public CardRandoEngine cardRandoEngine;
    public EnemyKillTracker enemyKillTracker;

    public void Initialize()
    {
        wave_m = ServiceLocator.Get<WaveManager>();
        splashScreen.SetActive(false);
        castleGameObject = ServiceLocator.Get<Castle>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        enemyKillTracker = FindObjectOfType<EnemyKillTracker>();
    }

    public void Death()
    {
        splashScreen.SetActive(true);
        enemyKillTracker.EndGame();
    }

    public void Retry()
    {
        RestartGame();
    }

    // Restart the game method
    private void RestartGame()
    {
        wave_m.StopWave();
        enemyKillTracker.ResetValues();
        castleGameObject.ResetHealth();
        splashScreen.SetActive(false);
        cardRandoEngine.NewWave();
        wave_m.StartWaves();
    }

    public void MainMenu()
    {
        wave_m.StopWave();
        var loadSceneTask = SceneManager.LoadSceneAsync(mainMenuScene);
    }

    public void Deckbuilder()
    {
        Debug.Log("Deckbuilding not yet implemented");
        //SceneManager.LoadScene(deckbuilderScene);
    }
}
