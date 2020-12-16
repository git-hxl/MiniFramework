using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;

public class SocketExample : MonoBehaviour
{
    public string ip;
    public int port;

    public bool isAsServer;

    TcpClient tcpClient;
    TcpServer tcpServer;
    // Start is called before the first frame update
    void Start()
    {
        tcpClient = new TcpClient(ip, port);
        tcpServer = new TcpServer(port);
        if (isAsServer)
            tcpServer.Launch();

        else
            tcpClient.Connect();

        EventManager.Instance.Regist(1, (data) =>
        {
            Debug.Log("recv:" + Encoding.UTF8.GetString((byte[])data));
        });
    }


    private void OnGUI()
    {
        if (GUILayout.Button("Launch"))
        {
            if (isAsServer)
                tcpServer.Launch();
            else
                tcpClient.Connect();
        }

        if (GUILayout.Button("Close"))
        {
            if (isAsServer)
                tcpServer.Close();
            else
                tcpClient.Close();
        }

        if (GUILayout.Button("Send"))
        {
            if (isAsServer)
                tcpServer.Send(1, Encoding.UTF8.GetBytes("周敏是傻逼"));
            else
                tcpClient.Send(1, Encoding.UTF8.GetBytes("周敏是傻逼"));
        }
    }


}
