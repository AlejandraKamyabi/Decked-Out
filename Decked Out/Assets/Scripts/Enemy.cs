using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float moveSpeed = 2.0f; 
    public Transform targetCastle;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    private float timeSinceLastSpawn = 0.0f;
    public float spawnInterval = 3.0f;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
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
        if (spawnPoints.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        
    }
}
