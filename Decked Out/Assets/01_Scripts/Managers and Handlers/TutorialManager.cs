using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] _popups;
    [Range(0f, 0.25f)]
    [SerializeField] float _checkTimer;

    private GameLoader _loader;
    public int _index = 0;
    private float _timer;
    private bool _tutorial;
    private bool _waiting = false;
    private string _waitingObjectName;
    private GameObject _waitingObject;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    public void Waiting(bool waiting, string waitingObjectName)
    {
        _waiting = waiting;
        _waitingObjectName = waitingObjectName;
    }

    private void Initialize()
    {
        TutorialPassthrough passthrough = FindObjectOfType<TutorialPassthrough>();
        if (passthrough != null)
        {
            _tutorial = true;
            LoadPopup();
            _timer = _checkTimer;
        }
    }
    private void LoadPopup()
    {
        _popups[_index].SetActive(true);
    }
    private void Update()
    {
        if (_tutorial)
        {
            _timer -= Time.deltaTime;
            if (_waiting && _timer <= 0)
            {
                _waitingObject = GameObject.Find(_waitingObjectName);
                _timer = _checkTimer;
            }
            if (_waitingObject != null && _waitingObject.activeInHierarchy)
            {
                _waiting = false;
                _waitingObject = null;
                _popups[_index].gameObject.SetActive(false);
                _index++;
                LoadPopup();
            }
            else if (_waitingObject != null && _waitingObject.activeInHierarchy == false)
            {
                Debug.LogError("Found Waiting Object, but is not active");
            }
        }
       
        
       

    }

}
