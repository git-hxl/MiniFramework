using MiniFramework;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SimpleChat : MonoBehaviour {
    public Text Msg;
    public InputField InputField;
    public Button ButtonSend;
	// Use this for initialization
	void Start () {
        NetMsgDispatcher.Instance.Regist(NetMsgID.Test, (data) =>
        {
            string msg = Encoding.UTF8.GetString(data);
            Text txtMsg = Instantiate(Msg.gameObject, Msg.transform.parent).GetComponent<Text>();
            txtMsg.gameObject.SetActive(true);
            txtMsg.text = DateTime.Now.ToShortTimeString() + ":" + msg;
            txtMsg.alignment = TextAnchor.MiddleRight;
            txtMsg.color = Color.blue;
        });

        ButtonSend.onClick.AddListener(() =>
        {
            string msg = InputField.text;
            Text txtMsg = Instantiate(Msg.gameObject, Msg.transform.parent).GetComponent<Text>();
            txtMsg.gameObject.SetActive(true);
            txtMsg.text = DateTime.Now.ToShortTimeString() + ":" + msg;
            UdpClientComponent.Instance.Send(NetMsgID.Test, Encoding.UTF8.GetBytes(msg));
        });
	}
}