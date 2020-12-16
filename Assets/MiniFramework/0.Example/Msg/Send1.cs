using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MiniFramework;
using UnityEngine;

public class Send1 : MonoBehaviour
{
    Thread thread;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        thread = new Thread(() =>
        {
            while (true)
            {
                Thread.Sleep(10);
                index++;
                MsgManager.Instance.Enqueue(1111, index);
            }
        });
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Send"))
        {
            EventManager.Instance.Dispatch(1111, "xxxxx");
        }

        if (GUILayout.Button("Auto Send"))
        {
            thread.Start();
        }
    }
    private void OnDestroy() {
        thread.Abort();
    }
}
