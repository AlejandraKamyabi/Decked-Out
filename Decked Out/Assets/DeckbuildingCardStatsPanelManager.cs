using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckbuildingCardStatsPanelManager : MonoBehaviour
{
    [SerializeField] TowerCardSO _card;
    [SerializeField] float _sliderCheat;

    [Header("Display Stats")]
    [SerializeField] Image _image;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;

    [Header("Sliders")]
    [SerializeField] Slider _dmgSlider;
    [SerializeField] Image _dmgFill;
    [SerializeField] TextMeshProUGUI _dmgText;
    [SerializeField] Image _rangeFill;
    [SerializeField] Slider _rangeSlider;
    [SerializeField] TextMeshProUGUI _rangeText;
    [SerializeField] Slider _rofSlider;
    [SerializeField] Image _rofFill;
    [SerializeField] TextMeshProUGUI _rofText;
    [SerializeField] Slider _durationSlider;
    [SerializeField] Image _durationFill;
    [SerializeField] TextMeshProUGUI _durationText;

    Color _rarityColour;
    public void Awake()
    {
        this.gameObject.SetActive(false);
    }
    public void EnableAndFillStatsPanel(TowerCardSO card)
    {
        _card = card;
        this.gameObject.SetActive(true);
        Debug.Log("Slotting in " + card.name + " to deckbuilding stats panel.");
        _image.sprite = _card.image;
        _icon.sprite = _card.icon;
        _name.text = _card.name;
        _name.color = _rarityColour;

        _rarityColour = _card.rarityColor;

        _dmgSlider.value = (_card.damage / 25) + _sliderCheat;
        _dmgText.text = _card.damage.ToString();
        _dmgFill.color = _rarityColour;

        _rangeSlider.value = (_card.range / 5) + _sliderCheat;
        _rangeText.text = _card.range.ToString();
        _rangeFill.color = _card.rarityColor;

        _rofSlider.value = (_card.rateOfFire / 10) + _sliderCheat;
        _rofText.text = _card.rateOfFire.ToString();
        _rofFill.color = _rarityColour;

        _durationSlider.value = (_card.duration / 10) + _sliderCheat;
        _durationText.text = _card.duration.ToString();
        _durationFill.color = _rarityColour;
    }
    public void DisableStatsPanel()
    {
        this.gameObject.SetActive(false);
    }
}
