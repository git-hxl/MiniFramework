using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;
using System;

public class LaunchAsServer : MonoBehaviour
{
   // public Dictionary<string, PlayerController> players = new Dictionary<string, PlayerController>();
    float startTime;
    // Use this for initialization
    void Start()
    {
        SocketManager.Instance.ClientConnected += ClientConnect;
        SocketManager.Instance.ClientAborted += ClientAbort;
        SocketManager.Instance.LaunchAsServer(8888);

        MsgManager.Instance.RegisterMsg(this, "ClientConnect", (data) =>
        {
            ResourceManager.Instance.AssetLoader.LoadAsset("Player", (obj) =>
            {
               // string id = (string)data;
                GameObject player = Instantiate(obj) as GameObject;
                player.GetComponent<MeshRenderer>().material.color = Color.red;
              //  PlayerController pController = player.GetComponent<PlayerController>();
               // pController.IsSelf = false;
              //  pController.Port = id;
              //  players.Add(id, pController);
            });
        });
        MsgManager.Instance.RegisterMsg(this, "Abort", (data) =>
        {
           // string id = (string)data;
           // Destroy(players[id].gameObject);
          //  players.Remove(id);
        });
    }
    void ClientConnect(string id)
    {
        MsgManager.Instance.SendMsg("ClientConnect", id);
    }
    void ClientAbort(string id)
    {
        MsgManager.Instance.SendMsg("Abort", id);
    }

}
