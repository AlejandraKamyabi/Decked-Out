// =============================================================================
// 
// Any tower placing related stuff are all here :)
// 
//            
// 
// =============================================================================
using UnityEngine;

public class MouseInputHandling : MonoBehaviour
{
    [Header("Range Colours")]
    public Color rigColor;
    public Color notPlaceable;
    public Color onIsland;
    public Color supportOnIsland;
    private TowerSelection towerSelection;
    private GameObject currentTowerInstance;
    public GameObject castleGameObject;
    public CardRandoEngine cardRandoEngine;
    
    private WaveManager Wave;
    Vector3 mousePosition;
  
    private float unitSquareSize = 10.0f;
    public bool collisionOccurred = false;
    private bool _initialized = false;
    private bool islandTowerSelection = false;
    public GameObject towerRig;
    private SpriteRenderer towerRigSprite;
    public SpriteRenderer rangeIndicator;
    
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

        if (towerSelection.IsSelectingTower())
        {
            
            HandleTowerPlacement();
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;           
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
            towerRigSprite.color = notPlaceable;
            rangeIndicator.color = notPlaceable;
            return;
        }

        // --------------------------- TowerRig ---------------------------
        if (!towerSelection.IsSelectingSpell())
        {
            switch (towerSelection.towers)
            {
                case "Arrow Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.ArcherTower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.ArcherTower.GetComponent<SpriteRenderer>().sprite;
                    float towerRange = towerSelection.ArcherTower.GetComponent<ArcherTower>().attackRange;
                    Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;
                case "Cannon Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.CannonTower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.CannonTower.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.CannonTower.GetComponent<CannonTower>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;


                case "Flamethrower Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.FlameTower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.FlameTower.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.FlameTower.GetComponent<FlamethrowerTower>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;

                case "Frost Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.FrostTower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.FrostTower.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.FrostTower.GetComponent<FrostTower>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;
                case "Wave Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.Wave_Tower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.Wave_Tower.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.Wave_Tower.GetComponent<Wave_Tower>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;

                case "Buff Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.BuffTower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.BuffTower.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.BuffTower.GetComponent<BuffTower>().buffRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;

                case "Electric Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.ElectricTower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.ElectricTower.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.ElectricTower.GetComponent<ElectricTower>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;

                case "Earthquake Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.EathQuack.transform.localScale;
                    towerRigSprite.sprite = towerSelection.EathQuack.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.EathQuack.GetComponent<EarthQuack>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;


                case "Attraction Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.attraction_Tower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.attraction_Tower.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.attraction_Tower.GetComponent<AttractionTower>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;
                case "Poison Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.Poison_Tower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.Poison_Tower.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.Poison_Tower.GetComponent<Poison_tower>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;
                case "Ballista Tower":
                    towerRig.gameObject.transform.localScale = towerSelection.Ballista_Tower.transform.localScale;
                    towerRigSprite.sprite = towerSelection.Ballista_Tower.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.Ballista_Tower.GetComponent<Ballista_Tower>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;
            }
        }
        else
        {
            switch (towerSelection.spells)
            {
                case "Lightning":
                    towerRig.gameObject.transform.localScale = towerSelection.lightning.transform.localScale;
                    towerRigSprite.sprite = towerSelection.lightning.GetComponent<SpriteRenderer>().sprite;
                    float towerRange = towerSelection.lightning.GetComponent<LightningStrike>().attackRange;
                    Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;

                case "Fireball":
                    towerRig.gameObject.transform.localScale = towerSelection.fireball.transform.localScale;
                    towerRigSprite.sprite = towerSelection.fireball.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.fireball.GetComponent<Fireball>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;
                case "Nuke":
                    towerRig.gameObject.transform.localScale = towerSelection.nuke.transform.localScale;
                    towerRigSprite.sprite = towerSelection.nuke.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.nuke.GetComponent<Nuke>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;
                case "Big Bomb":
                    towerRig.gameObject.transform.localScale = towerSelection.bigBomb.transform.localScale;
                    towerRigSprite.sprite = towerSelection.bigBomb.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.bigBomb.GetComponent<BigBomb>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;
                case "Chill":
                    towerRig.gameObject.transform.localScale = towerSelection.chill.transform.localScale;
                    towerRigSprite.sprite = towerSelection.chill.GetComponent<SpriteRenderer>().sprite;
                    towerRange = towerSelection.chill.GetComponent<Chill>().attackRange;
                    towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                    rangeIndicator.transform.localScale = towerRangeScaling * 2;
                    break;
            }
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
                    if (!towerSelection.IsSelectingSpell())
                    {
                        towerCollision = true;
                        break;
                    }
                    else
                    {
                        towerCollision = false;
                    }
                    
                }
               
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                towerRig.gameObject.SetActive(false);


                // --------------------------- TowerInstantiate ---------------------------
                
