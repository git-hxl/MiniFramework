using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;
using UnityEngine.UI;

public class FrameServer : MonoBehaviour
{
    public Button Launch;
    public Button Close;
    public Button Clear;
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
        Launch.onClick.AddListener(() =>
        {
            MiniTcpServer.Instance.Launch(8888);
        });
        Close.onClick.AddListener(() =>
        {
            MiniTcpServer.Instance.Close(false);
        });
        Clear.onClick.AddListener(() =>
        {
            MiniTcpServer.Instance.Clear();
        });
    }

}
