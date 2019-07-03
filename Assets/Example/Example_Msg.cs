using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Msg : MonoBehaviour
{
    void Awake()
    {
		//注册
        MsgManager.Instance.RegisterMsg(this, MsgID.Test, (txt) => Debug.Log(txt));
    }
    void Start()
    {
		//发送消息
        MsgManager.Instance.SendMsg(MsgID.Test, "hello");
    }
}
