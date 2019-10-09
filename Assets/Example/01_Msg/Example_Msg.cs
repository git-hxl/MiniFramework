using UnityEngine;
using MiniFramework;
using System.Text;

public class Example_Msg : MonoBehaviour
{
    void Awake()
    {
        //注册

        //主线程消息处理
        EventDispatcher.Instance.Regist<string>("1001", SyncMsg);
        //不同线程之间消息处理
        MsgDispatcher.Instance.Regist(this, 1002, AsyncMsg);
    }
    void Start()
    {
        //发送消息

        //主线程消息处理
        EventDispatcher.Instance.Dispatch<string>("1001", "Hello world 2");
        //不同线程之间消息处理
        MsgDispatcher.Instance.Dispatch(1002, null);
    }

    void SyncMsg(string s)
    {
        Debug.Log(s);
    }

    void AsyncMsg(byte[] data)
    {
        Debug.Log("Hello world 3");
    }
}
