using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
public class Example_Msg_Recv : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        DelegateDispatcher.Instance.Regist("001", () => { Debug.Log(001); });

        MsgDispatcher.Instance.Regist(this, 002, (data) => { Debug.Log(002); });
    }

}
