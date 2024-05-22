using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenuButton : MonoBehaviour
{
    [SerializeField] string _menuScene = "MainMenu";

    public void LoadScene()
    {
        SceneManager.LoadScene(_menuScene);
    }
}
