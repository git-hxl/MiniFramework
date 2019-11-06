using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Load : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Instantiate(ResourceManager.Instance.Load<GameObject>("Prefab/Prefab/Cube"));
    }
}
