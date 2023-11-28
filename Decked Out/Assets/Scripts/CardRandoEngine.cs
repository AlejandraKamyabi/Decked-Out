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
    public Transform leftSpot;
    public Vector3 leftSpotScale;
    public Transform bottomSpot;
    public Vector3 bottomSpotScale;
    public float delayTimer;
    public Slider timerSlider;
    public bool cardsOnLeft;

    [Header("Card Spaces")]
    public GameObject cardSpace0;   
    public GameObject cardSpace1;
    public GameObject cardSpace2;
    public GameObject cardSpace3;
    public GameObject cardSpace4; 
    public GameObject blockingButton;

    [Header("Hand Cards Data")]
    public int handSize;
   

    [Header("Button Input Modifers")]
    public List<Button> buttons = new List<Button>();
    public float longPressDuration = 1.0f;    

    [Header("Card 0 Data")]
    public TowerCardSO card0Data;
    public Image cardSpace0Background;
    public string card0Name;
    public Image card0TowerImage;
    public Image card0IconImage;
    public int card0TowerID;
    public bool card0Used;


    [Header("Card 1 Data")]
    public TowerCardSO card1Data;
    public Image cardSpace1Background;
    public string card1Name;
    public Image card1TowerImage;
    public Image card1IconImage;
    public int card1TowerID;
    public bool card1Used;

    [Header("Card 2 Data")]
    public TowerCardSO card2Data;
    public Image cardSpace2Background;
    public string card2Name;
    public Image card2TowerImage;
    public Image card2IconImage;
    public int card2TowerID;
    public bool card2Used;

    [Header("Card 3 Data")]
    public TowerCardSO card3Data;
    public Image cardSpace3Background;
    public string card3Name;
    public Image card3TowerImage;
    public Image card3IconImage;
    public int card3TowerID;
    public bool card3Used;

    [Header("Card 4 Data")]
    public TowerCardSO card4Data;
    public Image cardSpace4Background;
    public string card4Name;
    public Image card4TowerImage;
    public Image card4IconImage;
    public int card4TowerID;
    public bool card4Used;



    [Header("Card Rando System")]
    public List<TowerCardSO> towerCards = new List<TowerCardSO>();
    public List<TowerCardSO> cardsInHand = new List<TowerCardSO>();

    [Header("Card Audio")]
    public AudioSource cardShuffle;

    private GameLoader _loader;
    private bool isSelectingTower;
    private float timer;
    private bool timerOn = false;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
    {
        transform.position = bottomSpot.position;
        transform.rotation = bottomSpot.rotation;
        cardSpace0.gameObject.SetActive(false);
        cardSpace1.gameObject.SetActive(false);
        cardSpace2.gameObject.SetActive(false);
        cardSpace3.gameObject.SetActive(false);
        cardSpace4.gameObject.SetActive(false);       
        blockingButton.gameObject.SetActive(false);
        timerSlider.gameObject.SetActive(false);
        timer = delayTimer;
        NewWave();
    }
    private void Update()
    {
        isSelectingTower = towerSelection.isSelectingTower;
        if (!isSelectingTower)
        {
            blockingButton.gameObject.SetActive(false);
        }
        if (timerOn)
        {
            
            if (!card0Used || !card1Used || !card2Used || !card3Used || !card4Used) 
            {
                timerSlider.gameObject.SetActive(true);
                timer -= Time.deltaTime;
                timerSlider.value = timer / delayTimer;
                if (timer <= 0)
                {
                    timerOn = false;
                    timerSlider.gameObject.SetActive(false);
                    MoveToLeft();

                    timer = delayTimer;
                }
            }
            else if (card0Used && card1Used && card2Used && card3Used && card4Used)
            {
                timerOn = false;
                timerSlider.gameObject.SetActive(false);
                MoveToLeft();
                timer = delayTimer;
            }

        }
       
    }

    public void NewWave()
    {
        cardsInHand.Clear();
        blockingButton.gameObject.SetActive(true);
        PlayCardSuffleSound();
        cardSpace0.gameObject.SetActive(true);
        cardSpace1.gameObject.SetActive(true);
        cardSpace2.gameObject.SetActive(true);
        cardSpace3.gameObject.SetActive(true);
        cardSpace4.gameObject.SetActive(true);      
        GetCards();
        card0Used = false;
        card1Used = false;
        card2Used = false;
        card3Used = false;
        card4Used = false;
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
        //cardsInHand.Clear();

    }
    public void ButtonData()
    {
        cardSpace0Background.sprite = card0Data.background;
        card0TowerImage.sprite = card0Data.image;
        card0IconImage.sprite = card0Data.icon;
        card0TowerID = card0Data.towerID;

        cardSpace1Background.sprite = card1Data.background;
        card1TowerImage.sprite = card1Data.image;
        card1IconImage.sprite = card1Data.icon;
        card1TowerID = card1Data.towerID;

        cardSpace2Background.sprite = card2Data.background;
        card2TowerImage.sprite = card2Data.image;
        card2IconImage.sprite = card2Data.icon;
        card2TowerID = card2Data.towerID;

        cardSpace3Background.sprite = card3Data.background;
        card3TowerImage.sprite = card3Data.image;
        card3IconImage.sprite = card3Data.icon;
        card3TowerID = card3Data.towerID;

        cardSpace4Background.sprite = card4Data.background;
        card4TowerImage.sprite = card4Data.image;
        card4IconImage.sprite = card4Data.icon;
        card4TowerID = card4Data.towerID;
       
    }  

    public void Button0()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card0TowerID;
        cardSpace0.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);
        card0Used = true;
    }   
    public void Button1()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card1TowerID;
        cardSpace1.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);
        card1Used = true;
    }
    public void Button2()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card2TowerID;
        cardSpace2.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);
        card2Used = true;
    }
    public void Button3()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card3TowerID;
        cardSpace3.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);
        card3Used = true;
    }
    public void Button4()
    {
        towerSelection.SelectTower();
        towerSelection.tower = card4TowerID;
        cardSpace4.gameObject.SetActive(false);
        blockingButton.gameObject.SetActive(true);
        card4Used = true;

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
    public void StartMoveToLeft()
    {
        timerOn = true;
    }

    public void MoveToLeft()
    {
        cardsOnLeft = true;
        timerSlider.gameObject.SetActive(false);
        cardsInHand.Clear();
        NewWave();
        transform.position = leftSpot.position;
        transform.rotation = leftSpot.rotation;
        transform.localScale = leftSpotScale;
    }
   
    public void MoveToBottom()
    {
        cardsOnLeft = false;
        transform.position = bottomSpot.position;
        transform.rotation = bottomSpot.rotation;
        transform.localScale = bottomSpotScale;
    }
   
   
}
