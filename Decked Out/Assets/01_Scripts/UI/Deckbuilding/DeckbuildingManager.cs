using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        _buttonScrips[0].SetTier();
    }

    public void OnStartButtonClicked()
    {
        
        var loadSceneTask = SceneManager.LoadSceneAsync(nextSceneName);
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
