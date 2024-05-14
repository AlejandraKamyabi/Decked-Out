using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public enum CardCollected
    {
        //Towers
        Arrow,
        Frost_Tower,
        Buff_Tower,
        Flamethrower,
        Electric_Tower,
        Earthquake_Tower,
        Attraction_Tower,
        Cannon,
        Wave_Tower,
        Balista_Tower,
        Poison_Tower,
        Mystery_Tower,
        Mortar_Tower,
        Sniper_Tower,

        //SPELLS
        Lighting_Bolt,
        Big_Bomb,
        Fireball,
        Nuke,
        Frost,
        Freeze_Time
    }
    public enum PurchasedItem
    {
        b
    }

    private bool outPut = false;
    private bool[] allOutPut;
    private int itemCount = 0;
    private int cardCount = 0;

    private Dictionary<CardCollected, string> cardCollectedString;
    private Dictionary<PurchasedItem, string> purchasedItemString;

    private void Awake()
    {
        GameObject[] saveSystem = GameObject.FindGameObjectsWithTag("SaveSystem");

        if (saveSystem.Length > 1)
        {
            Destroy(gameObject);
        }

        foreach (CardCollected card in System.Enum.GetValues(typeof(CardCollected)))
        {
            cardCollectedString[card] = card.ToString();
            cardCount++;
        }
        foreach (PurchasedItem item in System.Enum.GetValues(typeof(PurchasedItem)))
        {
            purchasedItemString[item] = item.ToString();
            itemCount++;
        }

        DontDestroyOnLoad(gameObject);
    }

    //Set Card Collected
    public void SetCardCollected(CardCollected card, bool haveCard)
    {
        if (haveCard)
        {
            PlayerPrefs.SetInt(cardCollectedString[card], 1);
        }
        else
        {
            PlayerPrefs.SetInt(cardCollectedString[card], 0);
        }
    }
    //Get One Card Collected
    public bool GetCardCollected(CardCollected card)
    {
        outPut = false;
        if (PlayerPrefs.HasKey(cardCollectedString[card]))
        {
            if (PlayerPrefs.GetInt(cardCollectedString[card]) == 1)
            {
                outPut = true;
            }
            else
            {
                outPut = false;
            }
        }
        return outPut;
    }
    //Get All Card Collected (in the order how the enum are)
    public bool[] GetAllCardCollected()
    {
        allOutPut = new bool[cardCount];
        int count = 0;

        foreach (CardCollected card in System.Enum.GetValues(typeof(CardCollected)))
        {
            allOutPut[count] = false;
            count++;
        }
        count = 0;

        foreach (CardCollected card in System.Enum.GetValues(typeof(CardCollected)))
        {
            if (PlayerPrefs.HasKey(cardCollectedString[card]))
            {
                if (PlayerPrefs.GetInt(cardCollectedString[card]) == 1)
                {
                    allOutPut[count] = true;
                }
                else
                {
                    allOutPut[count] = false;
                }
            }
            count++;
        }

        return allOutPut;
    }
    //Reset Card Collected
    public void ResetCardCollected()
    {
        foreach (CardCollected card in System.Enum.GetValues(typeof(CardCollected)))
        {
            PlayerPrefs.SetInt(cardCollectedString[card], 0);
        }
    }

    //Set Purchased Item
    public void SetPurchasedItem(PurchasedItem item, bool haveItem)
    {
        if (haveItem)
        {
            PlayerPrefs.SetInt(purchasedItemString[item], 1);
        }
        else
        {
            PlayerPrefs.SetInt(purchasedItemString[item], 0);
        }
    }
    //Get one Purchased Item
    public bool GetPurchasedItem(PurchasedItem item)
    {
        outPut = false;
        if (PlayerPrefs.HasKey(purchasedItemString[item]))
        {
            if (PlayerPrefs.GetInt(purchasedItemString[item]) == 1)
            {
                outPut = true;
            }
            else
            {
                outPut = false;
            }
        }
        return outPut;
    }
    //Get all Purchased Item (in the order how the enum are)
    public bool[] GetAllPurchasedItem()
    {
        allOutPut = new bool[itemCount];
        int count = 0;

        foreach (PurchasedItem item in System.Enum.GetValues(typeof(PurchasedItem)))
        {
            allOutPut[count] = false;
            count++;
        }
        count = 0;

        foreach (PurchasedItem item in System.Enum.GetValues(typeof(PurchasedItem)))
        {
            if (PlayerPrefs.HasKey(purchasedItemString[item]))
            {
                if (PlayerPrefs.GetInt(purchasedItemString[item]) == 1)
                {
                    allOutPut[count] = true;
                }
                else
                {
                    allOutPut[count] = false;
                }
            }
            count++;
        }

        return allOutPut;
    }
    //Reset Purchased Item
    public void ResetPurchasedItem()
    {
        foreach (PurchasedItem item in System.Enum.GetValues(typeof(PurchasedItem)))
        {
            PlayerPrefs.SetInt(purchasedItemString[item], 0);
        }
    }

    //Add Gem
    public void AddGem(int count)
    {
        int i = count;
        if (PlayerPrefs.HasKey("gemCount"))
        {
            i += PlayerPrefs.GetInt("gemCount");
        }
        PlayerPrefs.SetInt("gemCount", i);
    }
    //Save Gem Count
    public void SetGemCount(int count)
    {
        PlayerPrefs.SetInt("gemCount", count);
    }
    //Get Gem Count
    public int GetGemCount()
    {
        if (PlayerPrefs.HasKey("gemCount"))
        {
            return PlayerPrefs.GetInt("gemCount");
        }
        else
        {
            return 0;
        }
    }
    //Reset Gem Count
    public void ResetGemCount()
    {
        PlayerPrefs.SetInt("gemCount", 0);
    }

    //Add total kill
    public void AddTotalKill(int count)
    {
        int i = count;
        if (PlayerPrefs.HasKey("TotalKill"))
        {
            i += PlayerPrefs.GetInt("TotalKill");
        }
        PlayerPrefs.SetInt("TotalKill", i);
    }
    //Save Total Kill
    public void SetTotalKill(int count)
    {
        PlayerPrefs.SetInt("TotalKill", count);
    }
    //Get Total Kill
    public int GetTotalKill()
    {
        if (PlayerPrefs.HasKey("TotalKill"))
        {
            return PlayerPrefs.GetInt("TotalKill");
        }
        else
        {
            return 0;
        }
    }
    //Reset Total Kill
    private void ResetTotalKill()
    {
        PlayerPrefs.SetInt("TotalKill", 0);
    }
}
