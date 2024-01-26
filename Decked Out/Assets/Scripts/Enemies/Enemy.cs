using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public UnityEngine.Transform targetCastle;
    public float moveSpeed = 1f;
    public float damage = 10.0f;
    public float maxHealth;
    private float currentHealth;
    public Slider healthSlider;
    public GameObject zapPrefab;
    public bool isBurning = false;
    private bool hasBeenZapped = false;
    private float damageTimer = 1.0f;
    public bool isFrozen = false;
    private float timeSinceLastDamage = 0.0f;
    public AudioClip deathSound;
    private EnemyDeathSoundHandling deathSoundHandling;
    private EnemyKillTracker EnemyKillTracker;

    //Attraction tower 

    private Transform originalTarget;
    private bool isAttracted;

    private void Start()
    {
        currentHealth = maxHealth;
        timeSinceLastDamage = damageTimer;
        deathSoundHandling = GetComponent<EnemyDeathSoundHandling>();
        deathSoundHandling.enemyDeathSound = deathSound;
        EnemyKillTracker = GameObject.FindObjectOfType<EnemyKillTracker>();
    }

    private void Update()
    {
        if (targetCastle != null)
        {
            Vector2 moveDirection = (targetCastle.position + new Vector3(0f, -1f, 0) - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            if (healthSlider != null)
            {
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
                healthSlider.transform.position = screenPosition + new Vector2(0, 70.0f);
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
    public void ApplyPushback(float duration, float reducedSpeed)
    {
        StartCoroutine(TemporarySpeedReduction(duration, reducedSpeed));
    }

    private IEnumerator TemporarySpeedReduction(float duration, float reducedSpeed)
    {
        float originalSpeed = moveSpeed;
        moveSpeed = reducedSpeed;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
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
    public void Attracted(Transform attractionTower)
    {
        if (!isAttracted) 
        {
            originalTarget = targetCastle;
            targetCastle = attractionTower;
            isAttracted = true; 
            StartCoroutine(ResetAttracted()); 
        }
    }

    private IEnumerator ResetAttracted()
    {
        yield return new WaitForSeconds(5);
        targetCastle = originalTarget;
        isAttracted = false;
    }
    private void Die()
    {
        deathSoundHandling.PlayDeathSound();
        Destroy(healthSlider.gameObject);
        Destroy(gameObject);

        if (EnemyKillTracker != null)
        {
            EnemyKillTracker.EnemyDestroyed();
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
            Destroy(healthSlider.gameObject);
            Destroy(gameObject);
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
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 2f);

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