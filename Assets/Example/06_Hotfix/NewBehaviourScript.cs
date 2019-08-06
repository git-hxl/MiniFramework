using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 10 == 0)
            Debug.Log(Time.time);
        if (Time.frameCount % 15 == 0)
            Debug.LogWarning(Time.time);
        if (Time.frameCount % 25 == 0)
            Debug.LogError(Time.time);
    }
}
