using UnityEngine;

public class MouseInputHandling : MonoBehaviour
{
    private TowerSelection towerSelection;
    private GameObject currentTowerInstance;

    private void Start()
    {
        towerSelection = GetComponent<TowerSelection>();
    }

    private void Update()
    {
        if (towerSelection.IsSelectingTower())
        {
            HandleTowerPlacement();
        }
    }

    private void HandleTowerPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; 
            currentTowerInstance = Instantiate(towerSelection.towerPrefab, mousePos, Quaternion.identity);


            towerSelection.SetSelectingTower(false);
        }
        else if (currentTowerInstance != null && Input.GetMouseButtonDown(1))
        {

            Destroy(currentTowerInstance);


            towerSelection.SetSelectingTower(false);
        }
    }
}