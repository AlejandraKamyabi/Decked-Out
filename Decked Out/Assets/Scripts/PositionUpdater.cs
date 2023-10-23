using UnityEngine;

public class PositionUpdater : MonoBehaviour
{
    private Transform platformTransform;
    private Vector3 offset;
    private ArcherTower Mouse;
    private bool hasCollided = false;

    private void Start()
    {
        platformTransform = GameObject.FindGameObjectWithTag("Platform").transform;
        offset = transform.position - platformTransform.position;
        Mouse = GetComponent<ArcherTower>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasCollided && other.CompareTag("Platform"))
        {
            hasCollided = true;
            transform.position = platformTransform.position + new Vector3(0, 1.0f, 0);
            Mouse.setbool();
        }
    }

    private void Update()
    {
        if (hasCollided && platformTransform != null)
        {
            transform.position = platformTransform.position + new Vector3(0, 1.0f, 0);
        }
    }
}