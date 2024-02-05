using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Projectile : MonoBehaviour
{
    public float waveSpeed = 10.0f;
    private float damage;
    private Transform target;
    private bool shouldRotate = true;
    private bool hasHit = false;
    private bool canMove = false;
    [SerializeField] private float force;
    [SerializeField] private float duration;
    void Start()
    {
        StartCoroutine(EnableMovementAfterDelay(0.8f)); 

    }
    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true; 
    }

    private void Update()
    {
        if (!canMove) return;
        if (target == null || target.gameObject == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
        Vector2 directionToTarget = (targetPosition - currentPosition).normalized;
        float stopDistance = 0.7f;  
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
            if (!hasHit) 
            {
                DealDamage(target.gameObject);
                PushEnemy(target.gameObject, direction);
                hasHit = true;
            }

            shouldRotate = false;
            Destroy(gameObject, 1.5f);
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
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.HandleWaveImpact(direction, duration, force);
        }
        KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.HandleWaveImpact(direction ,duration, force);
        }
        Apostate apostate = enemy.GetComponent<Apostate>();
        if (apostate != null)
        {
            apostate.HandleWaveImpact(direction, duration, force);
            
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

