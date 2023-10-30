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

    public void Start()
    {
        splashScreen.SetActive(false);
    }

    public void Death()
    {
        splashScreen.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(gameScene);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    public void Deckbuilder()
    {
        Debug.Log("Deckbuilding not yet implimented");
        //SceneManager.LoadScene(deckbuilderScene);
    }
}
