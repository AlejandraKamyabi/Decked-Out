using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Popup : MonoBehaviour
{
    [Header("Files")]
    [SerializeField] string[] _fileNames;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI _popupText;
    [SerializeField] GameObject _nextButton;
    [Header("Misc")]
    [SerializeField] string _waitingObjectName;

    TextMeshProUGUI _buttonTextt;
    TutorialManager _manager;
    int _fileIndex = 0;
    private void Awake()
    {
        _buttonTextt = _nextButton.GetComponentInChildren<TextMeshProUGUI>();
        _manager = FindObjectOfType<TutorialManager>();
        if (_fileNames.Length == 0)
        {
            Debug.LogError("No filenames listed");
        }
        else
        {
            LoadText(_fileIndex);
            if (_fileNames.Length > 1)
            {
                _nextButton.SetActive(true);
                Debug.Log(_fileNames.Length);
            }
        } 
        
    }
    private void LoadText(int index)
    {        
        TextAsset textAsset = Resources.Load<TextAsset>("Text/" + _fileNames[index]);
        if (textAsset != null)
        {
            _popupText.text = textAsset.text;
        }
        else
        {
            Debug.LogError(_fileNames[index] + " not found.");
        }

        if (_fileIndex == _fileNames.Length)
        {
            
        }
    }
    public void NextText()
    {
        _fileIndex++;
        Debug.Log(_fileIndex);
        LoadText(_fileIndex);
        if (_fileIndex == _fileNames.Length -1)
        {
            _nextButton.SetActive(false);
            _manager.Waiting(true, _waitingObjectName);
        }
    }
}
