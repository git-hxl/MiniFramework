using System;
using UnityEngine;

namespace MiniFramework
{
    public class HearBeatComponent : MonoBehaviour
    {
        private float recvHearPackTime;
        private float checkHearPackInterval = 5f;

        private ISocket socket;

        private bool isOK;
        private void Start()
        {
            EventManager.Instance.Regist(MsgID.Ping, RecvHeartPack);
            recvHearPackTime = Time.time;
        }

        public void Init(ISocket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// 接收心跳包
        /// </summary>
        /// <param name="data"></param>
        private void RecvHeartPack(object data)
        {
            Debug.Log("接收到心跳");
            recvHearPackTime = Time.time;
            if (socket != null)
                socket.Send(MsgID.Ping, null);
            isOK = true;
        }

        /// <summary>
        /// 定时检测是否收到心跳包
        /// </summary>
        private void Update()
        {
            if (isOK && Time.time - recvHearPackTime > checkHearPackInterval)
            {
                if (socket != null)
                    socket.Close();
                isOK = false;
            }
        }
    }
}