                if (!towerCollision)
                {
                    if (!towerSelection.IsSelectingSpell())
                    {

                        switch (towerSelection.towers)
                        {
                            case "Arrow Tower":
                                currentTowerInstance = Instantiate(towerSelection.ArcherTower, mousePos, Quaternion.identity);
                                break;
                            case "Cannon Tower":
                                currentTowerInstance = Instantiate(towerSelection.CannonTower, mousePos, Quaternion.identity);
                                break;
                            case "Flamethrower Tower":
                                currentTowerInstance = Instantiate(towerSelection.FlameTower, mousePos, Quaternion.identity);
                                break;
                            case "Frost Tower":
                                currentTowerInstance = Instantiate(towerSelection.FrostTower, mousePos, Quaternion.identity);
                                break;
                            case "Buff Tower":
                                currentTowerInstance = Instantiate(towerSelection.BuffTower, mousePos, Quaternion.identity);
                                break;
                            case "Electric Tower":
                                currentTowerInstance = Instantiate(towerSelection.ElectricTower, mousePos, Quaternion.identity);
                                break;
                            case "Earthquake Tower":
                                currentTowerInstance = Instantiate(towerSelection.EathQuack, mousePos, Quaternion.identity);
                                break;
                            case "Attraction Tower":
                                currentTowerInstance = Instantiate(towerSelection.attraction_Tower, mousePos, Quaternion.identity);
                                break;
                            case "Wave Tower":
                                currentTowerInstance = Instantiate(towerSelection.Wave_Tower, mousePos, Quaternion.identity);
                                break;
                            case "Poison Tower":
                                currentTowerInstance = Instantiate(towerSelection.Poison_Tower, mousePos, Quaternion.identity);
                                break;
                            case "Ballista Tower":
                                currentTowerInstance = Instantiate(towerSelection.Ballista_Tower, mousePos, Quaternion.identity);
                                break;
                        }
                    }
                    else if (towerSelection.IsSelectingSpell())
                    {
                        switch (towerSelection.spells)
                        {

                            case "Lightning":
                                currentTowerInstance = Instantiate(towerSelection.lightning, mousePos, Quaternion.identity);
                                break;

                            case "Fireball":
                                currentTowerInstance = Instantiate(towerSelection.fireball, mousePos, Quaternion.identity);
                                break;

                            case "Nuke":
                                currentTowerInstance = Instantiate(towerSelection.nuke, mousePos, Quaternion.identity);
                                break;

                            case "Big Bomb":
                                currentTowerInstance = Instantiate(towerSelection.bigBomb, mousePos, Quaternion.identity);
                                break;

                            case "Chill":
                                currentTowerInstance = Instantiate(towerSelection.chill, mousePos, Quaternion.identity);
                                break;
                        }
                    }
                            SpriteRenderer towerRenderer = currentTowerInstance.GetComponent<SpriteRenderer>();
                    if (!islandTowerSelection)
                    {
                        if (!towerSelection.IsSelectingSpell())
                        {
                            currentTowerInstance.tag = "Placed";
                        }
                    }
                    if (towerRenderer != null)
                    {
                        float orderInLayer = (towerRig.transform.position.y * 100);
                        orderInLayer = -orderInLayer;

                        //if (orderInLayer <= 0)
                        {
                           // orderInLayer = 60;
                        }

                        towerRenderer.sortingOrder = ((int)orderInLayer);
                        if (hit.collider != null && hit.collider.gameObject.CompareTag("Platform") && towerSelection.towers != "Buff Tower" && towerSelection.towers != "Earthquake Tower" && towerSelection.towers != "Frost Tower" && !towerSelection.IsSelectingSpell())
                        {
                            towerRenderer.sortingOrder = 2501;
                        }
                    }
                    if (!Wave.collisionOccurred && !towerSelection.IsSelectingSpell())
                    {
                        currentTowerInstance.AddComponent<PositionUpdater>();
                    }
                    towerSelection.SetSelectingTower(false);
                    towerSelection.SetSelectingSpell(false);
                    Wave.IncrementTowersPlaced();
        
        
                    

                }


            }
            if (towerCollision)
            {                
                towerRigSprite.color = notPlaceable;
                rangeIndicator.color = notPlaceable;
            }

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Platform"))
            {
                if ((towerSelection.towers == "Frost Tower") || (towerSelection.towers == "Buff Tower") || (towerSelection.towers == "Earthquake Tower") || (towerSelection.spells== "Lightning") || (towerSelection.spells == "Fireball") || (towerSelection.spells == "Nuke") || (towerSelection.spells == "Big Bomb") || (towerSelection.spells == "Chill")) 
                {
                    towerRigSprite.color = supportOnIsland;
                    rangeIndicator.color = supportOnIsland;
                }
                else
                {
                    towerRigSprite.color = onIsland;
                    rangeIndicator.color = onIsland;
                    islandTowerSelection = true;
                }

            }

            else if (!towerCollision)
            {
                towerRigSprite.color = Color.white;
                rangeIndicator.color = rigColor;
            }
            else { islandTowerSelection = false; }
        }  
        else
        {
            towerRigSprite.color = notPlaceable;
            rangeIndicator.color = notPlaceable;
        }
    }

    public void ClearRig()
    {
        towerRig.gameObject.SetActive(false);
    }
    public float SpellSlotCheck()
    {
        if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace0)
        {
            return 0;
        }
        else if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace1)
        {
            return 1;
        }
        else if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace2)
        {
            return 2;
        }
        else if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace3)
        {
            return 3;
        }
        else if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace4)
        {
            return 4;
        }
        else
        {
            return float.NaN;
        }
    }
    public void setCollision()
    {
        collisionOccurred = true;
    }

}
