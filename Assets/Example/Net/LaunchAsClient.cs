using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;
using System;

public class LaunchAsClient : MonoBehaviour
{
    float startTime;
    // Use this for initialization
    void Start()
    {
        SocketManager.Instance.ConnectSuccess+=ConnectSuccess;
        SocketManager.Instance.LaunchAsClient("192.168.89.37", 8888);
        

        MsgManager.Instance.RegisterMsg(this,"Connect",(data)=>{
            ResourceManager.Instance.AssetLoader.LoadAsset("Player",(obj)=>{
           GameObject player = Instantiate(obj) as GameObject;
           player.GetComponent<MeshRenderer>().material.color = Color.blue;
        });
        });
    }
    void ConnectSuccess(){
        MsgManager.Instance.SendMsg("Connect",null);
    }
   
}
