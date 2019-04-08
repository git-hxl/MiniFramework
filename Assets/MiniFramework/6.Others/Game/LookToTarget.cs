using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToTarget : MonoBehaviour
{
    public Transform Target;
    // Update is called once per frame
    void Update()
    {
        if (Target == null)
        {
            return;
        }
        transform.forward = Target.forward;
    }
}
