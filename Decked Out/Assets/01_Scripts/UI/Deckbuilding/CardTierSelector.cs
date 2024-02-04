using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardTierSelector : MonoBehaviour
{
    [Header("Tier Data")]
    [SerializeField] Image _border;

    [Header("Card Data")]
    [SerializeField] SelectedCard[] _cardsSlots;
    [SerializeField] Image[] _cardBlanks;    
    [SerializeField] TowerCardSO[] _cardsOfRarity;

    CardToPick[] _cardsToPick;
    Color _rarityColour;
    public void SetTier()
    {
        _rarityColour = gameObject.GetComponent<Image>().color;
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
        }
        foreach (Image image in _cardBlanks)
        {
            image.enabled = true;
        }
        PresentCards();
    }
    private void PresentCards()
    {
        _cardsToPick = FindObjectsOfType<CardToPick>();
        for (int i = 0; i < _cardsToPick.Length; i++)
        {
            if (i < _cardsOfRarity.Length) // Check to avoid IndexOutOfRangeException
            {
                _cardsToPick[i].SetCard(_cardsOfRarity[i]);
            }
            else
            {
                // Optionally handle the case where _cardsOfRarity doesn't have an element for this index
                Debug.LogWarning($"No corresponding element in _cardsOfRarity for index {i}, skipping.");
            }
        }

    }

}
