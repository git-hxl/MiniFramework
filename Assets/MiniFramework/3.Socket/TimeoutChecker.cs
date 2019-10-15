using System;
using UnityEngine;

namespace MiniFramework
{
    public class TimeoutChecker : MonoSingleton<TimeoutChecker>
    {
        public Action ConnectTimeout;
        public Action NetWorkTimeout;
        public int NetWorkLatency;
        public int HeartPackLatency;
        private Coroutine heartAction;
        private float sendTime;
        private float recvTime;

        public override void Init()
        {
            MsgDispatcher.Instance.Regist(this, MsgID.HeartPack, (obj) =>
            {
                //接收心跳包 
                recvTime = Time.time;
                HeartPackLatency = (int)((recvTime - sendTime) * 1000);
            });
        }
        public void AutoSendPing(string address)
        {
            RepeatAction.Excute(this, 1, -1, () => SendPing(address));
        }
        private void SendPing(string address)
        {
            Ping ping = new Ping(address);
            UntilAction.Excute(this, () => ping.isDone, () =>
            {
                NetWorkLatency = ping.time;
                ping.DestroyPing();
            });
        }
        /// <summary>
        /// 连接超时
        /// </summary>
        public void CheckConnectTimeout(MiniTcpClient tcpClient)
        {
            DelayAction.Excute(this, 3, () =>
            {
                if (!tcpClient.IsConnected)
                {
                    Debug.LogError("连接超时!");
                    tcpClient.Close();
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
        public void CheckHeartPack(MiniTcpClient miniTcpClient)
        {
            RepeatAction.Excute(this, 5, -1, () =>
            {
                if (!miniTcpClient.IsConnected)
                {
                    sendTime = 0;
                    recvTime = 0;
                    return;
                }
                if (recvTime - sendTime < 0)
                {
                    Debug.LogError("心跳包接收超时!");
                    miniTcpClient.Close();
                    if (NetWorkTimeout != null)
                    {
                        NetWorkTimeout();
                    }
                    return;
                }
                miniTcpClient.Send(MsgID.HeartPack, null);
                sendTime = Time.time;
            });
        }

        public void CheckHeartPack(MiniUdpClient miniUdpClient)
        {
            RepeatAction.Excute(this, 5, -1, () =>
            {
                if (!miniUdpClient.IsActive)
                {
                    sendTime = 0;
                    recvTime = 0;
                    return;
                }
                if (recvTime - sendTime < 0)
                {
                    Debug.LogError("心跳包接收超时!");
                    miniUdpClient.Close();
                    if (NetWorkTimeout != null)
                    {
                        NetWorkTimeout();
                    }
                    return;
                }
                miniUdpClient.Send(MsgID.HeartPack, null);
                sendTime = Time.time;
            });
        }
    }
}