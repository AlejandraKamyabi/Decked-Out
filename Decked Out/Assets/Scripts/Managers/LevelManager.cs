using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _waveManager = null;
    [SerializeField] private MouseInputHandling _mouseInputHandling = null;
    [SerializeField] private Castle castle = null;
    [SerializeField] private EndGameSplashManager _endGameSplash = null;


    private GameLoader _loader;

    public event Action LevelLoaded;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log("Registering Wave Manager");
        if (_waveManager != null)
        {
            var wm = Instantiate(_waveManager, GameLoader.SystemsParent);
            var waveManager = wm.GetComponent<WaveManager>();
            ServiceLocator.Register<WaveManager>(waveManager.Initialize());
        }

        Debug.Log("Registering MouseInputHandling");
        _mouseInputHandling.Initialize();
        ServiceLocator.Register<MouseInputHandling>(_mouseInputHandling);
        castle.Initialize();
        ServiceLocator.Register<Castle>(castle);

        _endGameSplash.Initialize();

        LevelLoaded?.Invoke();
    }
}
