using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerCard", menuName = "DOGU/Card")]
public class TowerCardSO : ScriptableObject
{
    public string towerName;
    public Sprite background;
    public Sprite image;
    public int towerID;
    public float rarityWeight;      
}
