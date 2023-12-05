using UnityEngine;

public class MouseInputHandling : MonoBehaviour
{
    private TowerSelection towerSelection;
    private GameObject currentTowerInstance;
    public GameObject castleGameObject;
    public CardRandoEngine cardRandoEngine;
    
    private WaveManager Wave;
    Vector3 mousePosition;
    float attackRange;
    private float unitSquareSize = 10.0f;
    public bool collisionOccurred = false;
    private bool _initialized = false;
    private bool IslandTowerSelection = false;
    public GameObject towerRig;
    private SpriteRenderer towerRigSprite;
    public SpriteRenderer rangeIndicator;
    private Color rigColor;
    public void Initialize()
    {
        Debug.Log("<color=cyan> INITIALIZAING </color>");
        towerSelection = GetComponent < TowerSelection>();
        castleGameObject = GameObject.Find("Main Castle");
        Wave = ServiceLocator.Get<WaveManager>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        towerRig.gameObject.SetActive(false);
        rigColor = rangeIndicator.color;
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
        int enemy = Wave.GetEnemies();
        if (enemy <= 0)
        {
            Debug.Log("All Enemies Dead");
            cardRandoEngine.cardsInHand.Clear();
            cardRandoEngine.NewWave();
        }
    }


    private void HandleTowerPlacement()
    {
        
        towerRig.transform.position = mousePosition;
        towerRigSprite = towerRig.GetComponent<SpriteRenderer>();
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 towerPosition = mousePos;
        mousePos.z = 0;
        float minDistance = 1.0f;
        Vector3 castlePosition = castleGameObject.transform.position;
        float distanceToCastle = Vector3.Distance(mousePos, castlePosition);

        Vector2 mouseRay = mousePos;
        RaycastHit2D hit = Physics2D.Raycast(mouseRay, Vector2.zero);

        if (distanceToCastle < minDistance)
        {
            towerRigSprite.color = Color.red;
            rangeIndicator.color = Color.red;
            return;
        }
        if (towerSelection.tower == 1)
        {
            towerRig.gameObject.transform.localScale = towerSelection.towerPrefab.transform.localScale;
            towerRigSprite.sprite = towerSelection.towerPrefab.GetComponent<SpriteRenderer>().sprite;
            float towerRange = towerSelection.towerPrefab.GetComponent<ArcherTower>().attackRange;
            Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
            rangeIndicator.transform.localScale = towerRangeScaling * 2;

        }
        if (towerSelection.tower == 2)
        {
            towerRig.gameObject.transform.localScale = towerSelection.towerPrefab.transform.localScale;
            towerRigSprite.sprite = towerSelection.towerPrefab1.GetComponent<SpriteRenderer>().sprite;
            float towerRange = towerSelection.towerPrefab1.GetComponent<FlamethrowerTower>().attackRange;
            Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
            rangeIndicator.transform.localScale = towerRangeScaling * 2;
        }
        if (towerSelection.tower == 3)
        {
            towerRig.gameObject.transform.localScale = towerSelection.towerPrefab.transform.localScale;
            towerRigSprite.sprite = towerSelection.towerPrefab2.GetComponent<SpriteRenderer>().sprite;
            float towerRange = towerSelection.towerPrefab2.GetComponent<FrostTower>().attackRange;
            Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
            rangeIndicator.transform.localScale = towerRangeScaling * 2;
        }
        if (towerSelection.tower == 4)
        {
            towerRig.gameObject.transform.localScale = towerSelection.towerPrefab.transform.localScale;
            towerRigSprite.sprite = towerSelection.towerPrefab3.GetComponent<SpriteRenderer>().sprite;
            float towerRange = towerSelection.towerPrefab3.GetComponent<BuffTower>().buffRange;
            Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
            rangeIndicator.transform.localScale = towerRangeScaling * 2;
        }
        if (towerSelection.tower == 5)
        {
            towerRig.gameObject.transform.localScale = towerSelection.towerPrefab.transform.localScale;
            towerRigSprite.sprite = towerSelection.towerPrefab4.GetComponent<SpriteRenderer>().sprite;
            float towerRange = towerSelection.towerPrefab4.GetComponent<ElectricTower>().attackRange;
            Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
            rangeIndicator.transform.localScale = towerRangeScaling * 2;
        }

        if (towerSelection.tower == 6)
        {
            towerRig.gameObject.transform.localScale = towerSelection.towerPrefab.transform.localScale;
            towerRigSprite.sprite = towerSelection.towerPrefab5.GetComponent<SpriteRenderer>().sprite;
            float towerRange = towerSelection.towerPrefab5.GetComponent<EarthQuack>().attackRange;
            Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
            rangeIndicator.transform.localScale = towerRangeScaling * 2;
        }
        if (towerSelection.tower == 7)
        {
            towerRig.gameObject.transform.localScale = towerSelection.towerPrefab.transform.localScale;
            towerRigSprite.sprite = towerSelection.spellPrefab1.GetComponent<SpriteRenderer>().sprite;
            float towerRange = towerSelection.spellPrefab1.GetComponent<LightningStrike>().attackRange;
            Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
            rangeIndicator.transform.localScale = towerRangeScaling * 2;
        }
        towerRig.gameObject.SetActive(true);

        if (Mathf.Abs(towerPosition.x) <= unitSquareSize / 1 && Mathf.Abs(towerPosition.y) <= unitSquareSize / 2.5)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, 0.1f);
            bool towerCollision = false;

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Tower") || collider.CompareTag("Buffer") || collider.CompareTag("Placed"))
                {
                    towerCollision = true;                   
                    break;
                }
               
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                towerRig.gameObject.SetActive(false);


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
                    else if (towerSelection.tower == 5)
                    {
                        currentTowerInstance = Instantiate(towerSelection.towerPrefab4, mousePos, Quaternion.identity);
                    }
                    else if (towerSelection.tower == 6)
                    {
                        currentTowerInstance = Instantiate(towerSelection.towerPrefab5, mousePos, Quaternion.identity);
                    }
                    else if (towerSelection.tower == 7)
                    {
                        currentTowerInstance = Instantiate(towerSelection.spellPrefab1, mousePos, Quaternion.identity);
                    }

