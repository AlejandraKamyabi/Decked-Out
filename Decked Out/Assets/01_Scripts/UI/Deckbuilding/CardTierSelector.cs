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
    [SerializeField] Image _border;
    [SerializeField] string _tier;

    [Header("Card Data")]
    [SerializeField] SelectedCard[] _cardRenderers;
    [SerializeField] Image[] _cardBlanks;
    [SerializeField] GameObject[] _cardsToPickGO;
    [SerializeField] TowerCardSO[] _cardsOfRarity;

    public void SetTier()
    {
        bool noCardsSaved = false;
        if (_manager.CheckIfSavedCards(_tier).Length < 0)
        {
            noCardsSaved = false;
        }
        else
        {
            noCardsSaved = true;
        }
        Debug.Log("Renderering " + _cardsOfRarity.Length + " cards from " + _tier + " tier");
        _cardRenderers = _manager.SetTierRenderers(_cardsOfRarity.Count());
  
        //if (noCardsSaved)
        {
            //TowerCardSO[] savedCards = new TowerCardSO[_manager.CheckIfSavedCards(_tier).Length];
            //savedCards = _manager.CheckIfSavedCards(_tier).ToArray();
            //for (int i = 0; i < savedCards.Length; i++)
            {
                //_cardRenderers[i].SlotInCard(savedCards[i]);
            }           
        }
        PresentCards();
    }
    private void PresentCards()
    {
        for (int i = 0; i < _cardRenderers.Length; i++)
        {
            _cardRenderers[i].SlotInCard(_cardsOfRarity[i]);
        }

    }

    public void SaveCards()
    {
        //TowerCardSO[] cards = new TowerCardSO[_cardRenderers.Length];
        //List<TowerCardSO> cardsList = new List<TowerCardSO>();

        //foreach (SelectedCard slottedCards in _cardRenderers)
        {
           // if (slottedCards.slottedIn)
            {
                //TowerCardSO card = slottedCards.card;
                //cardsList.Add(card);
            }
        }
       // cards = cardsList.ToArray();
        //if (_manager.CheckCurrentTier(_tier))
        {
           // _manager.SetCardsOfTier(cards, _tier);
        }       
    }


}
