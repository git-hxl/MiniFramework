using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_GMsgSend : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameMsgDispatcher.Instance.Dispatch<string>(GameMsgID.Test, "hello");
	}
	
}
