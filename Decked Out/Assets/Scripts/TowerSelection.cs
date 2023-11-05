using UnityEngine;

public class TowerSelection : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject towerPrefab1;
    public GameObject towerPrefab2;
    public GameObject towerPrefab3;
    public bool isSelectingTower = false;
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
    public void setArcherTower()
    {
        tower = 1;
    }
    public void setFlameTower()
    {
        tower = 2;
    }
    public void setFrostTower()
    {
        tower = 3;
    }
    public void setBufferTower()
    {
        tower = 4;
    }
}