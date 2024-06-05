using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreResets : MonoBehaviour
{
    SaveSystem _saveSystem;

    void Awake()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
    }

    public void ResetCards()
    {
        _saveSystem.ResetCardCollected();
    }
    public void ResetGems()
    {
        _saveSystem.ResetGemCount();
    }
    public void ResetTotalKills()
    {
        _saveSystem.ResetTotalKill();
    }
}
