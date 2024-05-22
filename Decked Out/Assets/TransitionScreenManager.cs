using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScreenManager : MonoBehaviour
{
    [SerializeField] GameObject _leftParent;
    [SerializeField] Transform _leftTarget;
    [SerializeField] GameObject _rightParent;
    [SerializeField] Transform _rightTarget;

    bool _loading;
    string _previousScene;
    string _targetScene;

    public void LoadingScene(string loadingSceneName)
    {
        _previousScene = SceneManager.GetActiveScene().name;
        _loading = true;
        _targetScene = loadingSceneName;
        
    }
        

}
