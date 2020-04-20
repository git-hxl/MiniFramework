using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Network;
using UnityEngine.UI;
using System;
using MiniFramework;
using System.Text;

public class TestTcpClient : MonoBehaviour
{
    public string Address;
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
        SocketManager.Instance.GetTcpClient.ConnectFailed += GetTcpClient_ConnectFailed;
        SocketManager.Instance.GetTcpClient.ConnectSuccess += GetTcpClient_ConnectSuccess;
        SocketManager.Instance.GetTcpClient.ConnectAbort += GetTcpClient_ConnectAbort;
        SocketManager.Instance.GetTcpClient.Init(Address, Port);


        MsgManager.Instance.Regist(MsgID.Test, GetMsg);

        buttonConnect.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetTcpClient.Connect();
        });

        buttonClose.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetTcpClient.Close();
        });

        buttonSend.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetTcpClient.Send(MsgID.Test, Encoding.UTF8.GetBytes(inputFieldMsg.text));
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


    void UpdateMsg(string txt)
    {
        Text msgCopy = Instantiate(msg.gameObject, msg.transform.parent).GetComponent<Text>();
        msgCopy.text = DateTime.Now + ":" + txt;
        msgCopy.gameObject.SetActive(true);
        scrollRect.verticalScrollbar.value =0;
    }
}
