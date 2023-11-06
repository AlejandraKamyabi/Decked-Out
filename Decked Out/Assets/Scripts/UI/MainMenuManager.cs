using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameScene;
    public GameObject scripturesPanel;
    public GameObject settingsPanel;
    public GameObject minimizeCollider;
    public Image enemyPageImage;
    

    public void Start()
    {
        scripturesPanel.SetActive(false);
        settingsPanel.SetActive(false);
        minimizeCollider.SetActive(false);
    }

    public void StartGame()
    {
        var loadSceneTask = SceneManager.LoadSceneAsync(gameScene);
    }
    public void ContinueGame()
    {
        Debug.Log("Continue Game not yet implimented");
    }
    public void OpenScripture()
    {
        scripturesPanel.SetActive(true);
        minimizeCollider.SetActive(true);
    }
    public void OpenSettings()
    {
        Debug.Log("Settings not yet implimented");
        //settingsPanel.SetActive(true);
        //minimizeCollider.SetActive(true);
    }
    public void MinimizeScripturePanel()
    {
        Debug.Log("Collider Clicked");
        scripturesPanel.SetActive(false);
        minimizeCollider.SetActive(false);
    }
   
}
