using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuffTower : MonoBehaviour
{
    public float buffRange = 2.0f;
    public float damageBuff = 2.0f;
    public float rateOfFireBuff = 0.2f;

    public float health = 2;

    private void Update()
    {
        ITower[] towers = FindTowersInRange();

        foreach (ITower tower in towers)
        {
            tower.ApplyBuff(damageBuff, rateOfFireBuff);
        }
    }
    private ITower[] FindTowersInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, buffRange);

        return colliders
            .Select(collider => collider.GetComponent<ITower>())
            .Where(tower => tower != null)
            .ToArray();
    }
}