using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float unitSquareSize = 10.0f;
    public Slider healthSliderPrefab;
    public List<Wave> waves = new List<Wave>();
    
    private Button startButton; // Reference to the button that starts the waves.

    private int currentWave = 0;

    public WaveManager Initialize()
    {
        Debug.Log("Wave Manager Initializing");
        return this;
    }

    private void StartWaves()
    {
        ToggleStartButton(false); // Disable and hide the button while the wave is running.
        StartCoroutine(StartWave());
    }

    public void SetStartButton(Button button)
    {
        startButton = button;
        startButton.onClick.AddListener(StartWaves);
        ToggleStartButton(true); // Enable and show the button at the start.
    }

    private IEnumerator StartWave()
    {

        if (currentWave < waves.Count)
        {
            int numberOfEnemies = waves[currentWave].numberOfEnemies;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
            }

            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return null;
            }

            ToggleStartButton(true); 
            currentWave++;
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
    private void ToggleStartButton(bool isEnabled)
    {
        startButton.interactable = isEnabled;
        startButton.gameObject.SetActive(isEnabled);
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
}