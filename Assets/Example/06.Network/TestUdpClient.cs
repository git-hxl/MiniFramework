using MiniFramework;
using MiniFramework.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TestUdpClient : MonoBehaviour
{
    public string Address;
    public int Port;
    public Text msg;
    public Button buttonLaunch;
    public Button buttonClose;
    public Button buttonSend;
    public Button buttonBroadcast;
    public InputField inputFieldMsg;

    public ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        SocketManager.Instance.GetUdpClient.SetIPEndPoint(Address, Port);


        MsgManager.Instance.Regist(MsgID.Test, GetMsg);

        buttonLaunch.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetUdpClient.Launch();
        });

        buttonClose.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetUdpClient.Close();
        });

        buttonSend.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetUdpClient.Send(MsgID.Test, Encoding.UTF8.GetBytes(inputFieldMsg.text));
        });

        buttonBroadcast.onClick.AddListener(() =>
        {
            SocketManager.Instance.GetUdpClient.Broadcast(MsgID.Test, Encoding.UTF8.GetBytes(inputFieldMsg.text));
        });

        Application.runInBackground = true;
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
        scrollRect.verticalScrollbar.value = 0;
    }
}
