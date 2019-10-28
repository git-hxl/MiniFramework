using MiniFramework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Example_NetMsgRecv : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        NetMsgDispatcher.Instance.Regist(NetMsgID.Test, OnRecv);
    }
	
    void OnRecv(byte[] data)
    {
        Debug.Log(gameObject.name+":"+Encoding.UTF8.GetString(data));
    }
    void OnDestroy()
    {
        NetMsgDispatcher.Instance.UnRegist(NetMsgID.Test, OnRecv);
    }
}