using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : MonoBehaviour
{

public GameObject enemyPrefab;
public UnityEngine.Transform targetCastle;
public float spawnInterval = 3.0f; // Adjust this to control spawn frequency.
public float unitSquareSize = 10.0f; // Adjust to your map size.
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
    // Calculate a random position on the perimeter of the unit square.
    Vector3 spawnPosition = CalculateRandomPerimeterSpawnPosition();

    // Instantiate the enemy prefab at the calculated spawn position.
    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
}

    Vector3 CalculateRandomPerimeterSpawnPosition()
    {
        // Generate a random side of the unit square (0-3 for top, right, bottom, left).
        int randomSide = Random.Range(0, 4);

        // Calculate random positions on the perimeter based on the selected side.
        float randomX = 0;
        float randomY = 0;

        switch (randomSide)
        {
            case 0: // Top side
                randomX = Random.Range(-unitSquareSize / 2, unitSquareSize / 2);
                randomY = unitSquareSize / 2;
                break;
            case 1: // Right side
                randomX = unitSquareSize / 2;
                randomY = Random.Range(-unitSquareSize / 2, unitSquareSize / 2);
                break;
            case 2: // Bottom side
                randomX = Random.Range(-unitSquareSize / 2, unitSquareSize / 2);
                randomY = -unitSquareSize / 2;
                break;
            case 3: // Left side
                randomX = -unitSquareSize / 2;
                randomY = Random.Range(-unitSquareSize / 2, unitSquareSize / 2);
                break;
        }

        // Calculate the spawn position.
        Vector3 spawnPosition = new Vector3(0,0,0) + new Vector3(randomX, randomY, 0);

        return spawnPosition;
    }
}