using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttractionTower : MonoBehaviour, ITower
{
    public float attackRange;
    public GameObject HeartPrefab;
    [SerializeField] private float Damage;
    [SerializeField] private float RateOfFire;
    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private float initialDamage;
    private float initialRateOfFire;
    private bool canAttack = true;
    private bool hasBeenBuffed = false;
    public AudioSource audioSource;

    private List<GameObject> recentlyShotEnemies = new List<GameObject>();
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Update()
    {
        FindAndShootTarget();
        if (health == 0)
        {
            spriteRenderer.color = Color.red;
        }
    }
    private void Start()
    {
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && health != 0)
            {
                Color buffColor = new Color(1.0f, 0.768f, 0.290f, 1.0f);
                spriteRenderer.color = buffColor;
            }
            hasBeenBuffed = true;

        }
    }
    private void FindAndShootTarget()
    {
        if (canAttack)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy") && !recentlyShotEnemies.Contains(collider.gameObject))
                {
                    recentlyShotEnemies.Add(collider.gameObject);
                    ShootHeart(collider.transform);
                    break;
                }
            }
        }
    }

    private void ShootHeart(Transform target)
    {
        //audioSource.Play();
        GameObject Heart = Instantiate(HeartPrefab, transform.position, Quaternion.identity);
        Heart_Projectile Heart_Script = Heart.GetComponent<Heart_Projectile>();
        Heart_Script.SetTarget(target);
        Heart_Script.SetTower(this.transform);
        canAttack = false;

        Heart_Script.SetDamage(Damage);

        StartCoroutine(AttackCooldown());

    }
    public void ResetTowerEffects()
    {
        Damage = initialDamage;
        RateOfFire = initialRateOfFire;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color defaultColor = Color.white;
            spriteRenderer.color = defaultColor;
        }
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

}
