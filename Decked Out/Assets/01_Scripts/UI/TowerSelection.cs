using UnityEngine;

public class TowerSelection : MonoBehaviour
{

    public bool isSelectingTower = false;
    public bool supportTower;
    public string towers;
    public string spells;


    [Header("Towers")]
    public GameObject ArcherTower;
    public GameObject CannonTower;
    public GameObject FlameTower; 
    public GameObject FrostTower;
    public GameObject BuffTower;
    public GameObject ElectricTower;
    public GameObject EathQuack;
    public GameObject attraction_Tower;
    public GameObject Wave_Tower;
    public GameObject Force_Field_Tower;

    [Header("Spells")]
    public GameObject lightning;
    public GameObject fireball;
    public bool isSelectingSpell = false;


    public void SelectTower()
    {
        isSelectingTower = true;
    }
    public void SelectSpells()
    {
        isSelectingSpell = true;
    }

    public bool IsSelectingTower()
    {
        return isSelectingTower;
    }
    public bool IsSelectingSpell()
    {
        return isSelectingSpell;
    }
    public void SetSelectingTower(bool value)
    {
        isSelectingTower = value;
    }
    public void SetSelectingSpell(bool value)
    {
        isSelectingSpell = value;
    }
}