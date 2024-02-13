using UnityEngine;

public class TowerHealthFlash : MonoBehaviour
{
    [SerializeField] float _flashDelay;
    [SerializeField] float _flashDuration;
    [SerializeField] Color _flashColour;
    [SerializeField] float _flashThreshold;

    float _timer;
    ITower _towerScript;
    IBuffTower _buffTowerScript;
    SpriteRenderer _spriteRenderer;
    bool _tower;
    bool _buffTower;

    private void Start()
    {
        if (TryGetComponent<ITower>(out _towerScript))
        {
            _tower = true;
            Debug.Log("Tower Script Found");
        }
        else if(TryGetComponent<IBuffTower>(out _buffTowerScript))
        {
            _buffTower = true;
            Debug.Log("Buff Tower Script Found");
        }
        else
        {
            Debug.LogError("No Tower Found - Check for Interface on Script");
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _timer = _flashDelay;
    }
    private void Update()
    {
        if (_tower)
        {
            if (_towerScript.health <= _flashThreshold)
            {
                //Debug.Log("Tower Below Threshold");
                _timer += Time.deltaTime;
                if (_timer >= (_flashDelay + _flashDuration))
                {
                    //Debug.Log("Tower Reset");
                    _spriteRenderer.color = Color.white;
                    _timer = 0;

                }
                else if (_timer >= _flashDelay)
                {
                    //Debug.Log("Tower Flashed");
                    _spriteRenderer.color = _flashColour;
                }
            }
            else
            {
                _timer = 0;
            }
        }
        else if (_buffTower)
        {
            if (_buffTowerScript.health <= _flashThreshold)
            {
                //Debug.Log("Tower Below Threshold");
                _timer += Time.deltaTime;
                if (_timer >= (_flashDelay + _flashDuration))
                {
                    //Debug.Log("Tower Reset");
                    _spriteRenderer.color = Color.white;
                    _timer = 0;

                }
                else if (_timer >= _flashDelay)
                {
                    //Debug.Log("Tower Flashed");
                    _spriteRenderer.color = _flashColour;
                }
            }
            else
            {
                _timer = 0;
            }
        }
       
    }
}
