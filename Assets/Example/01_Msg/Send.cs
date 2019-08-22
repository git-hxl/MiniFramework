using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Send : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MsgDispatcher.Instance.Send(1003);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
