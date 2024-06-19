using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSplashManager : MonoBehaviour
{
    public GameObject splashScreen;
    private WaveManager wave_m;
    private GameSpeedManager _gameSpeedManager;
    private TransitionScreenManager _transitionScreenManager;
    public Castle castleGameObject;
    public CardRandoEngine cardRandoEngine;
    public EnemyKillTracker enemyKillTracker;

    public void Initialize()
    {
        wave_m = ServiceLocator.Get<WaveManager>();
        _transitionScreenManager = FindObjectOfType<TransitionScreenManager>();
        _gameSpeedManager = FindObjectOfType<GameSpeedManager>();
        splashScreen.SetActive(false);
        castleGameObject = ServiceLocator.Get<Castle>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        enemyKillTracker = FindObjectOfType<EnemyKillTracker>();
    }

    public void Death()
    {
        _gameSpeedManager.ActivateControlPanel();
        _gameSpeedManager.ResumeGame();
        splashScreen.SetActive(true);
        enemyKillTracker.EndGame();
    }

    public void Continue()
    {
        wave_m.StopWave();
        castleGameObject.ResetHealth();
        splashScreen.SetActive(false);
        cardRandoEngine.NewWave();
        wave_m.StartWaves();
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
        _transitionScreenManager.StartTranistion("MainMenu");
    }
}
