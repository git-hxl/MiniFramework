using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using MiniFramework;
public class NewBehaviourScript2 : MonoBehaviour
{

    // Use this for initialization
    int i;
    Thread thread1;
    Thread thread2;
    private static readonly object locker = new object();
    void Start()
    {
        thread1 = new Thread(Test1);
        thread1.Start();

        thread2 = new Thread(Test2);
        thread2.Start();

        
    }


    void Test1()
    {
        while (i < 10)
        {
            lock (locker)
            {
                Thread.Sleep(1000);
                i++;
                Debug.Log("Test1:" + i);
            }

        }
    }

    void Test2()
    {
        while (true)
        {
            lock (locker)
            {
                Thread.Sleep(1000);
                i++;
                Debug.Log("Test2:" + i);
            }
        }
    }
    private void OnDestroy()
    {
        thread1.Abort();
        thread2.Abort();
    }
}
