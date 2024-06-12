using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();

    public GameObject enemyPrefab;
    public GameObject KaboomPrefab;
    public GameObject GolemPrefab;
    public GameObject Apostate_Prefab;
    public GameObject necromancer;
    public GameObject aegis;
    public GameObject cleric;
    public GameObject Mopey_prefab;
    public GameObject Mistake_Prefab;
    public float unitSquareSize = 10.0f;
    public float TowersLeft = 6;
    public bool kaboomEnemy = false;
    public Slider healthSliderPrefab;
    public TMP_Text towersLeftText;
    public bool collisionOccurred = false;

    private int enemiesSpawned = 0;
    private EnemyKillTracker _killTracker;
    private Coroutine spawningCoroutine;
    public CardRandoEngine cardRandoEngine;
    private Button startButton;
    public int towersPlaced = 0;
    public int currentWave = 0;
    public CardHandling deck_Building;
    private GameSpeedManager _gameSpeedManager;

    private void Start()
    {
        Initialize();
    }

    public WaveManager Initialize()
    {
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        deck_Building = FindObjectOfType<CardHandling>();
        _killTracker = FindObjectOfType<EnemyKillTracker>();
        _gameSpeedManager = FindObjectOfType<GameSpeedManager>();
        return this;
    }

    private void StartWaves()
    {
        ToggleStartButton(false);
        _gameSpeedManager.ActivateControlPanel();
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
        _killTracker.NumbersOfEnemiesInWave(numberOfEnemies);
        enemiesSpawned = 0;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (enemiesSpawned >= numberOfEnemies)
            {
                Debug.LogError("Trying to spawn more enemies than allowed");
                break;
            }
            else
            {
                SpawnEnemyBasedOnPercentage(waves[currentWave].enemySpawnPercentages);
                enemiesSpawned++;
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
            }
        }
    }

    private void SpawnEnemyBasedOnPercentage(SerializableDictionary<string, float> enemySpawnPercentages)
    {
        float rand = Random.value;
        float cumulative = 0f;

        foreach (var enemyType in enemySpawnPercentages)
        {
            cumulative += enemyType.Value;
            if (rand < cumulative)
            {
                switch (enemyType.Key)
                {
                    case "Acolyte":
                        SpawnEnemy();
                        break;
                    case "Kaboom":
                        SpawnKaboomEnemy();
                        break;
                    case "Golem":
                        SpawnGolemEnemy();
                        break;
                    case "Apostate":
                        SpawnApostateEnemy();
                        break;
                    case "Necromancer":
                        Spawn_Necromancer();
                        break;
                    case "Aegis":
                        Spawn_Aegis();
                        break;
                    case "Cleric":
                        Spawn_Cleric();
                        break;
                }
                break;
            }
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        SetupHealthSlider(newEnemy, newEnemy.GetComponent<Enemy>().maxHealth);
    }

    private void SpawnKaboomEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(KaboomPrefab, spawnPosition, Quaternion.identity);
        SetupHealthSlider(newEnemy, newEnemy.GetComponent<KaboomEnemy>().maxHealth);
    }

    private void SpawnGolemEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(GolemPrefab, spawnPosition, Quaternion.identity);
        SetupHealthSlider(newEnemy, newEnemy.GetComponent<Enemy>().maxHealth);
    }

    private void SpawnApostateEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(Apostate_Prefab, spawnPosition, Quaternion.identity);
        SetupHealthSlider(newEnemy, newEnemy.GetComponent<Apostate>().maxHealth);
    }

    private void Spawn_Necromancer()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(necromancer, spawnPosition, Quaternion.identity);
        SetupHealthSlider(newEnemy, newEnemy.GetComponent<Necromancer>().maxHealth);
    }
    public void IncrementTowersPlaced()
    {
        towersPlaced++;
        TowersLeft--;
    }
    private void Spawn_Aegis()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(aegis, spawnPosition, Quaternion.identity);
        SetupHealthSlider(newEnemy, newEnemy.GetComponent<Aegis>().maxHealth);
    }

    private void Spawn_Cleric()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(cleric, spawnPosition, Quaternion.identity);
        SetupHealthSlider(newEnemy, newEnemy.GetComponent<Cleric>().maxHealth);
    }

    private void SetupHealthSlider(GameObject enemy, float maxHealth)
    {
        Slider newHealthSlider = Instantiate(healthSliderPrefab);
        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;
        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = maxHealth;
        //enemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int randomSide = UnityEngine.Random.Range(0, 4);
        bool spawnInside = UnityEngine.Random.value < 0.3f;

        float insetFactor = spawnInside ? 0.1f : 0.0f;

        float maxInset = unitSquareSize * insetFactor;
        float minEdgeOffset = unitSquareSize / 1 - maxInset;

        float randomX = 0;
        float randomY = 0;

        switch (randomSide)
        {
            case 0:
                randomX = UnityEngine.Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                randomY = spawnInside ? UnityEngine.Random.Range(minEdgeOffset, unitSquareSize / 2) : unitSquareSize / 2;
                break;
            case 1:
                randomX = spawnInside ? UnityEngine.Random.Range(unitSquareSize / 2 - maxInset, unitSquareSize / 2) : unitSquareSize / 2;
                randomY = UnityEngine.Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                break;
            case 2:
                randomX = UnityEngine.Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                randomY = spawnInside ? UnityEngine.Random.Range(-unitSquareSize / 2, -minEdgeOffset) : -unitSquareSize / 2;
                break;
            case 3:
                randomX = spawnInside ? UnityEngine.Random.Range(-unitSquareSize / 2, -unitSquareSize / 2 + maxInset) : -unitSquareSize / 2;
                randomY = UnityEngine.Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                break;
        }

        return new Vector3(randomX, randomY, 0);
    }

    public void AddEnemyToCurrentWave(string enemyType, Vector3 spawnPosition)
    {
        GameObject newEnemy = null;
        Slider newHealthSlider = Instantiate(healthSliderPrefab);
        Vector3 sliderPosition;

        switch (enemyType)
        {
            case "Acolyte":
                newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
                break;
            case "Kaboom":
                newEnemy = Instantiate(KaboomPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<KaboomEnemy>().maxHealth;
                break;
            case "Golem":
                newEnemy = Instantiate(GolemPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
                break;
            case "Apostate":
                newEnemy = Instantiate(Apostate_Prefab, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Apostate>().maxHealth;
                break;
            case "Necromancer":
                newEnemy = Instantiate(necromancer, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Necromancer>().maxHealth;
                break;
            case "Aegis":
                newEnemy = Instantiate(aegis, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Aegis>().maxHealth;
                break;
            case "Cleric":
                newEnemy = Instantiate(cleric, spawnPosition, Quaternion.identity);
                newHealthSlider.maxValue = newEnemy.GetComponent<Cleric>().maxHealth;
                break;
        }

        if (newEnemy != null)
        {
            sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
            newHealthSlider.transform.position = sliderPosition;
            newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            //newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
            waves[currentWave].numberOfEnemies++;
            _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
        }
    }

    public void Spawn_mistakes(Vector3 spawnPosition)
    {
        Vector3 spawnOffset = Random.insideUnitCircle * 0.5f;
        GameObject newEnemy = Instantiate(Mistake_Prefab, spawnPosition + spawnOffset, Quaternion.identity);
        SetupHealthSlider(newEnemy, newEnemy.GetComponent<Enemy>().maxHealth);
    }

    public void IncrementEnemyCount()
    {
        waves[currentWave].numberOfEnemies++;
        _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
    }

    public void AllEnemiesInWaveDestroyed()
    {
        UpdateTowerHealth();
        DestroyTowers();
        ToggleStartButton(true);
        _gameSpeedManager.DeactiveControlPanel();
        towersPlaced = 0;
        TowersLeft = 5;
        currentWave++;
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
        GameObject[] empties = GameObject.FindGameObjectsWithTag("Empty");
        foreach (GameObject empty in empties)
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
        GameObject[] buffed_icon_object = GameObject.FindGameObjectsWithTag("buffed_icon");
        foreach (GameObject buffed_icon_objects in buffed_icon_object)
        {
            IBuffTower towerScript = buffed_icon_objects.GetComponent<IBuffTower>();
            Destroy(buffed_icon_objects);
        }
        GameObject[] buff = GameObject.FindGameObjectsWithTag("Placed");
        foreach (GameObject buffe in buff)
        {
            IBuffTower towerScript = buffe.GetComponent<IBuffTower>();
            Destroy(buffe);
        }
    }

    public void SetCollision(bool tf)
    {
        collisionOccurred = tf;
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
}
