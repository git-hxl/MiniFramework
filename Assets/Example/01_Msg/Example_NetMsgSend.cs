using MiniFramework;
using System.Text;
using System.Threading;
using UnityEngine;

public class Example_NetMsgSend : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Thread thread = new Thread(()=> NetMsgDispatcher.Instance.Dispatch(NetMsgID.Test, Encoding.UTF8.GetBytes("Hello")));
        thread.Start();
	}
}