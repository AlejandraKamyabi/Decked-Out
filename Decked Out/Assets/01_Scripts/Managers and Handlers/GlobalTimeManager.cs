using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimeManager : MonoBehaviour
{
    public static GlobalTimeManager Instance { get; private set; }
    public float GlobalDeltaTIme { get; private set; }


    float _lastFrameTime;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GlobalDeltaTIme = Time.time - _lastFrameTime;
        _lastFrameTime = Time.time;
    }
}
