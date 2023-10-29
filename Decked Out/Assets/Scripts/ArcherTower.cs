using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArcherTower : MonoBehaviour
{
    public float attackRange; 
    public GameObject arrowPrefab; 
    public float attackSpeed = 1.0f;
    public float damage;
    private float lastAttackTime;
    public bool collisionOccurredd = false;
    private bool canAttack = true;
    public int towerHealth = 3;
    private void Update()
    {
        FindAndShootTarget();
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

        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.SetTarget(target);

        lastAttackTime = Time.time;
        canAttack = false;
        arrowScript.SetDamage(damage);
        StartCoroutine(AttackCooldown());

    }
    public float GetAttackRange()
    {
        return attackRange;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
    public void setbool()
    {
        collisionOccurredd = true;
    }
}
