using System;
using UnityEngine;

namespace MiniFramework
{
    public class TimeoutChecker : MonoSingleton<TimeoutChecker>
    {
        public Action ConnectTimeout;
        public Action NetWorkTimeout;
        private Coroutine heartAction;
        private float sendTime;
        private float recvTime;
        private void Start()
        {
            MsgDispatcher.Instance.Regist(this, MsgID.HeartPack, (obj) =>
            {
                //接收心跳包 
                recvTime = Time.time;
            });
        }
        /// <summary>
        /// 连接超时
        /// </summary>
        public void CheckConnectTimeout()
        {
            DelayAction.Excute(this, 3, () =>
            {
                if (!MiniTcpClient.Instance.IsConnected)
                {
                    Debug.LogError("连接超时!");
                    MiniTcpClient.Instance.Close();
                    if (ConnectTimeout != null)
                    {
                        ConnectTimeout();
                    }
                }
            });
        }
        /// <summary>
        /// 心跳超时
        /// </summary>
        public void CheckHeartPack()
        {
            RepeatAction.Excute(this, 5, -1, () =>
            {
                if (!MiniTcpClient.Instance.IsConnected)
                {
                    sendTime = 0;
                    recvTime = 0;
                    return;
                }
                if (sendTime - recvTime > 0)
                {
                    Debug.LogError("心跳包接收超时!");
                    MiniTcpClient.Instance.Close();
                    if (NetWorkTimeout != null)
                    {
                        NetWorkTimeout();
                    }
                    return;
                }
                MiniTcpClient.Instance.Send(MsgID.HeartPack, null);
                sendTime = Time.time;
            });
        }
    }
}