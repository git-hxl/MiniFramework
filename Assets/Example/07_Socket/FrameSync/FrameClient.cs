using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework;
using System.Text;

public class FrameClient : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        MiniTcpClient.Instance.Launch("127.0.0.1", 8888);
        TimeoutChecker.Instance.CheckConnectTimeout();
        TimeoutChecker.Instance.CheckTcpHeartPack();
    }


	private void OnDestroy() {
		MiniTcpClient.Instance.Close();
	}
}
