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
    public delegate void SpawnAction();

    // Managing Wave
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
    [Range(1, 25)]
    private int enemiesBetweenMopeySpawns = 9;

    private EnemyKillTracker _killTracker;
    private Coroutine spawningCoroutine;
    public CardRandoEngine cardRandoEngine;
    private Button startButton;
    public int towersPlaced = 0;
    public int currentWave = 0;
    public CardHandling deck_Building;

    public WaveManager Initialize()
    {
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        deck_Building = FindObjectOfType<CardHandling>();
        _killTracker = FindObjectOfType<EnemyKillTracker>();
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
        _killTracker.NumbersOfEnemiesInWave(numberOfEnemies);
        enemiesSpawned = 0;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (enemiesSpawned >= numberOfEnemies)
            {
                Debug.LogError("Trying to spawn more enemies than allowed");
                break;
            }
            else if (ShouldSpawnSpecialEnemy(enemiesSpawned, out SpawnAction spawnAction))
            {
                spawnAction.Invoke();
                enemiesSpawned++;
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

    private bool ShouldSpawnSpecialEnemy(int enemiesSpawned, out SpawnAction spawnAction)
    {
        spawnAction = null;
        if (enemiesSpawned % enemiesBetweenNecromancerSpawns == 0 && enemiesSpawned != 0)
        {
            spawnAction = Spawn_Necromancer;
            return true;
        }
        else if (enemiesSpawned % enemiesBetweenAegisSpawns == 0 && enemiesSpawned != 0)
        {
            spawnAction = Spawn_Aegis;
            return true;
        }
        else if (enemiesSpawned % enemiesBetweenClericSpawns == 0 && enemiesSpawned != 0)
        {
            spawnAction = Spawn_Cleric;
            return true;
        }
        else if (enemiesSpawned % enemiesBetweenApostateSpawns == 0 && enemiesSpawned != 0)
        {
            spawnAction = SpawnApostateEnemy;
            return true;
        }
        else if (enemiesSpawned % enemiesBetweenGolemSpawns == 0 && enemiesSpawned != 0)
        {
            spawnAction = SpawnGolemEnemy;
            return true;
        }
        else if (enemiesSpawned % enemiesBetweenKaboomSpawns == 0 && enemiesSpawned != 0)
        {
            spawnAction = SpawnKaboomEnemy;
            return true;
        }
        else if (enemiesSpawned % enemiesBetweenMopeySpawns == 0 && enemiesSpawned != 0)
        {
            spawnAction = Spawn_Mopey;
            return true;
        }
        return false;
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

    private void Spawn_Mopey()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(Mopey_prefab, spawnPosition, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<Mopey_Misters>().maxHealth;
        newEnemy.GetComponent<Mopey_Misters>().SetHealthSlider(newHealthSlider);
    }

    public void Spawn_mistakes(Vector3 spawnPosition)
    {
        Vector3 spawnOffset = Random.insideUnitCircle * 0.5f;
        GameObject newEnemy = Instantiate(Mistake_Prefab, spawnPosition + spawnOffset, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
        newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
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
        GameObject[] buff = GameObject.FindGameObjectsWithTag("Placed");
        foreach (GameObject buffe in buff)
        {
            IBuffTower towerScript = buffe.GetComponent<IBuffTower>();
            Destroy(buffe);
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
                waves[currentWave].numberOfEnemies++;
                Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
                newHealthSlider.transform.position = sliderPosition;
                _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
                newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
                break;
            case "Kaboom":
                newEnemy = Instantiate(KaboomPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider = Instantiate(healthSliderPrefab);
                waves[currentWave].numberOfEnemies++;
                sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
                newHealthSlider.transform.position = sliderPosition;
                _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<KaboomEnemy>().maxHealth;
                newEnemy.GetComponent<KaboomEnemy>().SetHealthSlider(newHealthSlider);
                break;
            case "Apostate":
                newEnemy = Instantiate(Apostate_Prefab, spawnPosition, Quaternion.identity);
                newHealthSlider = Instantiate(healthSliderPrefab);
                waves[currentWave].numberOfEnemies++;
                Vector3 apostateSliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 1700.0f, 0));
                newHealthSlider.transform.position = apostateSliderPosition;
                _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<Apostate>().maxHealth;
                newEnemy.GetComponent<Apostate>().SetHealthSlider(newHealthSlider);
                break;
            case "Golem":
                newEnemy = Instantiate(GolemPrefab, spawnPosition, Quaternion.identity);
                newHealthSlider = Instantiate(healthSliderPrefab);
                waves[currentWave].numberOfEnemies++;
                Vector3 golemSliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 1700.0f, 0));
                newHealthSlider.transform.position = golemSliderPosition;
                _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<Enemy>().maxHealth;
                newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
                break;
            case "Aegis":
                newEnemy = Instantiate(aegis, spawnPosition, Quaternion.identity);
                newHealthSlider = Instantiate(healthSliderPrefab);
                waves[currentWave].numberOfEnemies++;
                Vector3 aegisSliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 1700.0f, 0));
                newHealthSlider.transform.position = aegisSliderPosition;
                _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<Aegis>().maxHealth;
                newEnemy.GetComponent<Aegis>().SetHealthSlider(newHealthSlider);
                break;
            case "Cleric":
                newEnemy = Instantiate(cleric, spawnPosition, Quaternion.identity);
                newHealthSlider = Instantiate(healthSliderPrefab);
                waves[currentWave].numberOfEnemies++;
                Vector3 clericSliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 1700.0f, 0));
                newHealthSlider.transform.position = clericSliderPosition;
                _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
                newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newHealthSlider.maxValue = newEnemy.GetComponent<Cleric>().maxHealth;
                newEnemy.GetComponent<Cleric>().SetHealthSlider(newHealthSlider);
                break;
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
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int randomSide = UnityEngine.Random.Range(0, 4);
        bool spawnInside = UnityEngine.Random.value < 0.3f;

        float insetFactor = spawnInside ? 0.2f : 0.0f;

        float maxInset = unitSquareSize * insetFactor;
        float minEdgeOffset = unitSquareSize / 2 - maxInset;

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

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
        return spawnPosition;
    }

    public void IncrementEnemyCount()
    {
        waves[currentWave].numberOfEnemies++;
        _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
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
