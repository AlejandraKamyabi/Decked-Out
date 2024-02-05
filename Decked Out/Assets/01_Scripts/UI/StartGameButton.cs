using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    private GameLoader _loader;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize() 
    {
        var waveManager = ServiceLocator.Get<WaveManager>();
        waveManager.SetStartButton(_startButton);
    }

}
