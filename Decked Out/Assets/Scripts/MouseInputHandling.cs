using UnityEngine;

public class MouseInputHandling : MonoBehaviour
{
    private TowerSelection towerSelection;
    private GameObject currentTowerInstance;
    public GameObject castleGameObject;
    private WaveManager Wave;
    Vector3 mousePosition;
    float attackRange;
    private float unitSquareSize = 10.0f;
    public bool collisionOccurred = false;
    private bool _initialized = false;

    public void Initialize()
    {
        Debug.Log("<color=cyan> INITIALIZAING </color>");
        towerSelection = GetComponent < TowerSelection>();
        castleGameObject = GameObject.Find("Main Castle");
        Wave = ServiceLocator.Get<WaveManager>();

        if (castleGameObject == null)
        {
            castleGameObject = GameObject.FindWithTag("Player");
        }

        _initialized = true;
    }

    private void Update()
    {
        if (_initialized == false) { return; }

        if (towerSelection.IsSelectingTower() && Wave.towersPlaced < 5)
        {
            HandleTowerPlacement();
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
        }
    }


    private void HandleTowerPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 towerPosition = mousePos;
            mousePos.z = 0;
            float minDistance = 1.0f;
            Vector3 castlePosition = castleGameObject.transform.position;
            float distanceToCastle = Vector3.Distance(mousePos, castlePosition);

            if (distanceToCastle < minDistance)
            {
                return;
            }

            if (Mathf.Abs(towerPosition.x) <= unitSquareSize / 1 && Mathf.Abs(towerPosition.y) <= unitSquareSize / 2.5)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, 0.1f);
                bool towerCollision = false;

                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Tower") || collider.CompareTag("Buffer"))
                    {
                        towerCollision = true;
                        break;
                    }
                }

                if (!towerCollision)
                {
                    if (towerSelection.tower == 1)
                    {
                        currentTowerInstance = Instantiate(towerSelection.towerPrefab, mousePos, Quaternion.identity);
                    }
                    else if (towerSelection.tower == 2)
                    {
                        currentTowerInstance = Instantiate(towerSelection.towerPrefab1, mousePos, Quaternion.identity);
                    }
                    else if (towerSelection.tower == 3)
                    {
                        currentTowerInstance = Instantiate(towerSelection.towerPrefab2, mousePos, Quaternion.identity);
                    }
                    else if (towerSelection.tower == 4)
                    {
                        currentTowerInstance = Instantiate(towerSelection.towerPrefab3, mousePos, Quaternion.identity);
                    }
                    towerSelection.SetSelectingTower(false);
                    Wave.IncrementTowersPlaced();
                    if (!Wave.collisionOccurred)
                    {
                        currentTowerInstance.AddComponent<PositionUpdater>();
               
                    }
                }
            }
        }
        else if (currentTowerInstance != null && Input.GetMouseButtonDown(1))
        {
            Destroy(currentTowerInstance);
            towerSelection.SetSelectingTower(false);
        }
    }


    public void setCollision()
    {
        collisionOccurred = true;
    }

}
