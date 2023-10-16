using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

public GameObject enemyPrefab;
public UnityEngine.Transform targetCastle;
public float spawnInterval = 3.0f; 
public float unitSquareSize = 10.0f; 
public float moveSpeed = 1f;
private float timeSinceLastSpawn = 0.0f;
private bool isSpawning = false;

private void Update()
{
    timeSinceLastSpawn += Time.deltaTime;

    if (timeSinceLastSpawn >= spawnInterval && !isSpawning)
    {
        isSpawning = true;
        SpawnEnemy();
        timeSinceLastSpawn = 0.0f;
    }
        if (targetCastle != null)
        {

            Vector3 moveDirection = (targetCastle.position - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        }
    }

void SpawnEnemy()
{

    Vector3 spawnPosition = CalculateRandomPerimeterSpawnPosition();

    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
}

    Vector3 CalculateRandomPerimeterSpawnPosition()
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

        Vector3 spawnPosition = new Vector3(0,0,0) + new Vector3(randomX, randomY, 0);

        return spawnPosition;
    }
}