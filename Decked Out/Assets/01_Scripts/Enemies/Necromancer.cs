using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Necromancer : MonoBehaviour
{
    public UnityEngine.Transform targetCastle;
    public float moveSpeed = 1f;
    private float original_moveSpeed;
    public float damage = 10.0f;
    public float maxHealth;
    private float currentHealth;
    public Slider healthSlider;
    public GameObject zapPrefab;
    public bool isBurning = false;
    private bool hasBeenZapped = false;
    public float detectionRadius;
    private float damageTimer = 1.0f;
    public bool isFrozen = false;
    public GameObject deathEffectPrefab;
    private HashSet<GameObject> detectedEnemy = new HashSet<GameObject>();
    private float timeSinceLastDamage = 0.0f;
    public AudioClip deathSound;
    private EnemyDeathSoundHandling deathSoundHandling;
    private EnemyKillTracker _killTracker;
    [SerializeField] private CircleCollider2D circleCollider;

    //Attraction tower 

    private Transform originalTarget;
    private bool isAttracted;

    //Wave_Tower
    public bool isBeingPushed = false;
    EnemyDeathAnimation _enemyDeathAnimation;
    CapsuleCollider2D _capsuleCollider;
    bool _isDead = false;


    private WaveManager wave;
    private GameLoader _loader;

    private void Start()
    {
        currentHealth = maxHealth;
        timeSinceLastDamage = damageTimer;
        original_moveSpeed = moveSpeed;
        deathSoundHandling = GetComponent<EnemyDeathSoundHandling>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
        deathSoundHandling.enemyDeathSound = deathSound;
        _killTracker = GameObject.FindObjectOfType<EnemyKillTracker>();
        _enemyDeathAnimation = GetComponent<EnemyDeathAnimation>();
    }
    public void Initialize()
    {
        wave = ServiceLocator.Get<WaveManager>();
    }
    private void Update()
    {
        DetectAndAddAcolyte();
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
                TakeDamage(10.0f);
            }
        }
     
        if (isFrozen)
        {
            moveSpeed = 0.39f;
        }
    }
    public void HandleWaveImpact(Vector2 direction, float duration, float distance)
    {
        if (!isBeingPushed)
        {
            Vector2 oppositeDirection = -direction.normalized;
            isBeingPushed = true;
            StartCoroutine(ManualPushback(oppositeDirection, duration, distance));
        }
    }
    public IEnumerator ManualPushback(Vector2 direction, float duration, float distance)
    {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition - direction.normalized * distance;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isBeingPushed = false;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateEnemyHealthUI();

        if (currentHealth <= 0 && !_isDead)
        {
            Die();
        }
    }
    private void DetectAndAddAcolyte()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (var collider in detectedObjects)
        {
            if (collider.CompareTag("Acolyte") && !detectedEnemy.Contains(collider.gameObject))
            {
                detectedEnemy.Add(collider.gameObject);
                wave.AddEnemyToCurrentWave("Acolyte", collider.transform.position);
            }
            else if (collider.CompareTag("Kaboom") && !detectedEnemy.Contains(collider.gameObject))
            {
                detectedEnemy.Add(collider.gameObject);
                wave.AddEnemyToCurrentWave("Kaboom", collider.transform.position);
            }
            else if (collider.CompareTag("Golem") && !detectedEnemy.Contains(collider.gameObject))
            {
                detectedEnemy.Add(collider.gameObject);
                wave.AddEnemyToCurrentWave("Golem", collider.transform.position);
            }
            else if (collider.CompareTag("Apostate") && !detectedEnemy.Contains(collider.gameObject))
            {
                detectedEnemy.Add(collider.gameObject);
                wave.AddEnemyToCurrentWave("Apostate", collider.transform.position);
            }
            else if (collider.CompareTag("Necromancer") && !detectedEnemy.Contains(collider.gameObject))
            {
                detectedEnemy.Add(collider.gameObject);
                wave.AddEnemyToCurrentWave("Necromancer", collider.transform.position);
            }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
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
    private IEnumerator reset_Field()
    {
        yield return new WaitForSeconds(2);
    }
    private void Die()
    {
        _isDead = true;
        moveSpeed = 0;
        _capsuleCollider.enabled = false;
        deathSoundHandling.PlayDeathSound();
        if (_killTracker != null)
        {
            _killTracker.EnemyKilled();
        }
        float deathAnimationDuration = _enemyDeathAnimation.PlayDeathAnimation();
        healthSlider.gameObject.SetActive(false);
        Destroy(healthSlider.gameObject, deathAnimationDuration);
        GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        Destroy(deathEffect, 10f);
        Destroy(gameObject, deathAnimationDuration);
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
            _killTracker.EnemyDestroyed();
            Destroy(healthSlider.gameObject);
            Destroy(gameObject);


        }
        if (circleCollider == null)
        {

            // if (collision.gameObject.CompareTag("Field"))
            // {
            //     Field force_Field = collision.gameObject.GetComponent<Field>();
            //     force_Field.StartFlickerEffect();
            //     StartCoroutine(reset_Field());
            //     force_Field.ResetFieldPrefabChanges();
            // }

            return;
        }

        if (collision.gameObject.CompareTag("Field"))
        {
            circleCollider.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (circleCollider == null) return;

        if (collision.gameObject.CompareTag("Field"))
        {
            circleCollider.enabled = false;
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
        moveSpeed = original_moveSpeed;
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