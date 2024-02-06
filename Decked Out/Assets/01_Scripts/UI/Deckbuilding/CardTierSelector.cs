using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardTierSelector : MonoBehaviour
{
    [SerializeField] DeckbuildingManager _manager;

    [Header("Tier Data")]
    [SerializeField] Image _border;
    [SerializeField] string _tier;

    [Header("Card Data")]
    [SerializeField] SelectedCard[] _cardsSlots;
    [SerializeField] Image[] _cardBlanks;
    [SerializeField] GameObject[] _cardsToPickGO;
    [SerializeField] TowerCardSO[] _cardsOfRarity;

    Color _rarityColour;
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
  
        _rarityColour = gameObject.GetComponentInChildren<Image>().color;
        _border.color = _rarityColour;
        SelectedCard[] allCardSlots = FindObjectsOfType<SelectedCard>();
        foreach (SelectedCard cardSlot in allCardSlots)
        {
            cardSlot.gameObject.SetActive(false);
        }
        Image[] allImages = FindObjectsOfType<Image>();
        foreach (Image image in allImages)
        {
            if (image.gameObject.CompareTag("Card Blank"))
            {
                image.enabled = false;
            }
        }
        foreach (SelectedCard cardSlot in _cardsSlots)
        {
            cardSlot.gameObject.SetActive(true);
            cardSlot.Unslot();   

        }
        foreach (Image image in _cardBlanks)
        {
            image.enabled = true;
        }
        if (noCardsSaved)
        {
            TowerCardSO[] savedCards = new TowerCardSO[_manager.CheckIfSavedCards(_tier).Length];
            savedCards = _manager.CheckIfSavedCards(_tier).ToArray();
            for (int i = 0; i < savedCards.Length; i++)
            {
                _cardsSlots[i].SlotInCard(savedCards[i]);
            }           
        }
        PresentCards();
    }
    private void PresentCards()
    {
        CardToPick[] _cardsToPick = new CardToPick[_cardsToPickGO.Length];
        foreach (GameObject cardToPickGO_ in _cardsToPickGO)
        {
            cardToPickGO_.gameObject.SetActive(true);
        }        
        for (int i = 0; i < _cardsOfRarity.Length; i++)
        {
            _cardsToPick[i] = _cardsToPickGO[i].GetComponent<CardToPick>();
            _cardsToPick[i].SetCard(_cardsOfRarity[i]);
        }
        for (int j = _cardsOfRarity.Length; j < _cardsToPickGO.Length; j++)
        {
            _cardsToPickGO[j].SetActive(false);
        }

    }

    public void SaveCards()
    {
        TowerCardSO[] cards = new TowerCardSO[_cardsSlots.Length];
        List<TowerCardSO> cardsList = new List<TowerCardSO>();

        foreach (SelectedCard slottedCards in _cardsSlots)
        {
            if (slottedCards.slottedIn)
            {
                TowerCardSO card = slottedCards.card;
                cardsList.Add(card);
            }
        }
        cards = cardsList.ToArray();
        if (_manager.CheckCurrentTier(_tier))
        {
            _manager.SetCardsOfTier(cards, _tier);
        }       
    }


}
