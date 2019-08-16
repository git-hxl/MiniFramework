using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MiniFramework;
using System;

public class TestServer : MonoBehaviour
{
    public InputField Content;
    public Button Send;
    public Text Text;
    public Button Close;
    // Use this for initialization
    void Start()
    {
        MiniTcpServer.Instance.Launch(8888);
        //MiniUdpClient.Instance.Launch(8888);
        Send.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(Content.text))
            {
                MiniTcpServer.Instance.Send(MsgID.Test, null);
            }
            else
            {
                MiniTcpServer.Instance.Send(MsgID.Test, SerializeUtil.ToProtoBuff(Content.text));
            }
            Text.text = DateTime.Now + ":" + Content.text;
            Text.color = Color.black;
            Instantiate(Text.gameObject, Text.transform.parent).SetActive(true);
        });
        Close.onClick.AddListener(() =>
        {
            MiniTcpServer.Instance.Clear();
        });
        MsgManager.Instance.RegisterMsg(this, MsgID.Test, (s) =>
        {
            string txt = SerializeUtil.FromProtoBuff<string>((byte[])s);
            Text.text = DateTime.Now + ":" + txt;
            Text.color = Color.blue;
            Instantiate(Text.gameObject, Text.transform.parent).SetActive(true);
            Debug.Log(txt);
        });
    }
}
