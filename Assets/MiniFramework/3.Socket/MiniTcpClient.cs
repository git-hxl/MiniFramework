using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework
{
    public class MiniTcpClient : MonoSingleton<MiniTcpClient>
    {
        public bool IsConnected;
        private int connectTimeout = 5;
        private int heartIntervalTime = 10;
        private int maxBufferSize = 1024;
        private byte[] recvBuffer;
        private TcpClient tcpClient;
        private DataPacker dataPacker;
        private DateTime lastRecvHeartTime = DateTime.MinValue;
        private Sequence heartSequence;
        protected override void Awake()
        {
            base.Awake();
            MsgManager.Instance.RegisterMsg(this, MsgID.HeartPack, (obj) =>
            {
                //接收到心跳包
                lastRecvHeartTime = DateTime.Now;
                Debug.Log("接收到心跳包");
            });
            MsgManager.Instance.RegisterMsg(this, MsgID.ConnectSuccess, (obj) =>
            {
                //心跳包发送
                heartSequence = this.Repeat(heartIntervalTime/2, -1, () =>
                {
                    if (lastRecvHeartTime == DateTime.MinValue)
                    {
                        lastRecvHeartTime = DateTime.Now;
                    }
                    else if ((DateTime.Now - lastRecvHeartTime).TotalSeconds > heartIntervalTime)
                    {
                        tcpClient.Close();
                        IsConnected = false;
                        Debug.LogError("网络超时!");
                        MsgManager.Instance.SendMsg(MsgID.NetTimeout, null);
                        return;
                    }
                    Send(MsgID.HeartPack, null);
                });
            });
            MsgManager.Instance.RegisterMsg(this, MsgID.ConnectFailed, (obj) =>
            {

            });
            MsgManager.Instance.RegisterMsg(this, MsgID.ConnectAbort, (obj) =>
            {

            });
            MsgManager.Instance.RegisterMsg(this, MsgID.NetTimeout, (obj) =>
            {
                heartSequence.Close();
            });
        }
        public void Connect(string ip, int port)
        {
            if (IsConnected)
            {
                Debug.Log("客户端已连接");
                return;
            }
            recvBuffer = new byte[maxBufferSize];
            dataPacker = new DataPacker();
            tcpClient = new TcpClient();
            tcpClient.BeginConnect(IPAddress.Parse(ip), port, ConnectResult, tcpClient);
            this.Delay(connectTimeout, () =>
            {
                if (tcpClient.Client != null && !IsConnected)
                {
                    tcpClient.Close();
                    IsConnected = false;
                    Debug.Log("连接超时!");
                    MsgManager.Instance.SendMsg(MsgID.ConnectFailed, null);
                }
            });
        }
        private void ConnectResult(IAsyncResult ar)
        {
            tcpClient = (TcpClient)ar.AsyncState;
            if (!tcpClient.Connected)
            {
                tcpClient.Close();
                IsConnected = false;
                Debug.Log("连接服务器失败，请尝试重新连接!");
                MsgManager.Instance.SendMsg(MsgID.ConnectFailed, null);
            }
            else
            {
                tcpClient.EndConnect(ar);
                NetworkStream stream = tcpClient.GetStream();
                stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, tcpClient);
                IsConnected = true;
                Debug.Log("客户端连接成功");
                MsgManager.Instance.SendMsg(MsgID.ConnectSuccess, null);
            }
        }
        private void ReadResult(IAsyncResult ar)
        {
            tcpClient = (TcpClient)ar.AsyncState;
            NetworkStream stream = tcpClient.GetStream();
            int recvLength = stream.EndRead(ar);
            if (recvLength <= 0)
            {
                tcpClient.Close();
                IsConnected = false;
                Debug.LogError("网络中断");
                MsgManager.Instance.SendMsg(MsgID.ConnectAbort, null);
                return;
            }
            byte[] recvBytes = new byte[recvLength];
            Array.Copy(recvBuffer, 0, recvBytes, 0, recvLength);
            dataPacker.UnPack(recvBytes);
            stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, tcpClient);
        }
        public void Send(int msgID, byte[] bodyData)
        {
            if (IsConnected)
            {
                PackHead head = new PackHead();
                head.MsgID = (short)msgID;
                byte[] sendData = dataPacker.Packer(head, bodyData);
                tcpClient.GetStream().BeginWrite(sendData, 0, sendData.Length, SendResult, tcpClient);
                Debug.Log("发送消息ID:" + head.MsgID + " 大小:" + sendData.Length + "字节");
            }
            else
            {
                Debug.LogError("连接已断开，发送失败!");
            }
        }
        private void SendResult(IAsyncResult ar)
        {
            tcpClient = (TcpClient)ar.AsyncState;
            NetworkStream stream = tcpClient.GetStream();
            stream.EndWrite(ar);
        }
        public void Close()
        {
            if (tcpClient != null && IsConnected)
            {
                tcpClient.Close();
                IsConnected = false;
                Debug.Log("主动断开连接");
            }
        }
        private void OnDestroy()
        {
            Close();
        }
    }
}