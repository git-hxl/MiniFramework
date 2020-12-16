using System;
using UnityEngine;

namespace MiniFramework
{
    public class HearBeatComponent : MonoBehaviour
    {
        private float recvHearPackTime;
        private float checkHearPackInterval = 5f;

        private TcpClient tcpClient;
        private TcpServer tcpServer;
        private UdpClient udpClient;

        public bool isOK;
        private void Start()
        {
            EventManager.Instance.Regist(MsgID.Ping, RecvHeartPack);
            recvHearPackTime = Time.time;
        }

        public void Init(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }
        public void Init(TcpServer tcpServer)
        {
            this.tcpServer = tcpServer;
        }
        public void Init(UdpClient udpClient)
        {
            this.udpClient = udpClient;
        }

        /// <summary>
        /// 接收心跳包
        /// </summary>
        /// <param name="data"></param>
        private void RecvHeartPack(object data)
        {
            //Debug.Log("接收到心跳包");
            recvHearPackTime = Time.time;
            if (tcpClient != null)
                tcpClient.Send(MsgID.Pong, null);
            else if (tcpServer != null)
                tcpServer.Send(MsgID.Pong, null);
            else if (udpClient != null)
                udpClient.Send(MsgID.Pong, null);

            isOK = true;
        }


        private void Update()
        {
            if (isOK && Time.time - recvHearPackTime > checkHearPackInterval)
            {
                if (tcpClient != null)
                    tcpClient.Close();
                else if (tcpServer != null)
                    tcpServer.Close();
                else if (udpClient != null)
                    udpClient.Close();
                isOK = false;
            }
        }
    }
}