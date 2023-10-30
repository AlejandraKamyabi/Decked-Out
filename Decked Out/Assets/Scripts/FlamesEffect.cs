using UnityEngine;

public class FlamesEffect : MonoBehaviour
{
    public float speed = 10.0f;
    public float damagePerSecond = 2.0f;
    private Transform target;

  
    private float damageTimer = 1.0f;
    private float currentDamageTime = 0.0f;

    public delegate void FlameEffectEndAction();
    public event FlameEffectEndAction OnFlameEffectEnd;

    private void Update()
    {
        if (target == null || target.gameObject == null)
        {

            TriggerFlameEffectEnd();
            return;
        }

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);

        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

      
        currentDamageTime += Time.deltaTime;
        if (currentDamageTime >= damageTimer)
        {

            DealDamage();
            currentDamageTime = 0.0f; 
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void DealDamage()
    {
        Enemy enemyScript = target.GetComponent<Enemy>();
        if (enemyScript != null)
        {

            enemyScript.setBurning();
        }
    }


    private void TriggerFlameEffectEnd()
    {
        OnFlameEffectEnd?.Invoke();
        Destroy(gameObject); 
    }
}
