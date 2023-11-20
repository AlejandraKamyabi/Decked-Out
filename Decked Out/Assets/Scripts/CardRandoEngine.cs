using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardRandoEngine : MonoBehaviour
{
    [Header("Game Tool")]
    public WaveManager waveManager;
    public TowerSelection towerSelection;

    [Header("Card Spaces")]
    public Button cardSpace0;
    public Button cardSpace1;
    public Button cardSpace2;
    public Button cardSpace3;
    public Button cardSpace4;
    public Button cardSpace5;
    public Button blockingButton;

    [Header("Hand Cards Data")]
    public int handSize;
   

    [Header("Button Input Modifers")]
    public List<Button> buttons = new List<Button>();
    public float longPressDuration = 1.0f;    

    [Header("Card 0 Data")]
    public TowerCardSO card0Data;
    public string card0Name;
    public Image card0TowerImage;
    public Image card0IconImage;
    public int card0TowerID;

    [Header("Card 1 Data")]
    public TowerCardSO card1Data;
    public string card1Name;
    public Image card1TowerImage;
    public Image card1IconImage;
    public int card1TowerID;

    [Header("Card 2 Data")]
    public TowerCardSO card2Data;
    public string card2Name;
    public Image card2TowerImage;
    public Image card2IconImage;

    public int card2TowerID;
    [Header("Card 3 Data")]
    public TowerCardSO card3Data;
    public string card3Name;
    public Image card3TowerImage;
    public Image card3IconImage;
    public int card3TowerID;

    [Header("Card 4 Data")]
    public TowerCardSO card4Data;
    public string card4Name;
    public Image card4TowerImage;
    public Image card4IconImage;
    public int card4TowerID;

    [Header("Card 5 Data")]
    public TowerCardSO card5Data;
    public string card5Name;
    public Image card5TowerImage;
    public Image card5IconImage;
    public int card5TowerID;


    [Header("Card Rando System")]
    public List<TowerCardSO> towerCards = new List<TowerCardSO>();
    public List<TowerCardSO> cardsInHand = new List<TowerCardSO>();

    [Header("Card Audio")]
    public AudioSource cardShuffle;

    private GameLoader _loader;
    private bool isSelectingTower;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
    {
    
        cardSpace0.gameObject.SetActive(false);
        cardSpace1.gameObject.SetActive(false);
        cardSpace2.gameObject.SetActive(false);
        cardSpace3.gameObject.SetActive(false);
        cardSpace4.gameObject.SetActive(false);
        cardSpace5.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(false);
        NewWave();
    }
    private void Update()
    {
        isSelectingTower = towerSelection.isSelectingTower;
        if (!isSelectingTower)
        {
            blockingButton.gameObject.SetActive(false);
        }
    }

    public void NewWave()
    {
        //cardsInHand.Clear();
        blockingButton.gameObject.SetActive(true);
        PlayCardSuffleSound();
        cardSpace0.gameObject.SetActive(true);
        cardSpace1.gameObject.SetActive(true);
        cardSpace2.gameObject.SetActive(true);
        cardSpace3.gameObject.SetActive(true);
        cardSpace4.gameObject.SetActive(true);
        cardSpace5.gameObject.SetActive(true);
        GetCards();
    }
    public void GetCards()
    {       
        GetRandomizedCards(handSize);
        GetCardData();
        ButtonData();
    }

    public void GetCardData()
    {
        card0Data = cardsInHand[0];
        card1Data = cardsInHand[1];
        card2Data = cardsInHand[2];
        card3Data = cardsInHand[3];
        card4Data = cardsInHand[4];
        card5Data = cardsInHand[5];
        //cardsInHand.Clear();

    }
    public void ButtonData()
    {
        cardSpace0.image.sprite = card0Data.background;
        card0TowerImage.sprite = card0Data.image;
        card0IconImage.sprite = card0Data.icon;
        card0TowerID = card0Data.towerID;

        cardSpace1.image.sprite = card1Data.background;
        card1TowerImage.sprite = card1Data.image;
        card1IconImage.sprite = card1Data.icon;
        card1TowerID = card1Data.towerID;

        cardSpace2.image.sprite = card2Data.background;
        card2TowerImage.sprite = card2Data.image;
        card2IconImage.sprite = card2Data.icon;
        card2TowerID = card2Data.towerID;

        cardSpace3.image.sprite = card3Data.background;
        card3TowerImage.sprite = card3Data.image;
        card3IconImage.sprite = card3Data.icon;
        card3TowerID = card3Data.towerID;

        cardSpace4.image.sprite = card4Data.background;
        card4TowerImage.sprite = card4Data.image;
        card4IconImage.sprite = card4Data.icon;
        card4TowerID = card4Data.towerID;

        cardSpace5.image.sprite = card5Data.background;
        card5TowerImage.sprite = card5Data.image;
        card5IconImage.sprite = card5Data.icon;
        card5TowerID = card5Data.towerID;
    }  

    public void Button0()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card0TowerID;
        cardSpace0.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);
    }   
    public void Button1()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card1TowerID;
        cardSpace1.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);
    }
    public void Button2()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card2TowerID;
        cardSpace2.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);
    }
    public void Button3()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card3TowerID;
        cardSpace3.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);
    }
    public void Button4()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card4TowerID;
        cardSpace4.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);

    }
    public void Button5()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card5TowerID;
        cardSpace5.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);

    }
    public List<TowerCardSO> GetRandomizedCards(int count)
    {
        List<TowerCardSO> remainingCards = new List<TowerCardSO>(towerCards);
        for (int i = 0; i < count; i++)
        {
            TowerCardSO randomCard = SelectRandomWeightedCard(remainingCards);
            cardsInHand.Add(randomCard);
        }

        return cardsInHand;
    }
    private TowerCardSO SelectRandomWeightedCard(List<TowerCardSO> cards)
    {
        float totalWeight = 0f;

        foreach (TowerCardSO card in cards)
        {
            totalWeight += card.rarityWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        foreach (TowerCardSO card in cards)
        {
            if (randomValue <= card.rarityWeight)
            {
                return card;
            }

            randomValue -= card.rarityWeight;
        }

        return null;
    }
    public void PlayCardSuffleSound()
    {
        if (!cardShuffle.isPlaying)
        {
            cardShuffle.Play();
        }
    }
   
   
}
