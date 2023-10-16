using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed = 10.0f; 

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
            DealDamage();
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void DealDamage()
    {
       
    }
}