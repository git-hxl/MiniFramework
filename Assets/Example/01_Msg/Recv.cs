using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Recv : MonoBehaviour {

	// Use this for initialization
	void Start () {
		 MsgDispatcher.Instance.Regist(this,1003,Test);
	}
	
	void Test(object[] data)
	{
		Debug.Log("ok");
	}
}
