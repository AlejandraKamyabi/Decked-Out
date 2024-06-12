using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] GameObject _gemPanel;
    [SerializeField] TextMeshProUGUI _gemAmountText;

    [SerializeField] GameObject _cardPanel;
    
    SaveSystem _saveSystem;

    private void Start()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
        UpdateUI();
    }

    public void OpenGemBuyingPanel()
    {
        _cardPanel.SetActive(false);
        _gemPanel.SetActive(true);
    }
    public void OpenCardPanel()
    {
        _cardPanel.SetActive(true);
        _gemPanel.SetActive(false);
    }

    public void UpdateUI()
    {
        _gemAmountText.text = _saveSystem.GetGemCount().ToString();
    }
}
