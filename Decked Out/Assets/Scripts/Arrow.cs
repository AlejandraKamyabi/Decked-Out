using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed = 10.0f;
    private float damage;
    private Transform target;

    private void Update()
    {
      if (target == null)
      {
      
          return;
      }

        transform.position = Vector3.MoveTowards(transform.position, target.position, arrowSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            DealDamage(target.gameObject);
            Destroy(gameObject);
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

    private void DealDamage(GameObject enemy)
    {
  
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
    }
}