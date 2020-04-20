using UnityEngine;

namespace MiniFramework.Network
{
    public partial class SocketManager : MonoSingleton<SocketManager>, ISocketManager
    {
        private TcpClient tcpClient;
        public ITcpClient GetTcpClient
        {
            get
            {
                if (tcpClient == null)
                {
                    tcpClient = new TcpClient(this);
                    tcpClient.ConnectSuccess += TcpClient_ConnectSuccess;
                    tcpClient.ConnectAbort += TcpClient_ConnectAbort;
                }
                return tcpClient;
            }
        }
        private TcpServer tcpServer;
        public ITcpServer GetTcpServer
        {
            get
            {
                if (tcpServer == null)
                {
                    tcpServer = new TcpServer();
                }
                return tcpServer;
            }
        }
        private float sendheartPackTime;
        private float recvHearPackTime;
        private float hearPackInterval = 2f;
        private SequenceNode sequenceNodeHeartPack;
        private void TcpClient_ConnectAbort()
        {
            sequenceNodeHeartPack.Stop();
            MsgManager.Instance.UnRegist(MsgID.HeartPack, TcpClient_RecvHeartPack);
        }

        private void TcpClient_ConnectSuccess()
        {
            sendheartPackTime = 0f;
            recvHearPackTime = 0f;
            sequenceNodeHeartPack = this.Sequence().Repeat(-1, hearPackInterval, TcpClient_SendHeartPack).Begin();
            MsgManager.Instance.Regist(MsgID.HeartPack, TcpClient_RecvHeartPack);
        }
        /// <summary>
        /// 发送心跳包
        /// </summary>
        private void TcpClient_SendHeartPack()
        {
            if (sendheartPackTime - recvHearPackTime > hearPackInterval)
            {
                Debug.LogError("心跳超时");
                tcpClient.Close();
                return;
            }
            tcpClient.Send(MsgID.HeartPack, null);
            sendheartPackTime = Time.time;
        }
        /// <summary>
        /// 接收心跳包
        /// </summary>
        /// <param name="data"></param>
        private void TcpClient_RecvHeartPack(byte[] data)
        {
            recvHearPackTime = Time.time;
        }

        private void OnDestroy()
        {
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }
    }
}