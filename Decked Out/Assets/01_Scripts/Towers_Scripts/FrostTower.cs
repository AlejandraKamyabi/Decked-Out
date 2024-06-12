using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTower : MonoBehaviour, ITower
{
    public float attackRange = 5.0f;
    private float Damage = 10.0f;
    private float RateOfFire = 1.0f;
    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private float initialDamage;
    public GameObject effect;
    private GameObject buffed;
    private float initialRateOfFire;
    private bool hasBeenBuffed = false;
    private void Start()
    {
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void Update()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                KaboomEnemy kaboom = collider.GetComponent<KaboomEnemy>();
                Mopey_Misters mopey = collider.GetComponent<Mopey_Misters>();
                Apostate apostate = collider.GetComponent<Apostate>();
                Cleric cleric = collider.GetComponent<Cleric>();

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
                Aegis aegis = collider.GetComponent<Aegis>();
                if (aegis != null)
                {
                    aegis.TakeDamage(damage);

                }
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
    private void OnDestroy()
    {
        if (buffed != null)
        {
            Destroy(buffed);
        }
    }
}