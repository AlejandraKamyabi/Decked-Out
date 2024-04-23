using UnityEngine;

public class SmallBullet : MonoBehaviour
{
    public float SmallBulletSpeed = 30.0f;
    private float damage;
    private Vector2 moveDirection;
    private Transform target;
    public GameObject effect;
    [SerializeField] private float aoeRadius = 3.0f;
    private void Update()
    {
        if (target == null || target.gameObject == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, SmallBulletSpeed * Time.deltaTime);
        transform.Translate(moveDirection * SmallBulletSpeed * Time.deltaTime, Space.World);
        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            Vector3 explosionPosition = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
            GameObject Explosion = Instantiate(effect, explosionPosition, Quaternion.identity);
            DealDamage(transform.position);
            Destroy(Explosion, 0.3f);
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

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void DealDamage(Vector2 explosionPoint)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(explosionPoint, aoeRadius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemyScript = hitCollider.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
            KaboomEnemy kaboom = hitCollider.GetComponent<KaboomEnemy>();
            if (kaboom != null)
            {
                kaboom.TakeDamage(damage);
            }
            Apostate apostate = hitCollider.GetComponent<Apostate>();
            if (apostate != null)
            {
                apostate.TakeDamage(damage);
            }
            Necromancer necromancer = hitCollider.GetComponent<Necromancer>();
            if (necromancer != null)
            {
                necromancer.TakeDamage(damage);
            }
            Cleric cleric = hitCollider.GetComponent<Cleric>();
            if (cleric != null)
            {
                cleric.TakeDamage(damage);
            }
            Aegis aegis = hitCollider.GetComponent<Aegis>();
            if (aegis != null)
            {
                aegis.TakeDamage(damage);

            }
        }
    }
}
