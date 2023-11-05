using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlamethrowerTower : MonoBehaviour, ITower
{
    public float attackRange;
    public GameObject Flame;
    [SerializeField] private float Damage = 3.0f;
    [SerializeField] private float RateOfFire = 1.0f;
    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private float lastAttackTime;
    private bool canAttack = true;
    private bool hasBeenBuffed = false;
    private void Update()
    {
        FindAndShootTarget();
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
            if (spriteRenderer != null)
            {
                Color buffColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);
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
                if (collider.CompareTag("Enemy"))
                {
                    ShootArrow(collider.transform);
                    break;
                }
            }
        }
    }

    private void ShootArrow(Transform target)
    {

        GameObject arrow = Instantiate(Flame, transform.position, Quaternion.identity);
        FlamesEffect flameScript = arrow.GetComponent<FlamesEffect>();
        flameScript.SetTarget(target);

        lastAttackTime = Time.time;
        canAttack = false;
        flameScript.SetDamage(Damage);
        StartCoroutine(AttackCooldown());

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
