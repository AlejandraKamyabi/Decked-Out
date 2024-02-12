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
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void Update()
    {
        if (health == 0)
        {
            spriteRenderer.color = Color.red;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                KaboomEnemy kaboom = collider.GetComponent<KaboomEnemy>();
                Apostate apostate = collider.GetComponent<Apostate>();

                if (enemy != null)
                {
                  
                    enemy.ApplyFreeze();

                }
                if (kaboom != null)
                {
                    kaboom.ApplyFreeze();

                }
                if (apostate != null)
                {
                    apostate.ApplyFreeze();

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
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
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
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
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