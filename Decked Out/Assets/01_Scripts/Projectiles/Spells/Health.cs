using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float attackRange = 2f;
    private PlayerHealth playerHealth; 

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        Debug.Log("拿到player Health");
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("进入player");

        if (collision.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.RecoverHealth(5.0f);  
            }
            Destroy(collision.gameObject);

            Debug.Log("回血");
        }
    }
}


public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100;

    public void RecoverHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
     
        Debug.Log("Current Health: " + currentHealth);
    }
}
