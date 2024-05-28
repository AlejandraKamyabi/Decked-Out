using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista_Tower : MonoBehaviour, ITower
{
    public float attackRange;
    public GameObject arrowPrefab;
    [SerializeField] private float Damage;
    [SerializeField] private float RateOfFire;
    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private float initialDamage;
    private float initialRateOfFire;
    public GameObject effect;
    private GameObject buffed;
    private bool canAttack = true;
    private bool hasBeenBuffed = false;
    private Animator animator;
    public AudioSource audioSource;

    private float _BallistaAnimLength = 1.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Update()
    {
        FindAndShootTarget();
    }

    private void Start()
    {
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (animator != null)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals("Ballista_Animation"))
                {
                    _BallistaAnimLength = clip.length;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Animator component not found on Ballista_Tower");
        }
    }

    public float damage
    {
        get { return Damage; }
        set { Damage = value; }
    }

    public float attackSpeed
    {
        get { return RateOfFire; }
        set { RateOfFire = value; }
    }

    GameObject ITower.gameObject
    {
        get { return towerGameObject; }
        set { towerGameObject = value; }
    }

    public float health
    {
        get { return Health; }
        set { Health = value; }
    }

    public float range
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    public void ApplyBuff(float damageBuff, float rateOfFireBuff)
    {
        if (!hasBeenBuffed && !gameObject.CompareTag("Empty"))
        {
            Damage += damageBuff;
            RateOfFire *= rateOfFireBuff;
            if (RateOfFire < 0.1f)
            {
                RateOfFire = 0.1f;
            }
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null && health != 0)
            {
                buffed = Instantiate(effect, transform.position, Quaternion.identity);
            }
            hasBeenBuffed = true;
        }
    }

    private void FindAndShootTarget()
    {
        if (canAttack)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
            bool enemyFound = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    ShootArrow(collider.transform);
                    enemyFound = true;
                    break;
                }
            }

            if (!enemyFound)
            {
                animator.Play("Ballista_Animation", 0, 0); 
                animator.speed = 0;
            }
        }
    }

    private void ShootArrow(Transform target)
    {
        audioSource.Play();
        animator.speed = 1;
        animator.Play("Ballista_Animation", -1, 0);
        canAttack = false;

        StartCoroutine(ShootArrowCoroutine(target));
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator ShootArrowCoroutine(Transform target)
    {
        float offset = 0.25f;
        yield return new WaitForSeconds(_BallistaAnimLength - offset);
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Ballista_Arrow arrowScript = arrow.GetComponent<Ballista_Arrow>();
        arrowScript.SetTarget(target);
        arrowScript.SetDamage(Damage);
    }

    public void ResetTowerEffects()
    {
        Damage = initialDamage;
        RateOfFire = initialRateOfFire;
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color defaultColor = Color.white;
            spriteRenderer.color = defaultColor;
        }
        Destroy(buffed);
        hasBeenBuffed = false;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    private IEnumerator AttackCooldown()
    {
        float actualRateOfFire = RateOfFire;
        while (!canAttack)
        {
            yield return new WaitForSeconds(actualRateOfFire);
            canAttack = true;
        }
    }

    private void OnDestroy()
    {
        if (buffed != null)
        {
            Destroy(buffed);
        }
    }
}
