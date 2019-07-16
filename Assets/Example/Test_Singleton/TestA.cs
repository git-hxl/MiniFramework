using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class TestA : MonoSingleton<TestA> {
	protected override void Awake(){
		base.Awake();
		Debug.Log("Awake");
	}
	// Use this for initialization
	void Start () {
		
	}
	
	
	void Update () {
		
	}
	public override void Dispose() {
		base.Dispose();
		Debug.Log("Dispose");
	}
}
