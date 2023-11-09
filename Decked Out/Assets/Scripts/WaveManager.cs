using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
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
    public GameObject enemyPrefab;
    public GameObject newEnemyPrefab;
    public float unitSquareSize = 10.0f;
    public float TowersLeft = 5;
    public bool kaboomEnemy = false;
    public Slider healthSliderPrefab;
    public List<Wave> waves = new List<Wave>();
    public TMP_Text towersLeftText;
    public bool collisionOccurred = false;
    private int enemiesSpawned = 0;
    public int spawnNewEnemyAfter = 4;
    private Coroutine spawningCoroutine;
    public CardRandoEngine cardRandoEngine;

    private GameLoader _loader;    

    private Button startButton;
    public int towersPlaced = 0;
    public int currentWave = 0;

    public WaveManager Initialize()
    {
        towersLeftText = FindObjectOfType<TMP_Text>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        Debug.Log("Wave Manager Initializing");       
        return this;
    }

    private void StartWaves()
    {
        ToggleStartButton(false);
        DestroyTowers();
        spawningCoroutine = StartCoroutine(StartWave());
  
    }

    public void SetStartButton(Button button)
    {
        startButton = button;
        startButton.onClick.AddListener(StartWaves);
        ToggleStartButton(true); 
    }
    public void SetText(Text text)
    {
        towersLeftText.text = "Towers Left to Place: " + TowersLeft;
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

                if (enemiesSpawned == spawnNewEnemyAfter)
                {
                    SpawnNewEnemy(); 
                    kaboomEnemy = true;
                    enemiesSpawned = 0; 
                }

                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
            }
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                cardRandoEngine.cardsInHand.Clear();
                cardRandoEngine.NewWave();
                yield return null;
            }            

            UpdateTowerHealth();

            ToggleStartButton(true);

            towersPlaced = 0;
            TowersLeft = 5;
            currentWave++;
        }
    }
    private void SpawnNewEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(newEnemyPrefab, spawnPosition, Quaternion.identity);

        Slider newHealthSlider = Instantiate(healthSliderPrefab);

        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(newEnemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;

        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = newEnemy.GetComponent<KaboomEnemy>().maxHealth;
        newEnemy.GetComponent<KaboomEnemy>().SetHealthSlider(newHealthSlider);
    }
    public void StopWave()
    {
        towersPlaced = 0;
        TowersLeft = 5;
        currentWave = 0;
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

        towersLeftText.text = "Towers Left to Place: " + TowersLeft;
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
}