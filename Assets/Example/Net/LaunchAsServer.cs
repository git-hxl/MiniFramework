using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;
using System;

public class LaunchAsServer : MonoBehaviour
{
    float startTime;
    // Use this for initialization
    void Start()
    {
        MsgManager.Instance.RegisterMsg(this, "1", Recv);
        SocketManager.Instance.LaunchAsServer(1122);
    }
    void Recv(object data)
    {
        byte[] bytes = (byte[])data;
        Debug.Log(Encoding.UTF8.GetString(bytes));
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime < 0.2f)
        {
            return;
        }
        byte[] bytes = Encoding.UTF8.GetBytes("I am Server");
        PackHead head = new PackHead();
        head.MsgID = 1;
        head.TimeStamp = DateTime.Now.Second;
        head.PackLength = (short)bytes.Length;
        SocketManager.Instance.SendToClient(head, bytes);
		startTime = Time.time;
    }
}
