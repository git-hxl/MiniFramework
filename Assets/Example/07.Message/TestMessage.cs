using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;
using System;
using System.Threading;

public class TestMessage : MonoBehaviour
{
    Thread thread;
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        MsgManager.Instance.Regist(1000, getThreadMsg);

        if (thread == null)
        {
            thread = new Thread(() =>
            {
                Thread.Sleep(1000);
                while (true)
                {
                    MsgManager.Instance.Dispatch(1000, BitConverter.GetBytes(TimeUtil.GetTimeStamp(false)));
                    Thread.Sleep(20);
                    //curTime += 0.02f;
                }
            });
            thread.Start();
        }
        EventManager.Instance.Regist("Test", getEventMsg);
        EventManager.Instance.Dispatch("Test");
    }

    void getThreadMsg(byte[] data)
    {
        Debug.Log("消息延迟:" + (TimeUtil.GetTimeStamp(false) - BitConverter.ToInt32(data,0)));
    }
    void getEventMsg()
    {
         
    }
    private void OnDestroy()
    {
        thread.Abort();
    }
}
