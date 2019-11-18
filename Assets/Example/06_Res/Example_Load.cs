using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Load : MonoBehaviour
{
    public Object ResObject;
    public Object AssetObject;
    // Use this for initialization
    void Start()
    {
        ResObject = ResourceManager.Instance.Load<Sprite>("Background");
        AssetObject = ResourceManager.Instance.Load<Texture2D>("Background");
    }
}
