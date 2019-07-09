using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Reciver : MonoBehaviour
{
    private void Awake()
    {
        MsgManager.Instance.RegisterMsg(this, MsgID.Test, reciverCallback);
    }
    void reciverCallback(object o)
    {
        Debug.Log(o);
    }
}
