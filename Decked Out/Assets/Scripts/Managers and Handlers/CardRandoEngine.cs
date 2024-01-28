using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    [Header("Card Stats Panel")]
    public GameObject cardStatsPanel;
    public Image cardStatsBackground;
    public Image cardStatsImage;
    public Image cardStatsIcon;
    public TextMeshProUGUI cardStatsTitleText;
    public TextMeshProUGUI cardStatsInfoText;

    public Slider dmgSlider;
    public TextMeshProUGUI dmgText;
    public Slider rangeSlider;
    public TextMeshProUGUI rangeText;
    public Slider rofSlider;
    public TextMeshProUGUI rofText;
    public Slider durationSlider;
    public TextMeshProUGUI durationText;
    public float sliderCheat;

    [Header("Hand Cards Data")]
    public int handSize;
    public int spellUses = 4;
   

    [Header("Button Input Modifers")]
    public List<Button> buttons = new List<Button>();
    public float longPressDuration = 1.0f;    

    [Header("Card 0 Data")]
    public TowerCardSO card0Data;
    public Image cardSpace0Background;
    public string card0Name;
    public TextMeshProUGUI card0SpellUsesText;
    public Image card0TowerImage;
    public Image card0IconImage;
    public int card0TowerID;
    public bool card0Used;
    private int spell0Usage;


    [Header("Card 1 Data")]
    public TowerCardSO card1Data;
    public Image cardSpace1Background;
    public string card1Name;
    public TextMeshProUGUI card1SpellUsesText;
    public Image card1TowerImage;
    public Image card1IconImage;
    public int card1TowerID;
    public bool card1Used;
    private int spell1Usage;

    [Header("Card 2 Data")]
    public TowerCardSO card2Data;
    public Image cardSpace2Background;
    public string card2Name;
    public TextMeshProUGUI card2SpellUsesText;
    public Image card2TowerImage;
    public Image card2IconImage;
    public int card2TowerID;
    public bool card2Used;
    private int spell2Usage;

    [Header("Card 3 Data")]
    public TowerCardSO card3Data;
    public Image cardSpace3Background;
    public string card3Name;
    public TextMeshProUGUI card3SpellUsesText;
    public Image card3TowerImage;
    public Image card3IconImage;
    public int card3TowerID;
    public bool card3Used;
    private int spell3Usage;

    [Header("Card 4 Data")]
    public TowerCardSO card4Data;
    public Image cardSpace4Background;
    public string card4Name;
    public TextMeshProUGUI card4SpellUsesText;
    public Image card4TowerImage;
    public Image card4IconImage;
    public int card4TowerID;
    public bool card4Used;
    private int spell4Usage;



    [Header("Card Rando System")]
    public float totalWeight;
    public List<TowerCardSO> towerCards = new List<TowerCardSO>();
    public List<TowerCardSO> cardsInHand = new List<TowerCardSO>();

    [Header("Card Audio")]
    public AudioSource cardShuffle;

    private GameLoader _loader;
    private bool isSelectingTower;
    private float timer;
    private bool timerOn = false;
    private bool isButtonHeld = false;
    private float buttonHeldTime = 0f;
  
    float scale;
    private int current_Button_Held;

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
        spell0Usage =0;   
        spell1Usage=0;
        spell2Usage=0;
        spell3Usage=0;
        spell4Usage=0;
          
        blockingButton.gameObject.SetActive(false);
        timerSlider.gameObject.SetActive(false);
        timer = delayTimer;        
        NewWave();
        MoveToBottom();
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
        if (isButtonHeld)
        {
            buttonHeldTime += Time.deltaTime;
            Debug.Log("Button Held for:" + buttonHeldTime);
            if (buttonHeldTime >= longPressDuration)
            {
                switch(current_Button_Held)
                {
                    case 0:
                        ButtonStats(card0Data);
                        break;
                    case 1:
                        ButtonStats(card1Data);
                        break;
                    case 2:
                        ButtonStats(card2Data);
                        break;
                    case 3:
                        ButtonStats(card3Data);
                        break;
                    case 4:
                        ButtonStats(card4Data);
                        break;
                }
                
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
        spell0Usage = 0;
        card1Used = false;
        spell1Usage = 0;
        card2Used = false;
        spell2Usage = 0;
        card3Used = false;
        spell3Usage = 0;
        card4Used = false;
        spell4Usage = 0;
        
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
        card0Name = card0Data.towerName;
           

        cardSpace1Background.sprite = card1Data.background;
        card1TowerImage.sprite = card1Data.image;
        card1IconImage.sprite = card1Data.icon;
        card1Name = card1Data.towerName;
       

        cardSpace2Background.sprite = card2Data.background;
        card2TowerImage.sprite = card2Data.image;
        card2IconImage.sprite = card2Data.icon;
        card2Name = card2Data.towerName;
      

        cardSpace3Background.sprite = card3Data.background;
        card3TowerImage.sprite = card3Data.image;
        card3IconImage.sprite = card3Data.icon;
        card3Name = card3Data.towerName;
        

        cardSpace4Background.sprite = card4Data.background;
        card4TowerImage.sprite = card4Data.image;
        card4IconImage.sprite = card4Data.icon;
        card4Name = card4Data.towerName;
        


    }  


    public void Button0DragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 0)
        {
            PlaceButton0();
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }
    public void Button1ragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 1)
        {
            PlaceButton1();
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }
    public void Button2DragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 2)
        {
            PlaceButton2();
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }
    public void Button3DragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 3)
        {
            PlaceButton3();
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }
    public void Button4DragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 4)
        {
            PlaceButton4();
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }

    public void Button0()
    {
        Debug.Log("Button0 Clicked");
        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 0;
    }

    public void Button1()
    {
        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 1;

    }
    public void Button2()
    {
        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 2;

    }
    public void Button3()
    {
        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 3;

    }
    public void Button4()
    {

        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 4;
    }

    public void PlaceButton0()
    {      
      
        if (card0Name == "Lightning")
        {
            towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card0Name;
        }
        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card0Name;
        }

        if (card0Name != "Lightning")
        {
            cardSpace0.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card0Used = true;
        }
        else if (card0Name == "Lightning")
        {
            spell0Usage++;
            if (spell0Usage == 1)
            {
                card0SpellUsesText.text = "III";
            }
            if (spell0Usage == 2)
            {
                card0SpellUsesText.text = "II";
            }
            if (spell0Usage == 3)
            {
                card0SpellUsesText.text = "I";
            }
            if (spell0Usage == 4)
            {

                spell0Usage = 0;
                cardSpace0.gameObject.SetActive(false);
                blockingButton.gameObject.SetActive(true);
                card0Used = true;
            }
        }
    }

    private void ButtonStats(TowerCardSO cardDataToShow)
    {
        isButtonHeld = false;
        buttonHeldTime = 0;

        cardStatsPanel.gameObject.SetActive(true);

        Image dmgFillImage = dmgSlider.fillRect.GetComponent<Image>();
        Image rangeFillImage = rangeSlider.fillRect.GetComponent<Image>();
        Image rofFillImage = rofSlider.fillRect.GetComponent<Image>();
        Image durationFillImage = durationSlider.fillRect.GetComponent<Image>();


        cardStatsBackground.sprite = cardDataToShow.background;
        cardStatsImage.sprite = cardDataToShow.image;
        cardStatsIcon.sprite = cardDataToShow.icon;
        cardStatsTitleText.text = cardDataToShow.name;
        cardStatsTitleText.color = cardDataToShow.rarityColor;
        cardStatsInfoText.text = cardDataToShow.towerInfo;
        cardStatsInfoText.color = cardDataToShow.rarityColor;

        dmgSlider.value = (cardDataToShow.damage / 25) + sliderCheat;
        dmgText.text = cardDataToShow.damage.ToString();
        dmgFillImage.color = cardDataToShow.rarityColor;
        dmgText.color = cardDataToShow.rarityColor;

        rangeSlider.value = (cardDataToShow.range / 5) + sliderCheat;
        rangeText.text = cardDataToShow.range.ToString();
        rangeFillImage.color = cardDataToShow.rarityColor;
        rangeText.color = cardDataToShow.rarityColor;

        rofSlider.value = (cardDataToShow.rateOfFire / 10) + sliderCheat;
        rofText.text = cardDataToShow.rateOfFire.ToString();
        rofFillImage.color = cardDataToShow.rarityColor;
        rofText.color = cardDataToShow.rarityColor;

        durationSlider.value = (cardDataToShow.duration / 10) + sliderCheat;
        durationText.text = cardDataToShow.duration.ToString();
        durationFillImage.color = cardDataToShow.rarityColor;
        durationText.color = cardDataToShow.rarityColor;

        Debug.Log("Card Stats Panel Open");
    }
   
    public void PlaceButton1()
    {
        if (card1Name == "Lightning")
        {
            towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card1Name;
        }
        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card1Name;
        }

        if (card1Name != "Lightning")
        {
            cardSpace1.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card1Used = true;
        }
        else if (card1Name == "Lightning")
        {
            spell1Usage++;
            if (spell1Usage == 1)
            {
                card1SpellUsesText.text = "III";
            }
            if (spell1Usage == 2)
            {
                card1SpellUsesText.text = "II";
            }
            if (spell1Usage == 3)
            {
                card1SpellUsesText.text = "I";
            }
            if (spell1Usage == 4)
            {
                spell1Usage = 0;
                cardSpace1.gameObject.SetActive(false);
                blockingButton.gameObject.SetActive(true);
                card1Used = true;
            }
        }
    }
    public void PlaceButton2()
    {
        if (card2Name == "Lightning")
        {
            towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card2Name;
        }
        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card2Name;
        }
        if (card2Name != "Lightning")
        {
            cardSpace2.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card2Used = true;
        }
        else if (card2Name == "Lightning")
        {
            spell2Usage++;
            if (spell2Usage == 1)
            {
                card2SpellUsesText.text = "III";
            }
            if (spell2Usage == 2)
            {
                card2SpellUsesText.text = "II";
            }
            if (spell2Usage == 3)
            {
                card2SpellUsesText.text = "I";
            }
            if (spell2Usage == 4)
            {
                spell2Usage = 0;
                cardSpace2.gameObject.SetActive(false);
                blockingButton.gameObject.SetActive(true);
                card2Used = true;
            }
        }
    }
    public void PlaceButton3()
    {
        if (card3Name == "Lightning")
        {
            towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card3Name;
        }
        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card3Name;
        }
        if (card3Name != "Lightning")
        {
            cardSpace3.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card3Used = true;
        }
        else if (card3Name == "Lightning")
        {
            spell3Usage++;
            if (spell3Usage == 1)
            {
                card3SpellUsesText.text = "III";
            }
            if (spell3Usage == 2)
            {
                card3SpellUsesText.text = "II";
            }
            if (spell3Usage == 3)
            {
                card3SpellUsesText.text = "I";
            }
            if (spell3Usage == 4)
            {
                spell3Usage = 0;
                cardSpace3.gameObject.SetActive(false);
                blockingButton.gameObject.SetActive(true);
                card3Used = true;
            }
        }
    }
    public void PlaceButton4()
    {
        if (card4Name == "Lightning")
        {
            towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card4Name;
        }
        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card4Name;
        }

        if (card4Name != "Lightning")
        {
            cardSpace4.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card4Used = true;
        }
        else if (card4Name == "Lightning")
        {
            spell4Usage++;
            if (spell4Usage == 1)
            {
                card4SpellUsesText.text = "III";
            }
            if (spell4Usage == 2)
            {
                card4SpellUsesText.text = "II";
            }
            if (spell4Usage == 3)
            {
                card4SpellUsesText.text = "I";
            }
            if (spell4Usage == 4)
            {
                spell4Usage = 0;
                cardSpace4.gameObject.SetActive(false);
                blockingButton.gameObject.SetActive(true);
                card4Used = true;
            }
        }
    }    
  

    public void ExitCardStatsPanel()
    {
        cardStatsPanel.gameObject.SetActive(false);
    }
 

    public List<TowerCardSO> GetRandomizedCards(int count)
    {
        List<TowerCardSO> cardsToShuffle = new List<TowerCardSO>(towerCards);
        totalWeight = 0;
        foreach (TowerCardSO card in cardsToShuffle)
        {
            totalWeight += card.rarityWeight;
        }

        Debug.Log("Cards in Deck: " + cardsToShuffle.Count);
        Debug.Log("Total Weight: " + totalWeight);

        for (int i = 0; i < count; i++)
        {            
            TowerCardSO randomCard = SelectRandomWeightedCard(cardsToShuffle);
            cardsInHand.Add(randomCard);
        }

        Debug.Log(cardsInHand.Count);
        return cardsInHand;
    }
    private TowerCardSO SelectRandomWeightedCard(List<TowerCardSO> _cardsToShuffle)
    {         
        if (totalWeight > 162.5)
        {
            scale = totalWeight / 162.5f;
            Debug.Log("Scale: " + scale);
        }
        else if (totalWeight <= 162.5)
        {
            scale = 1;
        }
        float randomValue = Random.Range(0f, totalWeight);
        Debug.Log("Random: " + randomValue);

        foreach (TowerCardSO card in _cardsToShuffle)
        {
            float scaledWeight = card.rarityWeight * scale;           

            if (randomValue <= scaledWeight)
            {
                Debug.Log(card.towerName + " selected with a weight of:  " + scaledWeight);
                return card;
                
            }
            else if (randomValue > scaledWeight)
            {
                //Debug.Log(card.towerName + "NOT selected with a weight of: " + scaledWeight);
                randomValue -= scaledWeight;
            }
            
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
        card0SpellUsesText.gameObject.SetActive(false);
        card1SpellUsesText.gameObject.SetActive(false);
        card2SpellUsesText.gameObject.SetActive(false);
        card3SpellUsesText.gameObject.SetActive(false);
        card4SpellUsesText.gameObject.SetActive(false);
    }
   
    public void MoveToBottom()
    {
        cardsOnLeft = false;
        transform.position = bottomSpot.position;
        transform.rotation = bottomSpot.rotation;
        transform.localScale = bottomSpotScale;
        if (card0Name == "Lightning")
        {
            card0SpellUsesText.text = "IV";
            card0SpellUsesText.gameObject.SetActive(true);
        }
        else if (card0Name != "Lightning")
        {
            card0SpellUsesText.text = "IV";
            card0SpellUsesText.gameObject.SetActive(false);
        }
        if (card1Name == "Lightning")
        {
            card1SpellUsesText.text = "IV";
            card1SpellUsesText.gameObject.SetActive(true);
        }
        else if (card1Name != "Lightning")
        {
            card1SpellUsesText.text = "IV";
            card1SpellUsesText.gameObject.SetActive(false);
        }
        if (card2Name == "Lightning")
        {
            card2SpellUsesText.text = "IV";
            card2SpellUsesText.gameObject.SetActive(true);
        }
        else if (card2Name != "Lightning")
        {
            card2SpellUsesText.text = "IV";
            card2SpellUsesText.gameObject.SetActive(false);
        }
        if (card3Name == "Lightning")
        {
            card3SpellUsesText.text = "IV";
            card3SpellUsesText.gameObject.SetActive(true);
        }
        else if (card3Name != "Lightning")
        {
            card3SpellUsesText.text = "IV";
            card3SpellUsesText.gameObject.SetActive(false);
        }
        if (card4Name == "Lightning")
        {
            card4SpellUsesText.text = "IV";
            card4SpellUsesText.gameObject.SetActive(true);
        }
        else if (card4Name != "Lightning")
        {
            card4SpellUsesText.text = "IV";
            card4SpellUsesText.gameObject.SetActive(false);
        }
    }    
   
   
}
