using UnityEngine;

public class TowerSelection : MonoBehaviour
{

    public bool isSelectingTower = false;
    public bool supportTower;
    public int tower;


    [Header("Towers")]
    public GameObject ArcherTower;
    public GameObject FlameTower;
    public GameObject FrostTower;
    public GameObject BuffTower;
    public GameObject ElectricTower;
    public GameObject EathQuack;
    public GameObject attraction_Tower;

    [Header("Spells")]
    public GameObject lightning;



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