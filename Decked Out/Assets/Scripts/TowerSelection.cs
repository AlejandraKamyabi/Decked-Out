using UnityEngine;

public class TowerSelection : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject towerPrefab1;
    public GameObject towerPrefab2;
    public GameObject towerPrefab3;
    public GameObject towerPrefab4;
    public GameObject towerPrefab5;
    public GameObject spellPrefab1;
    public bool isSelectingTower = false;
    public bool supportTower;
    public int tower;

    public void SelectTower()
    {
        isSelectingTower = true;
    }

    public bool IsSelectingTower()
    {
        return isSelectingTower;
    }
    public void SetSelectingTower(bool value)
    {
        isSelectingTower = value;
    }
}