using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DeckbuildingManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "SampleScene";

    [SerializeField] CardTierSelector[] _buttonScrips;
    [SerializeField] GameObject[] _cardRenderers;
    [SerializeField] Button _startButton;
    [SerializeReference] PillarIndicatorManager _pillarIndicatorManager;

    [SerializeField] List<TowerCardSO> _common;
    [SerializeField] List<TowerCardSO> _uncommon;
    [SerializeField] List<TowerCardSO> _rare;
    [SerializeField] List<TowerCardSO> _epic;
    [SerializeField] List<TowerCardSO> _legendary;
    [SerializeField] string _currentTier;

    public Sprite _glowBorder;
    bool allTiersHaveCard;
    public TowerCardSO[] commonCards { get { return _common.ToArray(); } }
    public TowerCardSO[] uncommonCards { get { return _uncommon.ToArray(); } }
    public TowerCardSO[] rareCards { get { return _rare.ToArray(); } }
    public TowerCardSO[] epicCards { get { return _epic.ToArray(); } }
    public TowerCardSO[] legendaryCards { get { return _legendary.ToArray(); } }

    public CardTierSelector[] tierButtons { get {  return _buttonScrips; } }

    private GameLoader _loader;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);

    }
    private void Update()
    {
        if (allTiersHaveCard)
        {
            _startButton.gameObject.SetActive(true);
            _startButton.enabled = true;
        }
        else if (!allTiersHaveCard && _startButton.isActiveAndEnabled)
        {
            _startButton.gameObject.SetActive(false);
            _startButton.enabled = false;
        }
    }
    private void Initialize()
    {
        //DontDestroyOnLoad(gameObject);
        if(ServiceLocator.Contains<DeckbuildingManager>() == false)
        {
            ServiceLocator.Register<DeckbuildingManager>(this);
        }
        TutorialPassthrough tutorialPassthrough = FindObjectOfType<TutorialPassthrough>();
        if (tutorialPassthrough == null)
        {
            Button commonButton = _buttonScrips[0].gameObject.GetComponent<Button>();
            commonButton.Select();
            commonButton.onClick.Invoke();
        }
        else
        {
            Debug.Log("Loading Tutorial");
        }
        _startButton.enabled = false;
        _startButton.gameObject.SetActive(false);

    }
    public SelectedCard[] SetTierRenderers(int cardsInTier, string tier)
    {
        _currentTier = tier;
        Debug.Log("Current Tier: " + tier);
        SelectedCard[] activeRenderers = new SelectedCard[cardsInTier];
        foreach (GameObject cardRenderer in _cardRenderers)
        {
            cardRenderer.SetActive(false);
        }
        for (int i = 0; i < cardsInTier; i++)
        {
            if (i <= cardsInTier)
            {
                _cardRenderers[i].SetActive(true);
                activeRenderers[i] = _cardRenderers[i].GetComponent<SelectedCard>();
                activeRenderers[i].Unslot();
            }
            else
            {
                Debug.LogError("No Cards in Tier - Check the Button");
            }

        }
        _pillarIndicatorManager.SetTier(_currentTier);
        return activeRenderers.ToArray();
    }

    public void OnStartButtonClicked()
    {
        TransitionScreenManager transitionScreenManager = FindObjectOfType<TransitionScreenManager>();
        transitionScreenManager.StartTranistion(nextSceneName);
        StartButtonLoading startButtonLoading = FindObjectOfType<StartButtonLoading>();
        startButtonLoading.DisableButton();
        //var loadSceneTask = SceneManager.LoadSceneAsync(nextSceneName);
    }
    public void LoadTutorialCards(TowerCardSO[] cards, string tier)
    {
        if (tier == "Common")
        {
            _common = cards.ToList();
        }
        if (tier == "Uncommon")
        {
            _uncommon = cards.ToList();
        }
        if (tier == "Rare")
        {
            _rare = cards.ToList();
        }
        if (tier == "Epic")
        {
            _epic = cards.ToList();
        }
        if (tier == "Legendary")
        {
            _legendary = cards.ToList();
        }        
    }
    public void AddCard(TowerCardSO card)
    {
        if(_currentTier == "Common")
        {
            _common.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _common.Count);
            Debug.Log(card + " added to manager.");
            if (_common.Count >= 4)
            {
                ActivateGreyOut(_common);
            }
        }
        else if (_currentTier == "Uncommon")
        {
            _uncommon.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _uncommon.Count);
            Debug.Log(card + " added to manager.");
            if (_uncommon.Count >= 4)
            {
                ActivateGreyOut(_uncommon);
            }
        }
        else if (_currentTier == "Rare")
        {
            _rare.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _rare.Count);
            Debug.Log(card + " added to manager.");
            if (_rare.Count >= 3)
            {
                ActivateGreyOut(_rare);
            }
        }
        else if (_currentTier == "Epic")
        {
            _epic.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _epic.Count);
            Debug.Log(card + " added to manager.");
            if (_epic.Count >= 3)
            {
                ActivateGreyOut(_epic);
            }
        }
        else if (_currentTier == "Legendary")
        {
            _legendary.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _legendary.Count);
            Debug.Log(card + " added to manager.");
            if (_legendary.Count >= 2)
            {
                ActivateGreyOut(_legendary);
            }
        }
        allTiersHaveCard = new[] { _common, _uncommon, _rare, _epic, _legendary }.All(array => array.Count > 0);
    }
    public void RemoveCard(TowerCardSO card)
    {
        if (_currentTier == "Common")
        {
            _common.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _common.Count);
            Debug.Log(card + " removed from manager.");
            if (_common.Count !<= 4)
            {
                DeactivateGreyOut();
            }
        }
        else if (_currentTier == "Uncommon")
        {
            _uncommon.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _uncommon.Count);

            Debug.Log(card + " removed from manager.");
            if (_uncommon.Count !<= 4)
            {
                DeactivateGreyOut();
            }
        }
        else if (_currentTier == "Rare")
        {
            _rare.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _rare.Count);

            Debug.Log(card + " removed from manager.");
            if (_rare.Count !<= 3)
            {
                DeactivateGreyOut();
            }
        }
        else if (_currentTier == "Epic")
        {
            _epic.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _epic.Count);

            Debug.Log(card + " removed from manager.");
            if (_epic.Count !<= 3)
            {
                DeactivateGreyOut();
            }
        }
        else if (_currentTier == "Legendary")
        {
            _legendary.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _legendary.Count);

            Debug.Log(card + " removed from manager.");
            if (_legendary.Count !<= 2)
            {
                DeactivateGreyOut();
            }
        }
        allTiersHaveCard = new[] { _common, _uncommon, _rare, _epic, _legendary }.All(array => array.Count > 0);
    }

    public bool CheckCurrentTier(string tier)
    {
        if (tier == _currentTier)
        {
            return false;
        }
        else
        {
            _currentTier = tier;
            return true;
        }
    }
  
    public void SetCardsOfTier(List<TowerCardSO> cards, string tier)
    {
        if (tier == "Common")
        {
            _common = cards;
        }
        if (tier == "Uncommon")
        {
            _uncommon = cards;
        }
        if (tier == "Rare")
        {
            _rare = cards;
        }
        if (tier == "Epic")
        {
            _epic = cards;
        }
        if (tier == "Legendary")
        {
            _legendary = cards;
        }
    }
    public TowerCardSO[] CheckIfSavedCards(string tier)
    {
        if (tier == "Common")
        {
            return commonCards;
        }
        if (tier == "Uncommon")
        {
            return uncommonCards;
        }
        if (tier == "Rare")
        {
            return rareCards;
        }
        if (tier == "Epic")
        {
            return epicCards;
        }
        if (tier == "Legendary")
        {
            return legendaryCards;
        }
        else
        {
            return null;
        }
    }
    public void ActivateGreyOut(List<TowerCardSO> cardList)
    {
        foreach (GameObject CardRenderer in _cardRenderers)
        {
            if (CardRenderer.activeInHierarchy)
            {
                SelectedCard script = CardRenderer.GetComponent<SelectedCard>();
                if (!script.selected && !cardList.Contains(script.card))
                {
                    script.GreyOut(true);
                }
            }

        }
    }
    public void DeactivateGreyOut()
    {
        foreach (GameObject CardRenderer in _cardRenderers)
        {
            if (CardRenderer.activeInHierarchy)
            {
                SelectedCard script = CardRenderer.GetComponent<SelectedCard>();
                script.GreyOut(false);
            }

        }
    }
}
