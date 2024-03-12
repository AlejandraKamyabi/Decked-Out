using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStartupRangeFlash : MonoBehaviour
{


    ITower _towerScript;
    IBuffTower _buffTowerScript;
    SpriteRenderer _rangeRenderer;
    bool _tower;
    bool _buffTower;

    private void Start()
    {
        if (TryGetComponent<ITower>(out _towerScript))
        {
            _tower = true;
            Debug.Log("Tower Script Found");
        }
        else if (TryGetComponent<IBuffTower>(out _buffTowerScript))
        {
            _buffTower = true;
            Debug.Log("Buff Tower Script Found");
        }
        else
        {
            Debug.LogError("No Tower Found - Check for Interface on Script");
        }

        _rangeRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void SetRange()
    {
        if (_tower)
        {
            float towerRange = _towerScript.range;
        }
    }

}
