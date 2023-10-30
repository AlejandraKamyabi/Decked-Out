using UnityEngine;

public class FlamethrowerTower : MonoBehaviour
{
    public float attackRange = 5.0f;
    public GameObject flamePrefab;
    private Transform currentTarget;
    private float timeBetweenFlames = 3.0f;
    private float lastFlameTime = 0.0f;
    private bool isShooting = false; 

    private void Update()
    {
        if (!isShooting) 
        {
            if (currentTarget == null)
            {
                FindNewTarget();
            }
            else
            {
                if (Time.time - lastFlameTime >= timeBetweenFlames)
                {
                    ShootFlame();
                }
            }
        }
    }

    private void FindNewTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                if (!collider.GetComponent<Enemy>().isBurning) 
                {
                    currentTarget = collider.transform;
                    break;
                }
            }
        }
    }

    private void ShootFlame()
    {
        if (currentTarget != null)
        {
            isShooting = true; 

            GameObject flame = Instantiate(flamePrefab, transform.position, Quaternion.identity);
            FlamesEffect flamesEffect = flame.GetComponent<FlamesEffect>();
            flamesEffect.SetTarget(currentTarget);


            flamesEffect.OnFlameEffectEnd += HandleFlameEffectEnd;

            lastFlameTime = Time.time;
        }
    }

    private void HandleFlameEffectEnd()
    {
        isShooting = false;
    }
}
