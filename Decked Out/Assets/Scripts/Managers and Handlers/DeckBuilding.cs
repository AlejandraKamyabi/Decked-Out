using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DeckBuilding : MonoBehaviour
{

    public bool isDeckActive;

    //Sprites

    public Sprite greyCard;
    public Sprite greenCard;
    public Sprite blueCard;
    public Sprite purpleCard;
    public Sprite goldenCard;
    public Sprite cardImage;
    private WaveManager Wave;
    // Cards (that are buttons)

    public Button button1;
    public Button button2;
    public Button button3;

    // Blocking UI

    public Button blockUI;
    public void Initialize()
    {
      
        Wave = ServiceLocator.Get<WaveManager>();
        SetupEventTrigger(button1);
        SetupEventTrigger(button2);
        SetupEventTrigger(button3);
        isDeckActive = Wave.getDeckBuilding();

    }

    void SetupEventTrigger(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((data) => { ButtonClicked(); });
        trigger.triggers.Add(clickEntry);
    }
    public int GetWave()
    {
        if (Wave != null)
        {
            return Wave.currentWave;
          
        }
        else
        {
            Debug.LogError("WaveManager is not initialized in DeckBuilding");
            return -1;
        }
    }

    public void ButtonClicked()
    {
        Wave.OnCardSelected();
    }
    public void EnableButtons()
    {
        button1.gameObject.SetActive(isDeckActive);
        button2.gameObject.SetActive(isDeckActive);
        button3.gameObject.SetActive(isDeckActive);


        blockUI.gameObject.SetActive(true);
    }
    public void UpdateButtonImages()
    {
        if (Wave != null)
        {
            Sprite newImage = null;
            switch (Wave.currentWave)
            {
                case 1:
                    newImage = greyCard;
                    break;
                case 2:
                    newImage = greenCard;
                    break;          
                case 3:
                    newImage = blueCard;
                    break;
                case 4:
                    newImage = purpleCard;
                    break;
                case 5:
                    newImage = goldenCard;
                    break;
               // case 6:
               //     To be done
               //   
            }
            if (newImage != null)
            {
                button1.GetComponent<Image>().sprite = newImage;
                button2.GetComponent<Image>().sprite = newImage;
                button3.GetComponent<Image>().sprite = newImage;
            }
        }
        else
        {
            Debug.LogError("WaveManager is not initialized in DeckBuilding");
        }
    }

}