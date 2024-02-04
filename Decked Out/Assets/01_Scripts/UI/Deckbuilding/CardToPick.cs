using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardToPick : MonoBehaviour
{
    [SerializeField] TowerCardSO _card;

    [Header("Card Display")]
    [SerializeField] Image _background;
    [SerializeField] Image _image;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;


    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        _background.sprite = _card.background;
        _image.sprite = _card.image;
        _icon.sprite = _card.icon;
        _name.text = _card.name;
        _name.color = _card.rarityColor;
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
