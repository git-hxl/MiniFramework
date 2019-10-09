using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;

public class FrameServer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        MiniTcpServer.Instance.Launch(8888);

        MsgDispatcher.Instance.Regist(this, MsgID.HeartPack, (data) =>
        {
            MiniTcpServer.Instance.Send(MsgID.HeartPack, data);
        });

        MsgDispatcher.Instance.Regist(this, MsgID.Test, (data) =>
        {
            MiniTcpServer.Instance.Send(MsgID.Test, data);
        });
    }

}
