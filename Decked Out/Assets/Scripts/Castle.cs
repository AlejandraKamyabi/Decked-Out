using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public float maxHealth = 100.0f; 
    private float currentHealth;

    public Slider healthSlider;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

    }

    private void UpdateHealthUI()
    {

        healthSlider.value = currentHealth / maxHealth;
    }
}
