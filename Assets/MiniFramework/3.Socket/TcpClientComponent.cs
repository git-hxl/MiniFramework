using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework
{
    public class TcpClientComponent : MonoSingleton<TcpClientComponent>
    {
        public string Address;
        public int Port;
        public bool IsConnected;

        public Action ConnectSuccess;
        public Action ConnectTimeout;
        public Action ConnectAbort;

        private int maxBufferSize = 1024;
        private byte[] recvBuffer;
        private TcpClient tcpClient;
        private DataPacker dataPacker;

        void Start()
        {
            Connect();
        }

        public void Connect()
        {
            if (IsConnected)
            {
                Debug.Log("客户端已连接");
                return;
            }
            recvBuffer = new byte[maxBufferSize];
            dataPacker = new DataPacker();
            tcpClient = new TcpClient();
            IPAddress ip = NetworkUtil.ParseIP(Address);
            tcpClient.BeginConnect(ip, Port, ConnectResult, tcpClient);
            Debug.Log("开始连接服务器：" + ip + " 端口：" + Port);
            CheckTimeout();
        }
        private void CheckTimeout()
        {
            DelayAction.Excute(this, 5, () =>
            {
                if (!IsConnected)
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
        private void ConnectResult(IAsyncResult ar)
        {
            tcpClient = (TcpClient)ar.AsyncState;
            tcpClient.EndConnect(ar);
            if (tcpClient.Connected)
            {
                NetworkStream stream = tcpClient.GetStream();
                stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, tcpClient);
                IsConnected = true;
                Debug.Log("客户端连接成功");
                if (ConnectSuccess != null)
                {
                    ConnectSuccess();
                }
            }
        }
        private void ReadResult(IAsyncResult ar)
        {
            tcpClient = (TcpClient)ar.AsyncState;
            NetworkStream stream = tcpClient.GetStream();
            int recvLength = stream.EndRead(ar);
            if (recvLength > 0)
            {
                byte[] recvBytes = new byte[recvLength];
                Array.Copy(recvBuffer, 0, recvBytes, 0, recvLength);
                dataPacker.UnPack(recvBytes);
                stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, tcpClient);
            }
        }
        public void Send(int msgID, byte[] bodyData)
        {
            if (IsConnected)
            {
                byte[] sendData = dataPacker.Packer(msgID, bodyData);
                tcpClient.GetStream().BeginWrite(sendData, 0, sendData.Length, SendResult, tcpClient);
                Debug.Log("发送消息ID：" + msgID + " 大小：" + sendData.Length + "字节");
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
                Debug.LogError("主动断开连接");
            }
        }
        void OnDestroy()
        {
            Close();
        }
    }
}