                    SpriteRenderer towerRenderer = currentTowerInstance.GetComponent<SpriteRenderer>();
                    if (!IslandTowerSelection)
                    {
                       currentTowerInstance.tag = "Placed";
                    }
                    if (towerRenderer != null)
                    {
                        int orderInLayer = Wave.towersPlaced; 

                        if (orderInLayer <= 0)
                        {
                            orderInLayer = 60;
                        }

                        towerRenderer.sortingOrder = orderInLayer--;
                    }
                    towerSelection.SetSelectingTower(false);
                    Wave.IncrementTowersPlaced();
                    if (!Wave.collisionOccurred && towerSelection.tower != 7)
                    {
                        currentTowerInstance.AddComponent<PositionUpdater>();

                    }

                }


            }
            if (towerCollision)
            {                
                towerRigSprite.color = Color.red;
                rangeIndicator.color = Color.red;
            }

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Platform"))
            {
                if ((towerSelection.tower == 3) || (towerSelection.tower == 4) || (towerSelection.tower == 6) || (towerSelection.tower == 7))
                {
                    towerRigSprite.color = Color.gray;
                    rangeIndicator.color = Color.gray;
                }
                else
                {
                    towerRigSprite.color = Color.yellow;
                    rangeIndicator.color = Color.yellow;
                    IslandTowerSelection = true;
                }

            }

            else if (!towerCollision)
            {
                towerRigSprite.color = Color.white;
                rangeIndicator.color = rigColor;
            }
            else { IslandTowerSelection = false; }
        }  
        
        else if (currentTowerInstance != null && Input.GetMouseButtonDown(1))
        {
            Destroy(currentTowerInstance);
            towerSelection.SetSelectingTower(false);
        }
        else
        {
            towerRigSprite.color = Color.red;
            rangeIndicator.color = Color.red;
        }
    }


    public void setCollision()
    {
        collisionOccurred = true;
    }

}
