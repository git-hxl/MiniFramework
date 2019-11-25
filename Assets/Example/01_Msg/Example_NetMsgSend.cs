using MiniFramework;
using System.Text;
using System.Threading;
using UnityEngine;

public class Example_NetMsgSend : MonoBehaviour {
    Thread thread;
    Thread thread2;
    // Use this for initialization
    void Start () {
        Application.runInBackground = true;
        int i = 0;
        thread = new Thread(()=>
        {
            while (i<100)
            {
                i++;
                NetMsgManager.Instance.Dispatch(NetMsgID.Test, Encoding.UTF8.GetBytes(i + ":Hello"));
                Thread.Sleep(1000);
            }         
        } );
        thread.IsBackground = true;
        thread.Start();
        int j = 0;
        thread2 = new Thread(() =>
        {
            while (j < 100)
            {
                j++;
                NetMsgManager.Instance.Dispatch(NetMsgID.Test, Encoding.UTF8.GetBytes(j + ":World"));
                Thread.Sleep(1000);
            }
        });
        thread2.IsBackground = true;
        thread2.Start();
    }

    private void OnDestroy()
    {
        thread.Abort();
        thread2.Abort();
    }
}