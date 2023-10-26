using UnityEngine;

public class MouseInputHandling : MonoBehaviour
{
    private TowerSelection towerSelection;
    private GameObject currentTowerInstance;
    public GameObject rangeIndicatorPrefab;
    public GameObject castleGameObject;
    private PositionUpdater updater;
    public GameObject Tower;
    Vector3 mousePosition;
    float attackRange;
    private ArcherTower archerTower;
    private float unitSquareSize = 10.0f;
    public bool collisionOccurred = false;
    private bool _initialized = false;

    public void Initialize()
    {
        Debug.Log("<color=cyan> INITIALIZAING </color>");
        towerSelection = GetComponent < TowerSelection>();
        updater = GetComponent<PositionUpdater>();
        castleGameObject = GameObject.Find("Main Castle");
        archerTower = Tower.GetComponent<ArcherTower>();
        attackRange = archerTower.GetAttackRange();
        rangeIndicatorPrefab.transform.localScale = new Vector3(attackRange / 0.40105374f, attackRange / 0.40105374f);

        if (castleGameObject == null)
        {
            castleGameObject = GameObject.FindWithTag("Player");
        }

        _initialized = true;
    }

    private void Update()
    {
        if (_initialized == false) { return; }

        if (towerSelection.IsSelectingTower())
        {
            HandleTowerPlacement();
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            rangeIndicatorPrefab.transform.position = mousePosition;
        }
    }

    private void HandleTowerPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 towerPosition = mousePos;
            mousePos.z = 0;
            float minDistance = 2.0f;
            Vector3 castlePosition = castleGameObject.transform.position;
            float distanceToCastle = Vector3.Distance(mousePos, castlePosition);

            if (distanceToCastle < minDistance)
            {
                return;
            }


            if (Mathf.Abs(towerPosition.x) <= unitSquareSize / 1 && Mathf.Abs(towerPosition.y) <= unitSquareSize / 3)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, 0.1f);
                bool towerCollision = false;

                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Tower"))
                    {
                        towerCollision = true;
                        break;
                    }
                }

                if (!towerCollision)
                {
                    currentTowerInstance = Instantiate(towerSelection.towerPrefab, mousePos, Quaternion.identity);

                    towerSelection.SetSelectingTower(false);

                    if (!collisionOccurred)
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
