using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework
{
    public class Client
    {
        public Action ConnectSuccess;
        public Action ConnectFailed;
        public Action ConnectAbort;
        public bool IsConnected { get; set; }
        public bool IsConnecting { get; set; }
        private int maxBufferSize;
        private byte[] recvBuffer;
        private TcpClient tcpClient;
        private DataPacker dataPacker;
        public Client(int size)
        {
            maxBufferSize = size;
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
            IsConnecting = true;
            tcpClient.BeginConnect(IPAddress.Parse(ip), port, ConnectResult, tcpClient);
        }
        private void ConnectResult(IAsyncResult ar)
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            if (!tcpClient.Connected)
            {
                Debug.Log("连接服务器失败，请尝试重新连接!");
                if (ConnectFailed != null)
                {
                    ConnectFailed();
                }
            }
            else
            {
                tcpClient.EndConnect(ar);
                NetworkStream stream = tcpClient.GetStream();
                stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, tcpClient);
                IsConnected = true;
                Debug.Log("客户端连接成功");
                if (ConnectSuccess != null)
                {
                    ConnectSuccess();
                }

            }
            IsConnecting = false;
        }
        private void ReadResult(IAsyncResult ar)
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            if (tcpClient.Connected)
            {
                NetworkStream stream = tcpClient.GetStream();
                int recvLength = stream.EndRead(ar);
                if (recvLength <= 0)
                {
                    Debug.Log("网络中断");
                    if (ConnectAbort != null)
                    {
                        ConnectAbort();
                    }
                    Close();
                    return;
                }
                byte[] recvBytes = new byte[recvLength];
                Array.Copy(recvBuffer, 0, recvBytes, 0, recvLength);
                dataPacker.UnPack(recvBytes);
                stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, tcpClient);
            }
        }
        public void Send(PackHead head, byte[] bodyData)
        {
            byte[] sendData = dataPacker.Packer(head, bodyData);
            Send(sendData);
        }
        public void Send(byte[] data)
        {
            if (tcpClient.Connected)
            {
                tcpClient.GetStream().BeginWrite(data, 0, data.Length, SendResult, tcpClient);
                Debug.Log("发送数据：" + data.Length + "字节");
            }
            else
            {
                Debug.LogError("客户端未连接，无法发送数据");
            }
        }
        private void SendResult(IAsyncResult ar)
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            NetworkStream stream = tcpClient.GetStream();
            stream.EndWrite(ar);
            Debug.Log("数据发送成功");
        }
        public void Close()
        {
            if (tcpClient != null)
            {
                tcpClient.Close();
                IsConnected = false;
            }
            Debug.Log("连接已断开");
        }
    }
}

