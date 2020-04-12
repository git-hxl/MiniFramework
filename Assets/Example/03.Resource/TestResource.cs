using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Resource;
using MiniFramework.Config;
public class TestResource : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResourceManager.Instance.readSuccessCallback += () =>
        {
            GameObject asset = ResourceManager.Instance.LoadAsset<GameObject>("UILogin");
            Instantiate(asset,transform);
        };
    }
}
