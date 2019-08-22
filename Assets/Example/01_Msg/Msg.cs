using UnityEngine;
using MiniFramework;
using System.Text;

public class Msg : MonoBehaviour
{
    void Awake()
    {
        //注册
        EventDispatcher.Instance.Regist("1000", null);
        //EventDispatcher.Instance.Regist("1000", Test);

        EventDispatcher.Instance.Regist<string>("1001", Test);

        MsgDispatcher.Instance.Regist(this, 1002, Test);
    }
    void Start()
    {
        //发送消息
        //EventDispatcher.Instance.UnRegist("1000",Test);
        EventDispatcher.Instance.Dispatch("1000");

        EventDispatcher.Instance.Dispatch<string>("1001", "Hello world 2");

        //MsgDispatcher.Instance.UnRegist(this,1002,Test);
        MsgDispatcher.Instance.Send(1002);
    }

    void Test()
    {
        Debug.Log("Hello world");
    }

    void Test(string s)
    {
        Debug.Log(s);
    }

    void Test(object[] data)
    {
        Debug.Log("Hello world 3");
    }
}
