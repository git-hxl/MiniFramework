using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_ResLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Instantiate(ResourceManager.Instance.Load<GameObject>("Assets/Prefabs/Cube.prefab"));
	}
    // Update is called once per frame
    void Update () {
		
	}
}
