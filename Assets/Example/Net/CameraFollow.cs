using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    public float Distance;
    public float Height;
    // Use this for initialization
    void Start() { }
    void LateUpdate()
    {
        if (Player != null)
        {
            transform.position = Player.position + new Vector3(0, Height, Distance);
            transform.LookAt(Player);
        }
    }
}
