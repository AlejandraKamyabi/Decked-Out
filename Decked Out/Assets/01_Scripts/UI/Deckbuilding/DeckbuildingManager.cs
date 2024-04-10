using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DeckbuildingManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "SampleScene";

    [SerializeField] CardTierSelector[] _buttonScrips;
    [SerializeField] TowerCardSO[] _common;
    [SerializeField] TowerCardSO[] _uncommon;
    [SerializeField] TowerCardSO[] _rare;
    [SerializeField] TowerCardSO[] _epic;
    [SerializeField] TowerCardSO[] _legendary;

    [SerializeField] string _currentTier;

    public TowerCardSO[] commonCards { get { return _common; } }
    public TowerCardSO[] uncommonCards { get { return _uncommon; } }
    public TowerCardSO[] rareCards { get { return _rare; } }
    public TowerCardSO[] epicCards { get { return _epic; } }
    public TowerCardSO[] legendaryCards { get { return _legendary; } }

    private GameLoader _loader;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        if(ServiceLocator.Contains<DeckbuildingManager>() == false)
        {
            ServiceLocator.Register<DeckbuildingManager>(this);
        }
        TutorialPassthrough tutorialPassthrough = FindObjectOfType<TutorialPassthrough>();
        if (tutorialPassthrough == null)
        {
            _buttonScrips[0].SetTier();
        }
        else
        {
            Debug.Log("Loading Tutorial");
        }
        
    }

    public void OnStartButtonClicked()
    {
        bool allTiersHaveCard = new[] {_common, _uncommon, _rare, _epic, _legendary}.All(array => array.Length > 0);
        if (allTiersHaveCard)
        {
            StartButtonLoading startButtonLoading = FindObjectOfType<StartButtonLoading>();
            startButtonLoading.DisableButton();
            var loadSceneTask = SceneManager.LoadSceneAsync(nextSceneName);
        }
        else
        {
            Debug.Log("Need a card slotted in each tier");
        }
        

    }
    public void LoadTutorialCards(TowerCardSO[] cards, string tier)
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
  
    public void SetCardsOfTier(TowerCardSO[] cards, string tier)
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
}
