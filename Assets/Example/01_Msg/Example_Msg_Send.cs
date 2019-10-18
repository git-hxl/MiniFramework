using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Msg_Send : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DelegateDispatcher.Instance.Dispatch("001");
		MsgDispatcher.Instance.Dispatch(002,null);
	}
}
