using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Update:"+Time.deltaTime);
	}
	void LateUpdate(){
		Debug.Log("LateUpdate:"+Time.deltaTime);
	}
	void FixedUpdate(){
		Debug.Log("FixedUpdate:"+Time.fixedDeltaTime);
	}
}
