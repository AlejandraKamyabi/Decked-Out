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

    public void Initialize()
    {
        wave = ServiceLocator.Get<WaveManager>();
        splashScreen.SetActive(false);
        castleGameObject = ServiceLocator.Get<Castle>();

    }
    public void Death()
    {
        splashScreen.SetActive(true);
    }

    public void Retry()
    {
        castleGameObject.ResetHealth();
        splashScreen.SetActive(false);
    }
    public void MainMenu()
    {
        wave.StopWave();
        var loadSceneTask = SceneManager.LoadSceneAsync(mainMenuScene);
    }
    public void Deckbuilder()
    {
        Debug.Log("Deckbuilding not yet implimented");
        //SceneManager.LoadScene(deckbuilderScene);
    }
}
