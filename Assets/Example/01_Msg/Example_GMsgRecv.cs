using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using UnityEngine.UI;

public class Example_GMsgRecv : MonoBehaviour
{
    // Use this for initialization
    void Awake()
    {
        GameMsgManager.Instance.Regist<string>(GameMsgID.Test, OnRecv);
        GameMsgManager.Instance.Regist<int>(GameMsgID.Test, OnRecv);
    }

    void OnRecv(string msg)
    {
        Debug.Log(this.name + ":" + msg);
    }
    void OnRecv(int msg)
    {
        Debug.Log(this.name + ":" + msg);
    }
    void OnDestroy()
    {
        //GameMsgManager.Instance.UnRegist<string>(GameMsgID.Test, OnRecv);
    }
}