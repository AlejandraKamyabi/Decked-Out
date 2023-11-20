using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuack : MonoBehaviour, ITower
{
    public float attackRange = 5.0f;
    [SerializeField] private float Damage = 10.0f;
    private float RateOfFire = 1.0f;
    public GameObject effect;
    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private bool hasBeenBuffed = false;
    private void Start()
    {
        StartCoroutine(DamageOverTime());
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

                if (enemy != null)
                {

                    enemy.ApplyFreeze();
 


                }
                if (kaboom != null)
                {
                    kaboom.ApplyFreeze();


                }
            }
        }
    }
    private IEnumerator DamageOverTime()
    {
        while (true) 
        {

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    KaboomEnemy kaboom = collider.GetComponent<KaboomEnemy>();

                    if (enemy != null)
                    {
                        enemy.TakeDamage(Damage);
                        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(deathEffect, 0.5f);
                    }
                    if (kaboom != null)
                    {
                        kaboom.TakeDamage(Damage);
                        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(deathEffect, 0.5f);
                    }
                }
            }
            yield return new WaitForSeconds(2.0f);
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
            if (spriteRenderer != null)
            {
                Color buffColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);
                spriteRenderer.color = buffColor;
            }
            hasBeenBuffed = true;

        }
    }
}
