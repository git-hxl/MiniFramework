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
            //Debug.Log(data);
        });
        MsgDispatcher.Instance.Regist(this, 100, (data) =>
        {
            //Debug.Log(data);
        });

        MsgDispatcher.Instance.Dispatch(100,null);
    }

}
