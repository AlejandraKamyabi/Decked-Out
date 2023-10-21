using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

public UnityEngine.Transform targetCastle;

public float unitSquareSize = 10.0f; 
public float moveSpeed = 1f;
public float damage = 10.0f; 

public float maxHealth = 100.0f; 
private float currentHealth;
public Slider healthSlider;



    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {

        currentHealth -= damage;
        UpdateEnemyHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(healthSlider.gameObject);
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
            Destroy(healthSlider.gameObject);
            Destroy(gameObject);
        }
    }
    private void Update()
    {

        if (targetCastle != null)
        {
            Vector3 moveDirection = (targetCastle.position - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            if (healthSlider != null)
            {
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
                healthSlider.transform.position = screenPosition + new Vector3(0, 70.0f, 0); 
            }
        }

    }
    private void UpdateEnemyHealthUI()
    {

        healthSlider.value = currentHealth;
    }
    public void SetHealthSlider(Slider slider)
    {
        healthSlider = slider;
    }


}