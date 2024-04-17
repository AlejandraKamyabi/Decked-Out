using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public enum CardCollected
    {
        a
    }
    public enum PurchasedItem
    {
        b
    }

    bool outPut;
    bool[] allOutPut;

    private void Awake()
    {
        GameObject[] allAudioManager = GameObject.FindGameObjectsWithTag("SaveSystem");

        if (allAudioManager.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetCardCollected(CardCollected card, bool haveCard)
    {
        switch (card)
        {
            case CardCollected.a:
                if (haveCard)
                {
                    PlayerPrefs.SetInt("a", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("a", 1);
                }
                break;
        }
    }
    public bool GetCardCollected(CardCollected card)
    {
        switch (card)
        {
            case CardCollected.a:
                if (PlayerPrefs.GetInt("a") == 0)
                {
                    outPut = false;
                }
                else
                {
                    outPut = true;
                }
                break;
        }
        return outPut;
    }

    public void SetPurchasedItem(PurchasedItem item, bool haveItem)
    {
        switch (item)
        {
            case PurchasedItem.b:
                if (haveItem)
                {
                    PlayerPrefs.SetInt("b", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("b", 1);
                }
                break;
        }
    }
    public bool GetPurchasedItem(PurchasedItem item)
    {
        switch (item)
        {
            case PurchasedItem.b:
                if (PlayerPrefs.GetInt("b") == 0)
                {
                    outPut = false;
                }
                else
                {
                    outPut = true;
                }
                break;
        }

        return outPut;
    }


    public void SetGemCount(int count)
    {
        PlayerPrefs.SetInt("gemCount", count);
    }
    public int GetGemCount()
    {
        return PlayerPrefs.GetInt("gemCount");
    }

    public void SetTotalKill(int count)
    {
        PlayerPrefs.SetInt("TotalKill", count);
    }
    public int GetTotalKill()
    {
        return PlayerPrefs.GetInt("TotalKill");
    }
}
