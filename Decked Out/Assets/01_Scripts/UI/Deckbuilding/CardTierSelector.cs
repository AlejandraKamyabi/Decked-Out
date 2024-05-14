using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardTierSelector : MonoBehaviour
{
    [SerializeField] DeckbuildingManager _manager;
    [SerializeField] Color _rarityColour;

    [Header("Tier Data")]
    [SerializeField] Sprite _border;
    [SerializeField] string _tier;
    [SerializeField] int _maxCount;

    [Header("Card Data")]
    [SerializeField] SelectedCard[] _cardRenderers;
    [SerializeField] Image[] _cardBlanks;
    [SerializeField] GameObject[] _cardsToPickGO;
    [SerializeField] TowerCardSO[] _cardsOfRarity;

    public void SetTier()
    {
         Debug.Log("Renderering " + _cardsOfRarity.Length + " cards from " + _tier + " tier");
        _cardRenderers = _manager.SetTierRenderers(_cardsOfRarity.Count(), _tier);
        PresentCards();
    }
    private void PresentCards()
    {
        _manager.DeactivateGreyOut();
        for (int i = 0; i < _cardRenderers.Length; i++)
        {
            _cardRenderers[i].SlotInCard(_cardsOfRarity[i]);
            _cardRenderers[i].SetBorderSprite(_border);
        }
        for (int i = 0; i < _cardRenderers.Length; i++)
        {
            if (_manager.CheckIfSavedCards(_tier).Contains(_cardRenderers[i].card))
            {
                _cardRenderers[i].ActivateGlow();
            }
        }
        if (_manager.CheckIfSavedCards(_tier).Length >= _maxCount)
        {
            foreach (SelectedCard cardRenderer in _cardRenderers)
            {
                if (!cardRenderer.selected)
                {
                    cardRenderer.GreyOut(true);
                }
            }
            
        }
    }


}
