using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Load : MonoBehaviour
{
    public Object Asset;
    // Use this for initialization
    void Start()
    {

    }

    public void Load()
    {
        Asset = (ResourceManager.Instance.Load<Sprite>("Texture/商城数字"));
    }
}
