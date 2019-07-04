using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestB : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//场景中不存在TestA时 会自动创建 并执行OnSingletonInit
		TestA.Instance.Dispose();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
