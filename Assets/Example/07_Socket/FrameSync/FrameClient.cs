using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;

public class FrameClient : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SocketManager.Instance.MiniTcpClient.Launch("127.0.0.1", 8888);
        TimeoutChecker.Instance.CheckConnectTimeout(SocketManager.Instance.MiniTcpClient);
        TimeoutChecker.Instance.CheckHeartPack(SocketManager.Instance.MiniTcpClient);

        TimeoutChecker.Instance.AutoSendPing("www.baidu.com");
    }
}
