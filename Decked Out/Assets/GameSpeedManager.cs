using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedManager : MonoBehaviour
{
    [SerializeField] float _fastSpeedTimeScale = 1.5f;
    [SerializeField] float _turboSpeedTimeScale = 3f;

    [Header("UI")]
    [SerializeField] GameObject _timeControlPanel;

    private void Start()
    {
        DeactiveControlPanel();
    }

    public void ActivateControlPanel()
    {
        _timeControlPanel.SetActive(true);
    }
    public void DeactiveControlPanel()
    {
        _timeControlPanel.SetActive(false);
        ResumeGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    public void FastSpeed()
    {
        Time.timeScale = _fastSpeedTimeScale;
    }
    public void TurboSpeed()
    {
        Time.timeScale = _turboSpeedTimeScale;
    }

}
