using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerCard", menuName = "DOGU/Card")]
public class TowerCardSO : ScriptableObject
{
    [Header("Displayed Data")]
    public string towerName;
    public Sprite background;
    public Sprite image;
    public Sprite icon;
    public string towerInfo;
    public int uses;

    [Header("Internal Data")]
    public int towerID;
    public float rarityWeight;
    public string rarity;
    public float damage;
    public float range;
    public float rateOfFire;
    public float duration;
}
