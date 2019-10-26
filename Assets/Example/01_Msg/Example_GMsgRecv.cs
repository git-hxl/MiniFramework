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
        GameMsgDispatcher.Instance.Regist<string>(GameMsgID.Test, OnRecv);
    }

    void OnRecv(string msg)
    {
        Debug.Log(this.name + ":" + msg);
    }

    void OnDestroy()
    {
        GameMsgDispatcher.Instance.UnRegist<string>(GameMsgID.Test, OnRecv);
    }
}