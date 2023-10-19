using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<Wave> waves = new List<Wave>(); 

    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        while (currentWave < waves.Count)
        {
            int numberOfEnemies = waves[currentWave].numberOfEnemies;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
            }

            yield return new WaitForSeconds(waves[currentWave].timeBetweenWaves);
            currentWave++;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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