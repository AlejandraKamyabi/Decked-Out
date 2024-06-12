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


    // note for me


    /*  I gotta go over this section since the tower usually scoops up when the detection on whether 
        or not the tower has been destroyed, it sets it to null if it dosent which happens
        when there no more detections used for this. setting it directly from the mousinputhandling script
        might work since the script has collisionOccurred with it and it would rquire me to check every 
        frame.This detection can sometimes require trying to make it a difference between each tower since the
        collision might sometimes cause issues when there are multiple towers placed, basiccally overlapping 
        in a single frame which can cause issues.
        delaying this proccess might be able to fix this issues so i will start with that and 
        connect to other issues. 
     */

    private IEnumerator changeTag(float delay)
{
    yield return new WaitForSeconds(delay);
        gameObject.tag = "Empty";
}

private void OnTriggerEnter2D(Collider2D other)
    {
        input.setIsland(false);
        if (gameObject.CompareTag("Buffer") || gameObject.CompareTag("Placed") || gameObject.CompareTag("Spell"))
        {
            return;
        }
        if (other.CompareTag("Empty"))
        {
         
            Destroy(other.gameObject);
        }
        StartCoroutine(changeTag(0.4f));
        if (!mouse.collisionOccurred && !other.CompareTag("Empty") && !other.CompareTag("Placed") && !other.CompareTag("Tower") && !other.CompareTag("Spell") && other.CompareTag("Platform"))
        {
            mouse.SetCollision(false);
            
            hasCollided = true;
            transform.position = platformTransform.position + new Vector3(0, 0.9f, 0);
            input.setIsland(false);
            gameObject.tag = "Temp";
        }

    }

    private void Update()
    {

        if (platformTransform == null)
        {
            input.setIsland(true);
        }


        if (hasCollided && platformTransform != null)
        {
            transform.position = platformTransform.position + new Vector3(0, 0.9f, 0);
        }


    }
}