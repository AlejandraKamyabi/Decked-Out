                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                // =============================================================================
// 
// Everything related to spawning enemies, waves are all here.
// 
//            
// 
// =============================================================================

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
    public GameObject Apostate_Prefab;
    public GameObject necromancer;
    public GameObject aegis;
    public GameObject cleric;


    public float unitSquareSize = 10.0f;
    public float TowersLeft = 6;
    public bool kaboomEnemy = false;
    public Slider healthSliderPrefab;

    public List<Wave> waves = new List<Wave>();
    public TMP_Text towersLeftText;
    public bool collisionOccurred = false;

    private int enemiesSpawned = 0;
    [Range(1, 25)]
    public int enemiesBetweenKaboomSpawns = 4;
    [Range(1, 25)]
    public int enemiesBetweenGolemSpawns = 6;
    [Range(1, 25)]
    public int enemiesBetweenApostateSpawns = 8;
    [Range(1, 25)]
    public int enemiesBetweenNecromancerSpawns = 10;
    [Range(1, 25)]
    public int enemiesBetweenAegisSpawns = 7;
    [Range(1, 25)]
    public int enemiesBetweenClericSpawns = 9;

    private EnemyKillTracker _killTracker;
    private Coroutine spawningCoroutine;
    public CardRandoEngine cardRandoEngine;
    private Button startButton;


    public int towersPlaced = 0;
    public int currentWave = 0;


    //Deck Building

    public CardHandling deck_Building;

    public WaveManager Initialize()
    {
        //towersLeftText = FindObjectOfType<TMP_Text>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();

        deck_Building = FindObjectOfType<CardHandling>();

        Debug.Log("Wave Manager Initializing");
        _killTracker = FindObjectOfType<EnemyKillTracker>();

        //Sets number to human readable
        enemiesBetweenApostateSpawns--;
        enemiesBetweenGolemSpawns--;
        enemiesBetweenKaboomSpawns--;
        enemiesBetweenNecromancerSpawns--;
        enemiesBetweenAegisSpawns--;
        enemiesBetweenClericSpawns--;
        return this;
    }

    private void StartWaves()
    {
        ToggleStartButton(false);        
        spawningCoroutine = StartCoroutine(StartWave());
  
    }

    public void SetStartButton(Button button)
    {
        startButton = button;
        startButton.onClick.AddListener(StartWaves);
        ToggleStartButton(true); 
    }


    private IEnumerator StartWave()
    {

        int numberOfEnemies = waves[currentWave].numberOfEnemies;
        _killTracker.NumbersOfEnemiesInWave(GetEnemies());
        enemiesSpawned = 0;
        for (int i = 0; i < numberOfEnemies; i++)
            {
            if (enemiesSpawned >= numberOfEnemies)
            {
                Debug.LogError("Trying to spawn more enemies than allowed");
                break;
            }
            else if (enemiesSpawned % enemiesBetweenNecromancerSpawns == 0 && enemiesSpawned != 0)
            {
                Spawn_Necromancer();
                enemiesSpawned++;
                //kaboomEnemy = true;
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
                continue;
            }
            else if (enemiesSpawned % enemiesBetweenAegisSpawns == 0 && enemiesSpawned != 0)
            {
                Spawn_Aegis();
                enemiesSpawned++;
                //kaboomEnemy = true;
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
                continue;
            }
            else if (enemiesSpawned % enemiesBetweenClericSpawns == 0 && enemiesSpawned != 0)
            {
                Spawn_Cleric();
                enemiesSpawned++;
                //kaboomEnemy = true;
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
                continue;
            }
            else if (enemiesSpawned % enemiesBetweenApostateSpawns == 0 && enemiesSpawned != 0)
            {
                SpawnApostateEnemy();
                enemiesSpawned++;
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
                continue;
            }
            else if (enemiesSpawned % enemiesBetweenGolemSpawns == 0 && enemiesSpawned != 0)
            {
                SpawnGolemEnemy();
                enemiesSpawned++;
                //kaboomEnemy = true;
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
                continue;
            }
            else if (enemiesSpawned % enemiesBetweenKaboomSpawns == 0 && enemiesSpawned != 0)
            {
                SpawnKaboomEnemy();
                enemiesSpawned++;
                //kaboomEnemy = true;
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
                continue;
            }
            else
            {
                SpawnEnemy();
                enemiesSpawned++;
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
                continue;
            }
            
        }
    }
    public void AllEnemiesInWaveDestroyed()
    {
        UpdateTowerHealth();
        DestroyTowers();
        ToggleStartButton(true);

        towersPlaced = 0;
        TowersLeft = 5;
        currentWave++;
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
    private void Spawn_Necromancer()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(necromancer, spawnPosition, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<Necromancer>().maxHealth;
        newEnemy.GetComponent<Necromancer>().SetHealthSlider(newHealthSlider);
    }
    private void Spawn_Aegis()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(aegis, spawnPosition, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<Aegis>().maxHealth;
        newEnemy.GetComponent<Aegis>().SetHealthSlider(newHealthSlider);
    }
    private void Spawn_Cleric()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(cleric, spawnPosition, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<Cleric>().maxHealth;
        newEnemy.GetComponent<Cleric>().SetHealthSlider(newHealthSlider);
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
    private void SpawnApostateEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(Apostate_Prefab, spawnPosition, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 1700.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<Apostate>().maxHealth;
        newEnemy.GetComponent<Apostate>().SetHealthSlider(newHealthSlider);
    }
    public void StopWave()
    {
        towersPlaced = 0;
        TowersLeft = 5;
        currentWave = 0;
        _killTracker.resetWave();
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
        GameObject[] Placed_buffer = GameObject.FindGameObjectsWithTag("Placed");
        foreach (GameObject Placed_buffers in Placed_buffer)
        {
            IBuffTower towerScript = Placed_buffers.GetComponent<IBuffTower>();
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
    public void AddEnemyToCurrentWave(string enemyType, Vector3 spawnPosition)
    {
      

        switch (enemyType)
        {
            case "Acolyte":
               
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                Slider newHealthSlider = Instantiate(healthSliderPrefab);

                Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
                newHealthSlider.transform.position = sliderPosition;
                _killTracker.NumbersOfEnemiesInWave(GetEnemies());
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
                newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
                break;
            case "Kaboom":
               
                newEnemy = Instantiate(KaboomPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider = Instantiate(healthSliderPrefab);

                sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
                newHealthSlider.transform.position = sliderPosition;
                _killTracker.NumbersOfEnemiesInWave(GetEnemies());
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<KaboomEnemy>().maxHealth;
                newEnemy.GetComponent<KaboomEnemy>().SetHealthSlider(newHealthSlider);

                break;
            case "Apostate":
           
                newEnemy = Instantiate(Apostate_Prefab, spawnPosition, Quaternion.identity);
                newHealthSlider = Instantiate(healthSliderPrefab);

                 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
                newHealthSlider.transform.position = sliderPosition;
                _killTracker.NumbersOfEnemiesInWave(GetEnemies());
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<Apostate>().maxHealth;
                newEnemy.GetComponent<Apostate>().SetHealthSlider(newHealthSlider);
                break;
            case "Golem":
               
                newEnemy = Instantiate(GolemPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider = Instantiate(healthSliderPrefab);

                sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 1700.0f, 0));
                newHealthSlider.transform.position = sliderPosition;
                _killTracker.NumbersOfEnemiesInWave(GetEnemies());
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
                newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
                break;
        }

        if (enemyPrefab != null)
        {
            waves[currentWave].numberOfEnemies++;
            
        }

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
        GameObject[] Placed_Buffer = GameObject.FindGameObjectsWithTag("Placed");
        foreach (GameObject Placed_Buffers in Placed_Buffer)
        {
            IBuffTower towerScript = Placed_Buffers.GetComponent<IBuffTower>();

            if (towerScript != null && towerScript.health <= 0)
            {

                Destroy(Placed_Buffers);

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
        bool spawnInside = Random.value < 0.3;  // 30% chance to spawn slightly inside

        // Reduced inset to a very minimal value to prevent mid-screen spawning
        float insetFactor = spawnInside ? 0.2f : 0.0f;  

        float maxInset = unitSquareSize * insetFactor;
        float minEdgeOffset = unitSquareSize / 2 - maxInset;  // Ensures always close to edge

        float randomX = 0;
        float randomY = 0;

        switch (randomSide)
        {
            case 0:  // Top edge
                randomX = Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                randomY = spawnInside ? Random.Range(minEdgeOffset, unitSquareSize / 2) : unitSquareSize / 2;
                break;
            case 1:  // Right edge
                randomX = spawnInside ? Random.Range(unitSquareSize / 2 - maxInset, unitSquareSize / 2) : unitSquareSize / 2;
                randomY = Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                break;
            case 2:  // Bottom edge
                randomX = Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                randomY = spawnInside ? Random.Range(-unitSquareSize / 2, -minEdgeOffset) : -unitSquareSize / 2;
                break;
            case 3:  // Left edge
                randomX = spawnInside ? Random.Range(-unitSquareSize / 2, -unitSquareSize / 2 + maxInset) : -unitSquareSize / 2;
                randomY = Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                break;
        }

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
        return spawnPosition;
    }
    public void IncrementTowersPlaced()
    {
        towersPlaced++;
        TowersLeft--;
    }
    public void setCollision(bool tf)
    {
        collisionOccurred = tf;
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