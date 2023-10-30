using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTower : MonoBehaviour
{
    public float attackRange = 5.0f; 

    private void Update()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();

                if (enemy != null)
                {
                  
                    enemy.ApplyFreeze();
                }
            }
        }
    }
}