using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

public GameObject enemyPrefab;
public UnityEngine.Transform targetCastle;

public float unitSquareSize = 10.0f; 
public float moveSpeed = 1f;
public float damage = 10.0f; 

public float maxHealth = 100.0f; 
private float currentHealth;


//public Transform healthBar;
private Vector3 healthBarScale;


    private void Start()
    {
        currentHealth = maxHealth;
       // healthBarScale = healthBar.localScale;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        float healthPercentage = currentHealth / maxHealth;
        healthBarScale.x = Mathf.Clamp(healthPercentage, 0f, 1f);
       // healthBar.localScale = healthBarScale;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Castle castle = collision.gameObject.GetComponent<Castle>();
            if (castle != null)
            {
                castle.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
        private void Update()
{

        if (targetCastle != null)
        {

            Vector3 moveDirection = (targetCastle.position - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        }
    }



 
      
}