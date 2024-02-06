using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardToPick : MonoBehaviour
{
    public TowerCardSO _card;

    [Header("Card Display")]
    [SerializeField] Image _background;
    [SerializeField] Image _image;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;

    public void UpdateUI()
    {
        if (_card != null)
        {
            _image.enabled = true;
        }
        else
        {
            _image.enabled = false;
        }
        _background.sprite = _card.background;
        _image.sprite = _card.image;
        _icon.sprite = _card.icon;
        _name.text = _card.name;
        _name.color = _card.rarityColor;
        
    }

    public void SetCard(TowerCardSO card)
    {
        _card = card;
        UpdateUI();
    }

    public void SlotIn()
    {
        SelectedCard[] selectedCardsArray = FindObjectsOfType<SelectedCard>();
        foreach (SelectedCard selectedCard in selectedCardsArray)
        {
            if (!selectedCard.slottedIn)
            {
                selectedCard.SlotInCard(_card);
            }
            else
            {
                continue;
            }
            break;
        }
    }
}
