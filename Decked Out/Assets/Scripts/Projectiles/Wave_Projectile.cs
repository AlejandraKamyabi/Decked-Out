using System.Collections.Generic;
using UnityEngine;

public class Wave_Projectile : MonoBehaviour
{
    public float waveSpeed = 10.0f;
    private float damage;
    [SerializeField] private float pushForce; 
    private Transform target;
    private HashSet<GameObject> pushedEnemies = new HashSet<GameObject>();
    private bool shouldRotate = true;

    private void Update()
    {
        if (target == null || target.gameObject == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
        Vector2 directionToTarget = (targetPosition - currentPosition).normalized;
        float stopDistance = 0.3f;  
        Vector2 offsetPosition = targetPosition - (directionToTarget * stopDistance);

        transform.position = Vector2.MoveTowards(currentPosition, offsetPosition, waveSpeed * Time.deltaTime);
        Vector2 direction = targetPosition - currentPosition;
        if (shouldRotate)
        {
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (Vector2.Distance(currentPosition, offsetPosition) < stopDistance)
        {
            DealDamage(target.gameObject);
            PushEnemy(target.gameObject, direction);
            shouldRotate = false; 
            Destroy(gameObject, 2f);
        }
    
}

    public void SetDamage(float value)
    {
        damage = value;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void PushEnemy(GameObject enemy, Vector2 direction)
    {
        if (!pushedEnemies.Contains(enemy))
        {
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(direction.normalized * pushForce, ForceMode2D.Impulse);
                pushedEnemies.Add(enemy);

                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.ApplyPushback(2f, 0f); 
                }
                KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
                if (kaboom != null)
                {
                    kaboom.ApplyPushback(2f, 0f);
                }
            }
        }
    }
    private void DealDamage(GameObject enemy)
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
    }
}

