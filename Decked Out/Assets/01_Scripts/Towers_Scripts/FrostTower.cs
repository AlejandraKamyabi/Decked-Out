using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTower : MonoBehaviour, ITower
{
    public float attackRange = 5.0f;
    public GameObject IceBulletPrefab;
    [SerializeField] private float Damage = 10.0f;
    [SerializeField] private float RateOfFire = 1.0f;
    [SerializeField] private float Health = 2;
    public GameObject effect;
    private GameObject buffed;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private float initialDamage;
    private float initialRateOfFire;
    private bool canAttack = true;
    private bool hasBeenBuffed = false;
    private Animator animator;
    public AudioSource audioSource;

    private float _iceTowerAnimLength = 1.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Update()
    {
        FindAndFreezeTarget();
    }

    private void Start()
    {
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("IceTower_Animation"))
            {
                _iceTowerAnimLength = clip.length;
                break;
            }
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

    public float range
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    public float health
    {
        get { return Health; }
        set { Health = value; }
    }

    GameObject ITower.gameObject
    {
        get { return towerGameObject; }
        set { towerGameObject = value; }
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

    private void FindAndFreezeTarget()
    {
        if (canAttack)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    FreezeTarget(collider.gameObject);
                    break;
                }
            }
        }
    }

    private void FreezeTarget(GameObject target)
    {
        canAttack = false;
        ApplyFreeze(target);
        StartCoroutine(AttackCooldown());
    }

    private void ApplyFreeze(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        KaboomEnemy kaboom = target.GetComponent<KaboomEnemy>();
        Mopey_Misters mopey = target.GetComponent<Mopey_Misters>();
        Apostate apostate = target.GetComponent<Apostate>();
        Cleric cleric = target.GetComponent<Cleric>();

        if (enemy != null)
        {
            enemy.ApplyFreeze(0.3f);
        }
        if (kaboom != null)
        {
            kaboom.ApplyFreeze(0.3f);
        }
        if (apostate != null)
        {
            apostate.ApplyFreeze(0.3f);
        }
        if (cleric != null)
        {
            cleric.ApplyFreeze(0.3f);
        }
        if (mopey != null)
        {
            mopey.ApplyFreeze(0.3f);
        }
        Aegis aegis = target.GetComponent<Aegis>();
        if (aegis != null)
        {
            aegis.TakeDamage(damage);
        }
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
