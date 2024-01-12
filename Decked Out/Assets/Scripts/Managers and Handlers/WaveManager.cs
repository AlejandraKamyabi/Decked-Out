using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class Wave
{
    public int numberOfEnemies = 5;
    public float timeBetweenEnemies = 2.0f;
    public float timeBetweenWaves = 10.0f;
}

public class WaveManager : MonoBehaviour
{
    //Managing Wave

    public GameObject enemyPrefab;
    public GameObject KaboomPrefab;
    public GameObject GolemPrefab;
    public float unitSquareSize = 10.0f;
    public float TowersLeft = 6;
    public bool kaboomEnemy = false;
    public Slider healthSliderPrefab;
    public List<Wave> waves = new List<Wave>();
    public TMP_Text towersLeftText;
    public bool collisionOccurred = false;
    private int enemiesSpawned = 0;
    public int spawnKaboomEnemyAfter = 4;
    public int spawnGolemEnemyAfter = 10;
    private EnemyKillTracker EnemyKillTracker;
    private Coroutine spawningCoroutine;
    public CardRandoEngine cardRandoEngine;
    private Button startButton;
    public int towersPlaced = 0;
    public int currentWave = 0;



    //Deck Building

    public Button buttonPrefab1;
    public Button buttonPrefab2;
    public Button buttonPrefab3;


    private Button instantiatedButton1;
    private Button instantiatedButton2;
    private Button instantiatedButton3;


    public Sprite greyCard;
    public Sprite greenCard;
    public Sprite blueCard;
    public Sprite purpleCard;
    public Sprite goldenCard;


    private bool buttonsInstantiated = false;

    public WaveManager Initialize()
    {
        //towersLeftText = FindObjectOfType<TMP_Text>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        
        Debug.Log("Wave Manager Initializing");
        EnemyKillTracker = FindObjectOfType<EnemyKillTracker>();       
        return this;
    }

    private void StartWaves()
    {
        ToggleStartButton(false);
        DestroyTowers();
        spawningCoroutine = StartCoroutine(StartWave());
  
    }
    private void UpdateButtonImages()
    {
        Sprite newImage = null;
        switch (currentWave) 
        {
            case 1:
                newImage = greyCard;
                break;
            case 2:
                newImage = greenCard;
                break;
            case 3:
                newImage = blueCard;
                break;
            case 4:
                newImage = purpleCard;
                break;
            case 5:
                newImage = goldenCard;
                break;
                // case 6:
                //     Additional logic for wave 6
        }

        if (newImage != null)
        {
            instantiatedButton1.GetComponent<Image>().sprite = newImage;
            instantiatedButton2.GetComponent<Image>().sprite = newImage;
            instantiatedButton3.GetComponent<Image>().sprite = newImage;
        }
        else
        {
            Debug.LogError("Sprite for current wave not set");
        }
    }
    public void SetStartButton(Button button)
    {
        startButton = button;
        startButton.onClick.AddListener(StartWaves);
        ToggleStartButton(true); 
    }
    private void InstantiateButtonPrefabs()
    {
        float buttonSpacing = 100; 
        Vector3 centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        
        Vector3 position1 = new Vector3(centerPosition.x - buttonSpacing, centerPosition.y, centerPosition.z);
        Vector3 position2 = new Vector3(centerPosition.x, centerPosition.y, centerPosition.z);
        Vector3 position3 = new Vector3(centerPosition.x + buttonSpacing, centerPosition.y, centerPosition.z);

     
        instantiatedButton1 = Instantiate(buttonPrefab1, position1, Quaternion.identity).GetComponent<Button>();
        instantiatedButton2 = Instantiate(buttonPrefab2, position2, Quaternion.identity).GetComponent<Button>();
        instantiatedButton3 = Instantiate(buttonPrefab3, position3, Quaternion.identity).GetComponent<Button>();

        buttonsInstantiated = true;

    }

    public void ShowDeckBuildingOptions()
    {
        InstantiateButtonPrefabs();
    }

    public void ButtonClickedMethod()
    {
        instantiatedButton1.gameObject.SetActive(false);
        instantiatedButton2.gameObject.SetActive(false);
        instantiatedButton3.gameObject.SetActive(false);
    }

    private IEnumerator StartWave()
    {
 
        if (currentWave < waves.Count)
        {
            int numberOfEnemies = waves[currentWave].numberOfEnemies;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                SpawnEnemy();
                enemiesSpawned++;

                if (enemiesSpawned == spawnKaboomEnemyAfter)
                {
                    SpawnKaboomEnemy(); 
                    kaboomEnemy = true;
                }
                if (enemiesSpawned == spawnKaboomEnemyAfter)
                {
                    SpawnGolemEnemy();
                    kaboomEnemy = true;
                    enemiesSpawned -=6;
                }

                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
            }
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                if (!cardRandoEngine.cardsOnLeft)
                {
                    cardRandoEngine.StartMoveToLeft();
                }
                else if (cardRandoEngine.cardsOnLeft)
                {
                    cardRandoEngine.NewWave();
                }
                yield return null;
            }            

            UpdateTowerHealth();

            ToggleStartButton(true);

