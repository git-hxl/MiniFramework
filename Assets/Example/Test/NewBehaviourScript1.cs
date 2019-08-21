using System.Collections;
using System.Collections.Generic;
using System.Text;
using MiniFramework;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        NetMsgManager.Instance.RegisterMsg(this, 100, (data) =>
        {
            Debug.Log(Encoding.UTF8.GetString(data) + "xxxxxx");
        });
        NetMsgManager.Instance.RegisterMsg(this, 100, (data) =>
        {
            Debug.Log(Encoding.UTF8.GetString(data) + "xxxxxx");
        });
        //NetMsgManager.Instance.UnRegisterMsg(100);
        //NetMsgManager.Instance.UnRegisterMsg(this,100);


        NetMsgManager.Instance.SendMsg(100, Encoding.UTF8.GetBytes("hello world"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
