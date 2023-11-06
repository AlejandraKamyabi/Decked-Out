using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameSplashManager : MonoBehaviour
{
    public GameObject splashScreen;
    public string gameScene;
    public string mainMenuScene;
    public string deckbuilderScene;
    private WaveManager wave;
    private GameLoader _loader;
    public Castle castleGameObject;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
        splashScreen.SetActive(false);
    }
    private void Initialize()
    {
        wave = ServiceLocator.Get<WaveManager>();
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
     //   wave.StopWave();
     //   var loadSceneTask = SceneManager.LoadSceneAsync(mainMenuScene);
    }
    public void Deckbuilder()
    {
        Debug.Log("Deckbuilding not yet implimented");
        //SceneManager.LoadScene(deckbuilderScene);
    }
}
