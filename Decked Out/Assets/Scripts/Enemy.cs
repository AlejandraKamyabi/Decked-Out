using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float moveSpeed = 2.0f; 
    public Transform targetTower; 

    private void Update()
    {
        if (targetTower != null)
        {
  
            Vector3 moveDirection = (targetTower.position - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}
