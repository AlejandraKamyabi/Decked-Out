using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float currentHealth;

    public Slider healthSlider;
    public EndGameSplashManager endGame;
    private GameLoader _loader;
    private WaveManager wave;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    public void Initialize()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        wave = ServiceLocator.Get<WaveManager>();
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
        endGame.Death();
        wave.StopWave();

    }
    public void ResetHealth()
    {
        currentHealth = 100;
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {

        healthSlider.value = currentHealth;
    }
}
