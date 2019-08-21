using UnityEngine;
using MiniFramework;
public class Msg : MonoBehaviour
{
    void Awake()
    {
		//注册
       // MsgManager.Instance.RegisterMsg(this, "Test", (txt) => Debug.Log(txt));
       EventDispatcher.GameEvent.Regist("1000",Test);
    //    EventDispatcher.GameEvent.Regist("1000",()=>{
    //        Debug.Log("Hello world 2");
    //    });
       EventDispatcher.GameEvent.Regist<string>("1000",Test);
    }
    void Start()
    {
		//发送消息
        //MsgManager.Instance.SendMsg("Test", "hello");
        // EventDispatcher.GameEvent.UnRegist("1000",Test);

        // EventDispatcher.GameEvent.DispatchEvent("1000");
        // EventDispatcher.GameEvent.DispatchEvent<string>("10001","xxxxx");
    }

    void Test()
    {
        Debug.Log("Hello world");
    }
    void Test(string s)
    {
        Debug.Log(s);
    }
}
