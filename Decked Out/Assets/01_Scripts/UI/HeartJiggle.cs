using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartJiggle : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] float _jiggleDuration = 0.25f;
    [SerializeField] float[] _jiggleMagnitudes;
    

    Castle _castleScript;
    Vector3 _originalPOS;
    private GameLoader _loader;

    float _jiggleMagnitude = 0.1f;
    float _currentHealth;
    float _lastHealth;
    float _maxHealth;

    float _jiggleTimer;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
    {
        _castleScript =  FindObjectOfType<Castle>();
        _maxHealth = _castleScript.maxHealth;
        _currentHealth = _castleScript.health;
        _lastHealth = _currentHealth;
    }
    public void StartJiggle(float health)
    {
        Debug.Log("jiggling");
        _lastHealth = _currentHealth;
        _jiggleMagnitude = 1f;

        float difference = Mathf.Abs(_lastHealth - health);
        int multipleOfFive = Mathf.FloorToInt(difference / 5);
        switch (multipleOfFive)
        {
            case 0:
                _jiggleMagnitude = _jiggleMagnitudes[0];
                break;
            case 1:
                _jiggleMagnitude = _jiggleMagnitudes[1];
                break;
            case 2:
                _jiggleMagnitude = _jiggleMagnitudes[2];
                break;
            case 3:
                _jiggleMagnitude = _jiggleMagnitudes[3];
                break;
            case 4:
                _jiggleMagnitude = _jiggleMagnitudes[4];
                break;
            case 5:
                _jiggleMagnitude = _jiggleMagnitudes[5]; 
                break;
        }
        _currentHealth = health;
        _jiggleDuration = 1f;
        _jiggleTimer = _jiggleDuration;
    }
    private void Update()
    {
        if (_jiggleTimer > 0)
        {
            _jiggleTimer -= Time.deltaTime;
            float jiggleFactorX = Mathf.Sin(Time.time * Mathf.PI * 2 + Random.Range(-1f, 1f)) * _jiggleMagnitude;
            float jiggleFactorY = Mathf.Sin(Time.time * Mathf.PI * 2 + Random.Range(-1f, 1f)) * _jiggleMagnitude;

            _slider.transform.localPosition = _originalPOS + new Vector3(jiggleFactorX, jiggleFactorY, 0);
        }
        else
        {
            _slider.transform.localPosition = _originalPOS;
        }
    }

}
