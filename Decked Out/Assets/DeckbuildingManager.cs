using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckbuildingManager : MonoBehaviour
{
    [SerializeField] CardTierSelector[] _buttonScrips;

    private void Start()
    {
        _buttonScrips[0].SetTier();
    }
}
