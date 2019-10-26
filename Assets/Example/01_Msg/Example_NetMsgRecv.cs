using MiniFramework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Example_NetMsgRecv : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        NetMsgDispatcher.Instance.Regist(NetMsgID.Test, OnRecv);
        NetMsgDispatcher.Instance.Regist(NetMsgID.Test, OnRecv2);
    }
	
    void OnRecv(byte[] data)
    {
        Debug.Log(this.name+":"+Encoding.UTF8.GetString(data));
    }
    void OnRecv2(byte[] data)
    {
        Debug.Log(this.name + ":" + Encoding.UTF8.GetString(data));
    }

    void OnDestroy()
    {
        NetMsgDispatcher.Instance.UnRegist(NetMsgID.Test, OnRecv);
    }
}