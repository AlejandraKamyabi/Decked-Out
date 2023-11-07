using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRandoEngine : MonoBehaviour
{
    [Header("Game Tool")]
    public WaveManager waveManager;
    public TowerSelection towerSelection;

    [Header("UI Stuff")]
    public Button cardSpace0;
    public Button cardSpace1;
    public Button cardSpace2;
    public Button cardSpace3;
    public Button cardSpace4;

    [Header("Hand Cards Data")]
    public int handSize;
    [Header("Card 0 Data")]
    public TowerCardSO card0Data;
    public string card0Name;
    public Image card0TowerImage;
    public int card0TowerID;
    [Header("Card 1 Data")]
    public TowerCardSO card1Data;
    public string card1Name;
    public Image card1TowerImage;
    public int card1TowerID;
    [Header("Card 2 Data")]
    public TowerCardSO card2Data;
    public string card2Name;
    public Image card2TowerImage;
    public int card2TowerID;
    [Header("Card 3 Data")]
    public TowerCardSO card3Data;
    public string card3Name;
    public Image card3TowerImage;
    public int card3TowerID;
    [Header("Card 4 Data")]
    public TowerCardSO card4Data;
    public string card4Name;
    public Image card4TowerImage;
    public int card4TowerID;


    [Header("Card Rando System")]
    public List<TowerCardSO> towerCards = new List<TowerCardSO>();
    List<TowerCardSO> cardsInHand = new List<TowerCardSO>();


    public void Start()
    {
        cardSpace0.gameObject.SetActive(false);
        cardSpace1.gameObject.SetActive(false);
        cardSpace2.gameObject.SetActive(false);
        cardSpace3.gameObject.SetActive(false);
        cardSpace4.gameObject.SetActive(false);
    }
    public void GetCards()
    {
        towerCards.Clear();
        cardsInHand.Clear();
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

    }
    public void ButtonData()
    {
        cardSpace0.image.sprite = card0Data.background;
        card0TowerImage.sprite = card0Data.image;
        card0TowerID = card0Data.towerID;

        cardSpace1.image.sprite = card1Data.background;
        card1TowerImage.sprite = card1Data.image;
        card1TowerID = card1Data.towerID;

        cardSpace2.image.sprite = card2Data.background;
        card2TowerImage.sprite = card2Data.image;
        card2TowerID = card2Data.towerID;

        cardSpace3.image.sprite = card3Data.background;
        card3TowerImage.sprite = card3Data.image;
        card3TowerID = card3Data.towerID;

        cardSpace4.image.sprite = card4Data.background;
        card4TowerImage.sprite = card4Data.image;
        card4TowerID = card4Data.towerID;
    }

    public void Button0()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card0TowerID;
        cardSpace0.gameObject.SetActive(false);
    }
    public void Button1()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card1TowerID;
        cardSpace1.gameObject.SetActive(false);
    }
    public void Button2()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card2TowerID;
        cardSpace2.gameObject.SetActive(false);
    }
    public void Button3()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card3TowerID;
        cardSpace3.gameObject.SetActive(false);
    }
    public void Button4()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card4TowerID;
        cardSpace4.gameObject.SetActive(false);

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
   
    public void NewWave()
    {
        GetCards();
        cardSpace0.gameObject.SetActive(true);
        cardSpace1.gameObject.SetActive(true);
        cardSpace2.gameObject.SetActive(true);
        cardSpace3.gameObject.SetActive(true);
        cardSpace4.gameObject.SetActive(true);
        
    }
}
