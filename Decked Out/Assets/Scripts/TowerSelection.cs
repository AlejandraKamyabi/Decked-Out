using UnityEngine;

public class TowerSelection : MonoBehaviour
{
    public GameObject towerPrefab;
    public GameObject towerPrefab1;
    public GameObject towerPrefab2;
    public GameObject towerPrefab3;
    public GameObject towerPrefab4;
    public GameObject towerPrefab5;
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
    public void setArcherTower()
    {
        tower = 1;
        supportTower = false;
    }
    public void setFlameTower()
    {
        tower = 2;
        supportTower = false;
    }
    public void setFrostTower()
    {
        tower = 3;
        supportTower = true;
    }
    public void setBufferTower()
    {
        tower = 4;
        supportTower = true;
    }
    public void setElecetricTower()
    {
        tower = 5;
        supportTower = false;
    }
    public void setEarthQuackTower()
    {
        tower = 6;
        supportTower = true;
    }
}