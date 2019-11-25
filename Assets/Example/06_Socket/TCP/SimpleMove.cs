using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class SimpleMove : MonoBehaviour {
    public float Speed;
    bool isCanMove;
	// Use this for initialization
	void Start () {
        NetMsgManager.Instance.Regist(NetMsgID.Test, (data) =>
        {
            string s =  SerializeUtil.FromProtoBuff<string>(data);
            string x = s.Split(':')[0];
            string z = s.Split(':')[1];
            Vector3 dir = new Vector3(float.Parse(x),0,float.Parse(z));
            transform.Translate(dir * Speed * Time.deltaTime);
        });
	}
	
	// Update is called once per frame
	void Update () {
		if(isCanMove)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            if(x!=0||z!=0)
            {
                string s = x + ":" + z;
                byte[] data = SerializeUtil.ToProtoBuff(s);
                TcpClientComponent.Instance.Send(NetMsgID.Test, data);
            }
        }
	}

    public void ConnectSuccess()
    {
        Debug.Log("连接成功");
        isCanMove = true;

        
    }
    public void ConnectAbort()
    {
        Debug.Log("网络中断");
        isCanMove = false;
    }
}
