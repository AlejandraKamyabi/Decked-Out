using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedCard : MonoBehaviour
{
    [SerializeField] TowerCardSO _card;

    [Header("Card Base Display")]
    [SerializeField] Image _background;
    [SerializeField] Image _image;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;

    [Header("Card Stats Display")]
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
    [SerializeField] float _sliderCheat = 0.1f;

    bool _slottedIn;
    Color _rarityColour;

    public TowerCardSO card { get { return _card; } }
    public bool slottedIn { get { return _slottedIn; } }

    GameLoader _loader;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);

    }
    private void Initialize()
    {
     
        //DisableUI();
    }

    public void SlotInCard(TowerCardSO card)
    {
        if (!_slottedIn)
        {
            Debug.Log(card + " :slotted in");
            _card = card;
            _slottedIn = true;
            UpdateUI();
        }
       
    }
    public void Unslot()
    {
        _card = null;
        _slottedIn = false;
        DisableUI();
    }
    private void UpdateUI()
    {
        _rarityColour = _card.rarityColor;
        _background.enabled = true;
        _image.enabled = true;
        _icon.enabled = true;
        _name.enabled = true;
        _dmgText.enabled = true;
        _rangeText.enabled = true;
        _rofText.enabled = true;
        _durationText.enabled = true;
        _background.sprite = _card.background;
        _image.sprite = _card.image;
        _icon.sprite = _card.icon;
        _name.text = _card.name;
        _name.color = _rarityColour;

        _dmgSlider.value = (_card.damage / 25) + _sliderCheat;
        _dmgText.text = _card.damage.ToString();
        _dmgText.color = _rarityColour;
        _dmgFill.color = _rarityColour;

        _rangeSlider.value = (_card.range / 5) + _sliderCheat;
        _rangeText.text = _card.range.ToString();
        _rangeFill.color = _card.rarityColor;
        _rangeText.color = _rarityColour;

        _rofSlider.value = (_card.rateOfFire / 10) + _sliderCheat;
        _rofText.text = _card.rateOfFire.ToString();
        _rofText.color = _rarityColour;
        _rofFill.color = _rarityColour;

        _durationSlider.value = (_card.duration / 10) + _sliderCheat;
        _durationText.text = _card.duration.ToString();
        _durationText.color = _rarityColour;
        _durationFill.color = _rarityColour;
    }
    private void DisableUI()
    {
        _background.enabled = false;
        _image.enabled = false;
        _icon.enabled = false;
        _name.enabled = false;

        _dmgSlider.value = 0f;
        _dmgText.enabled = false;
        _rangeSlider.value = 0f;
        _rangeText.enabled = false;
        _rofSlider.value = 0f;
        _rofText.enabled = false;
        _durationSlider.value = 0f;
        _durationText.enabled = false;
    }
    public void SelectCard()
    {

    }
}
