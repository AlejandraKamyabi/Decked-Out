using UnityEngine;

public class TowerSelection : MonoBehaviour
{
    public GameObject towerPrefab; 
    public bool isSelectingTower = false;

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