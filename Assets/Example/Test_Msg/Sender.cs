using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Threading;

public class Sender : MonoBehaviour
{
    Thread threadA;
    Thread threadB;
    // Use this for initialization
    void Start()
    {
		//检测是否线程安全
        threadA = new Thread(ThreaA);
        threadA.Start();

        threadB = new Thread(ThreaB);
        threadB.Start();
    }

    void ThreaA()
    {
        while (true)
        {
            Thread.Sleep(1000);
            MsgManager.Instance.SendMsg(MsgID.Test, "threadA");
        }
    }

    void ThreaB()
    {
        while (true)
        {
            Thread.Sleep(1000);
            MsgManager.Instance.SendMsg(MsgID.Test, "threadB");
        }
    }

	private void OnDestroy() {
		threadA.Abort();
		threadB.Abort();
	}
}
