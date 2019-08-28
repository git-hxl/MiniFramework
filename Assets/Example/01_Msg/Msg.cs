using UnityEngine;
using MiniFramework;
using System.Text;

public class Msg : MonoBehaviour
{
    void Awake()
    {
        //注册
        EventDispatcher.Instance.Regist<string>("1001", Test);

        MsgDispatcher.Instance.Regist(this, 1002, Test);
    }
    void Start()
    {
        //发送消息
        EventDispatcher.Instance.Dispatch<string>("1001", "Hello world 2");

        MsgDispatcher.Instance.Dispatch(1002,null);
    }

    void Test(string s)
    {
        Debug.Log(s);
    }

    void Test(byte[] data)
    {
        Debug.Log("Hello world 3");
    }
}
