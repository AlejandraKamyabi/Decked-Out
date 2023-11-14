using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaboomEnemy : MonoBehaviour
{
    public UnityEngine.Transform targetCastle;
    public float moveSpeed = 1f;
    public float damage = 10.0f;
    public float maxHealth = 100.0f;
    private float currentHealth;
    public Slider healthSlider;
    public GameObject zapPrefab;
    private bool hasBeenZapped = false;
    public bool isBurning = false;
    public GameObject effect;
    private float damageTimer = 1.0f;
    public bool isFrozen = false;
    private float timeSinceLastDamage = 0.0f;
    public AudioClip deathSound;
    private EnemyDeathSoundHandling deathSoundHandling;
    private EnemyKillTracker enemyKillTracker;

    private void Start()
    {
        currentHealth = maxHealth;
        timeSinceLastDamage = damageTimer;
        deathSoundHandling = GetComponent<EnemyDeathSoundHandling>();
        deathSoundHandling.enemyDeathSound = deathSound;
        enemyKillTracker = GameObject.FindObjectOfType<EnemyKillTracker>();
    }

    private void Update()
    {
        if (targetCastle != null)
        {
            Vector3 moveDirection = (targetCastle.position + new Vector3(0f, -1f, 0) - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            if (healthSlider != null)
            {
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
                healthSlider.transform.position = screenPosition + new Vector3(0, 70.0f, 0);
            }
        }
        if (isBurning)
        {
            timeSinceLastDamage += Time.deltaTime;

            if (timeSinceLastDamage >= damageTimer)
            {
                timeSinceLastDamage = 0.0f;
                TakeDamage(20.0f);
            }
        }
        if (isFrozen)
        {
            moveSpeed = 0.39f;
        }
    }
    private void DealAOEDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy otherEnemy = collider.GetComponent<Enemy>();
                if (otherEnemy != null)
                {
                    otherEnemy.TakeDamage(10);
                }
            }
        }
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
        deathSoundHandling.PlayDeathSound();
        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
       

        DealAOEDamage();

        Destroy(healthSlider.gameObject);
        Destroy(gameObject);
        Destroy(deathEffect, 4.0f);

        if (enemyKillTracker != null)
        {
            enemyKillTracker.EnemyDestroyed();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Castle castle = collision.gameObject.GetComponent<Castle>();
            if (castle != null)
            {
                castle.TakeDamage(damage);
            }
            GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(healthSlider.gameObject);
            Destroy(gameObject);
            Destroy(deathEffect, 4.0f);
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
    public void setBurning()
    {
        isBurning = true;
    }
    public void ApplyFreeze()
    {
        if (!isFrozen)
        {
            isFrozen = true;
            StartCoroutine(DisableFreezeAfterDuration(3.0f));
        }
    }
    private IEnumerator DisableFreezeAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        moveSpeed = 1.0f;
    }
    public void Zap()
    {
        if (!hasBeenZapped)
        {
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 6f);

            foreach (Collider2D enemyCollider in nearbyEnemies)
            {
                if (enemyCollider.CompareTag("Enemy") && enemyCollider.gameObject != this.gameObject)
                {
                    GameObject zapPrefabInstance = Instantiate(zapPrefab, transform.position, Quaternion.identity);
                    ZapProjectile zapProjectile = zapPrefabInstance.GetComponent<ZapProjectile>();

                    if (zapProjectile != null)
                    {
                        zapProjectile.SetTarget(enemyCollider.transform);
                        zapProjectile.SetDamage(30f);
                    }
                }
            }
        }
    }
    public void ResetZapFlag()
    {
        hasBeenZapped = true;
    }
}