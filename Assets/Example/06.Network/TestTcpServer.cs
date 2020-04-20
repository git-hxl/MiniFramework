using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniFramework;
using MiniFramework.Network;
using System.Text;
using System;

public class TestTcpServer : MonoBehaviour
{
    public int Port;
    public Text msg;
    public Button buttonConnect;
    public Button buttonClose;
    public Button buttonSend;
    public InputField inputFieldMsg;

    public ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
 
        SocketManager.Instance.GetTcpServer.Init(Port);


        MsgManager.Instance.Regist(MsgID.Test, GetMsg);
        MsgManager.Instance.Regist(MsgID.HeartPack, GetHeartPack);

        buttonConnect.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetTcpServer.Launch();
        });

        buttonClose.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetTcpServer.Close();
        });

        buttonSend.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetTcpServer.Send(MsgID.Test, Encoding.UTF8.GetBytes(inputFieldMsg.text));
        });
        Application.runInBackground = true;
    }

    private void GetTcpClient_ConnectSuccess()
    {
        UpdateMsg("连接成功");
    }

    private void GetTcpClient_ConnectAbort()
    {
        UpdateMsg("断开连接");
    }

    private void GetTcpClient_ConnectFailed()
    {
        UpdateMsg("连接失败");
    }

    private void GetMsg(byte[] data)
    {
        UpdateMsg(Encoding.UTF8.GetString(data));
    }
    private void GetHeartPack(byte[] data)
    {
        SocketManager.Instance.GetTcpServer.Send(MsgID.HeartPack, null);
    }

    void UpdateMsg(string txt)
    {
        Text msgCopy = Instantiate(msg.gameObject, msg.transform.parent).GetComponent<Text>();
        msgCopy.text = DateTime.Now + ":" + txt;
        msgCopy.gameObject.SetActive(true);
        scrollRect.verticalScrollbar.value = 0;
    }
}
