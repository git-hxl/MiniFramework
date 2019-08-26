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
        MsgDispatcher.Instance.Regist(this, 100, (data) =>
        {
            Debug.Log(data[0]);
        });
        MsgDispatcher.Instance.Regist(this, 100, (data) =>
        {
            Debug.Log(data[0]);
        });

        MsgDispatcher.Instance.Dispatch(100, "hello world");
    }

}
