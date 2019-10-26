using MiniFramework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Example_NetMsgSend : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NetMsgDispatcher.Instance.Dispatch(NetMsgID.Test, Encoding.UTF8.GetBytes("Hello"));
	}
}