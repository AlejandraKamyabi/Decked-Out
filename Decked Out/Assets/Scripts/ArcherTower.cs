using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArcherTower : MonoBehaviour
{
    public float attackRange = 5.0f; 
    public GameObject arrowPrefab; 
    public float attackCooldown = 1.0f;
    public float damage;
    private float lastAttackTime;
    private bool canAttack = true;

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

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
