using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float Distance = 10;
    public float Height = 0;
    public float Angle = 45;
    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }
        
        transform.rotation = Quaternion.Euler(Angle, 0, 0);
		transform.position = Target.position + transform.forward * -Distance + transform.up * Height;
    }
}
