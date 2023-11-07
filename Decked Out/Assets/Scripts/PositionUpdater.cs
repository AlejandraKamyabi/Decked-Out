using UnityEngine;

public class PositionUpdater : MonoBehaviour
{
    private Transform platformTransform;
    private Vector3 offset;
    private bool hasCollided = false;
    private WaveManager mouse;
    private GameLoader _loader;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log("PostionUpdater.Initialize");
        platformTransform = GameObject.FindGameObjectWithTag("Platform").transform;
        offset = transform.position - platformTransform.position;
        mouse = ServiceLocator.Get<WaveManager>();

        if(mouse is null)
        {
            Debug.LogWarning("Position Updated Failed to find the MouseInputHandling");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("Buffer"))
        {
            return;
        }
        gameObject.tag = "Empty";
        if (!mouse.collisionOccurred && other.CompareTag("Platform"))
        {
            mouse.setCollision();
            hasCollided = true;
            transform.position = platformTransform.position + new Vector3(0, 0.9f, 0);
          

        }
     
    }

    private void Update()
    {
        if (hasCollided && platformTransform != null)
        {
            transform.position = platformTransform.position + new Vector3(0, 0.9f, 0);
        }
    }
}