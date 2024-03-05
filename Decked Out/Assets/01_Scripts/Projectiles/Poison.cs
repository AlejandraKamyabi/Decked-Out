using System.Collections;
using UnityEngine;

public class Poison : MonoBehaviour
{
   [SerializeField] public float range = 5f;
   [SerializeField] public float damage = 10f;
    private bool canAttack = true;

    private void Start()
    {
        Destroy(gameObject, 5f); // Destroy after 5 seconds
    }

    private void Update()
    {
        FindAndShootTarget();
    }
    private void FindAndShootTarget()
    {
        if (canAttack)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    DamageOverTime(collider.gameObject);
                    break;
                }
            }
        }
    }
    private IEnumerator AttackCooldown()
    {
   
        while (!canAttack)
        {
            yield return new WaitForSeconds(0.5f);
            canAttack = true;
        }
    }

    private void DamageOverTime(GameObject enemy)
    {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
            KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
            if (kaboom != null)
            {
                kaboom.TakeDamage(damage);
          

            }
            Apostate apostate = enemy.GetComponent<Apostate>();
            if (apostate != null)
            {
                apostate.TakeDamage(damage);

            }
            Necromancer necromancer = enemy.GetComponent<Necromancer>();
            if (necromancer != null)
            {
                necromancer.TakeDamage(damage);
             
            }
            canAttack = false;
            StartCoroutine(AttackCooldown());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            StopAllCoroutines(); 
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}