            if (!buttonsInstantiated)
            {
                InstantiateButtonPrefabs();
            }
            else
            {
                UpdateTowerHealth();
            }
            cardRandoEngine.MoveToBottom();
            towersPlaced = 0;
            TowersLeft = 5;
            currentWave++;
            EnemyKillTracker.WaveUpdate();
        }
    }

    private void SpawnKaboomEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(KaboomPrefab, spawnPosition, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<KaboomEnemy>().maxHealth;
        newEnemy.GetComponent<KaboomEnemy>().SetHealthSlider(newHealthSlider);
    }
        private void SpawnGolemEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(GolemPrefab, spawnPosition, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 1700.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
        newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
    }
    public void StopWave()
    {
        towersPlaced = 0;
        TowersLeft = 5;
        currentWave = 0;
        EnemyKillTracker.resetWave();
        ToggleStartButton(true);
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
            spawningCoroutine = null;
        }


        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        GameObject[] sliders = GameObject.FindGameObjectsWithTag("Health");
        foreach (GameObject slider in sliders)
        {
            Destroy(slider);
        }

        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            ITower towerScript = tower.GetComponent<ITower>();
            Destroy(tower);

        }
        GameObject[] placedTowers = GameObject.FindGameObjectsWithTag("Placed");
        foreach (GameObject placedTower in placedTowers)
        {
            ITower towerScript = placedTower.GetComponent<ITower>();
            Destroy(placedTower);

        }
        GameObject[] Empties = GameObject.FindGameObjectsWithTag("Empty");
        foreach (GameObject empty in Empties)
        {
            ITower towerScript = empty.GetComponent<ITower>();
            Destroy(empty);
            collisionOccurred = false;

        }
        GameObject[] buffer = GameObject.FindGameObjectsWithTag("Buffer");
        foreach (GameObject buffers in buffer)
        {
            IBuffTower towerScript = buffers.GetComponent<IBuffTower>();

                Destroy(buffers);
        }

    }

    private void UpdateTowerHealth()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            ITower towerScript = tower.GetComponent<ITower>();

            if (towerScript != null)
            {
                towerScript.health--;
            }
        }
        GameObject[] PlacedTowers = GameObject.FindGameObjectsWithTag("Placed");
        foreach (GameObject PlacedTower in PlacedTowers)
        {
            ITower towerScript = PlacedTower.GetComponent<ITower>();

            if (towerScript != null)
            {
                towerScript.health--;
            }
        }
        GameObject[] Empties = GameObject.FindGameObjectsWithTag("Empty");
        foreach (GameObject empty in Empties)
        {
            ITower towerScript = empty.GetComponent<ITower>();

            if (towerScript != null)
            {
                towerScript.health--;
            }

        }
        GameObject[] buffer = GameObject.FindGameObjectsWithTag("Buffer");
        foreach (GameObject buffers in buffer)
        {
            IBuffTower towerScript = buffers.GetComponent<IBuffTower>();
            if (towerScript != null)
            {
                towerScript.health--;
            }
        }
    }

    private void ToggleStartButton(bool isEnabled)
    {
        startButton.interactable = isEnabled;
        startButton.gameObject.SetActive(isEnabled);
    }
    private void Update()
    {

        //towersLeftText.text = "Towers Left to Place: " + TowersLeft;
    }

    private void DestroyTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            ITower towerScript = tower.GetComponent<ITower>();

           
            if (towerScript != null && towerScript.health <= 0)
            {

                Destroy(tower);

            }

        }
        GameObject[] PlacedTowers = GameObject.FindGameObjectsWithTag("Placed");
        foreach (GameObject PlacedTower in PlacedTowers)
        {
            ITower towerScript = PlacedTower.GetComponent<ITower>();
           
            if (towerScript != null && towerScript.health <= 0)
            {

                Destroy(PlacedTower);

            }

        }
        GameObject[] Empties = GameObject.FindGameObjectsWithTag("Empty");
        foreach (GameObject empty in Empties)
        {
            ITower towerScript = empty.GetComponent<ITower>();

            if (towerScript != null && towerScript.health <= 0)
            {

                Destroy(empty);
                collisionOccurred = false;

            }

        }
        GameObject[] buffer = GameObject.FindGameObjectsWithTag("Buffer");
        foreach (GameObject buffers in buffer)
        {
            IBuffTower towerScript = buffers.GetComponent<IBuffTower>();

            if (towerScript != null && towerScript.health <= 0)
            {

                Destroy(buffers);

            }
        }

    }
        private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
        newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
    }


    private Vector3 GetRandomSpawnPosition()
    {
        int randomSide = Random.Range(0, 4);


        float randomX = 0;
        float randomY = 0;

        switch (randomSide)
        {
            case 0:
                randomX = Random.Range(-unitSquareSize / 2, unitSquareSize / 2);
                randomY = unitSquareSize / 2;
                break;
            case 1:
                randomX = unitSquareSize / 2;
                randomY = Random.Range(-unitSquareSize / 2, unitSquareSize / 2);
                break;
            case 2:
                randomX = Random.Range(-unitSquareSize / 2, unitSquareSize / 2);
                randomY = -unitSquareSize / 2;
                break;
            case 3:
                randomX = -unitSquareSize / 2;
                randomY = Random.Range(-unitSquareSize / 2, unitSquareSize / 2);
                break;
        }

        Vector3 spawnPosition = new Vector3(0, 0, 0) + new Vector3(randomX, randomY, 0);

        return spawnPosition;
    }
    public void IncrementTowersPlaced()
    {
        towersPlaced++;
        TowersLeft--;
    }
    public void setCollision()
    {
        collisionOccurred = true;
    }
    public int GetEnemies()
    {
        return waves[currentWave].numberOfEnemies;
    }
    public int GetWave()
    {
        return currentWave;
    }
}