using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float time;
    public float attackRange = 2f;
    private float damage = 999;
    private Vector3 size;
    private Vector3 zero = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        size = gameObject.transform.localScale;
        size /= time * 30;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale -= size;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        if (gameObject.transform.localScale.x <= 0 && gameObject.transform.localScale.y <= 0 && gameObject.transform.localScale.z <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            print("hit");
            enemyScript.TakeDamage(damage);
        }
        KaboomEnemy kaboom = collision.gameObject.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.TakeDamage(damage);
        }
        Apostate apostate = collision.gameObject.GetComponent<Apostate>();
        if (apostate != null)
        {
            apostate.TakeDamage(damage);

        }
        Necromancer necromancer = collision.gameObject.GetComponent<Necromancer>();
        if (necromancer != null)
        {
            necromancer.TakeDamage(damage);
        }
        Cleric cleric = collision.gameObject.GetComponent<Cleric>();
        if (cleric != null)
        {
            cleric.TakeDamage(damage);
        }
    }
}
