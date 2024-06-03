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
        RestartGame();   
    }

    // Restart the game method
    private void RestartGame()
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
