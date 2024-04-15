// =============================================================================
// 
// For placing towers on the island. updates the position.
// 
//            
// 
// =============================================================================
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PositionUpdater : MonoBehaviour
{
    public Transform platformTransform;
    private Vector3 offset;
    private bool hasCollided = false;
    private WaveManager mouse;
    private MouseInputHandling input;
    private GameLoader _loader;
    private bool stop = false;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {
        Debug.Log("PostionUpdater.Initialize");
        platformTransform = GameObject.FindGameObjectWithTag("Platform").transform;
        offset = transform.position - platformTransform.position;
        mouse = ServiceLocator.Get<WaveManager>();
        input = ServiceLocator.Get<MouseInputHandling>();

        if (mouse is null)
        {
            Debug.LogWarning("Position Updated Failed to find the MouseInputHandling");
        }
    }

private IEnumerator changeTag(float delay)
{
    yield return new WaitForSeconds(delay);
        gameObject.tag = "Empty";
}

private void OnTriggerEnter2D(Collider2D other)
    {
    
        if (gameObject.CompareTag("Buffer") || gameObject.CompareTag("Placed") || gameObject.CompareTag("Spell"))
        {
            return;
        }
        if (other.CompareTag("Empty"))
        {
            input.setIsland(false);
            Destroy(other.gameObject);
        }
        StartCoroutine(changeTag(0.4f));
        if (!mouse.collisionOccurred && !other.CompareTag("Empty") && !other.CompareTag("Placed") && !other.CompareTag("Tower") && !other.CompareTag("Spell") && other.CompareTag("Platform"))
        {
            mouse.setCollision(false);
            hasCollided = true;
            transform.position = platformTransform.position + new Vector3(0, 0.9f, 0);

            gameObject.tag = "Temp";
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