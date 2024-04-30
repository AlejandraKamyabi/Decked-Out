using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomZealots : MonoBehaviour
{
    public float detectionRadius = 2f;
    public float speedUpPrecentage = 1.8f;

    private void Start()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = detectionRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyScript = collision.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.ApplySpeedUp(speedUpPrecentage);
        }
        KaboomEnemy kaboom = collision.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.ApplySpeedUp(speedUpPrecentage);
        }
        Apostate apostate = collision.GetComponent<Apostate>();
        if (apostate != null)
        {
            apostate.ApplySpeedUp(speedUpPrecentage);

        }
        Mopey_Misters _mopey = collision.GetComponent<Mopey_Misters>();
        if (_mopey != null)
        {
            _mopey.ApplySpeedUp(speedUpPrecentage);

        }
        Necromancer necromancer = collision.GetComponent<Necromancer>();
        if (necromancer != null)
        {
            necromancer.ApplySpeedUp(speedUpPrecentage);
        }
        Cleric cleric = collision.GetComponent<Cleric>();
        if (cleric != null)
        {
            cleric.ApplySpeedUp(speedUpPrecentage);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemyScript = collision.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.RemoveSpeedUp();
        }
        KaboomEnemy kaboom = collision.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.RemoveSpeedUp();
        }
        Apostate apostate = collision.GetComponent<Apostate>();
        if (apostate != null)
        {
            apostate.RemoveSpeedUp();

        }
        Necromancer necromancer = collision.GetComponent<Necromancer>();
        if (necromancer != null)
        {
            necromancer.RemoveSpeedUp();
        }
        Cleric cleric = collision.GetComponent<Cleric>();
        if (cleric != null)
        {
            cleric.RemoveSpeedUp();
        }
        Mopey_Misters _mopey = collision.GetComponent<Mopey_Misters>();
        if (_mopey != null)
        {
            _mopey.RemoveSpeedUp();
        }
    }
}
