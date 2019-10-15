using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework
{
    public class MiniTcpClient
    {
        public bool IsConnected;
        public Action ConnectSuccess;
        public IPAddress IP;
        public int Port;
        private int maxBufferSize = 1024;
        private byte[] recvBuffer;
        private TcpClient tcpClient;
        private DataPacker dataPacker;
        public void Launch(string address, int port)
        {
            Port = port;
            if (!IPAddress.TryParse(address, out IP))
            {
                IPHostEntry hostInfo = Dns.GetHostEntry(address);
                IP = hostInfo.AddressList[0];
            }
            Connect(IP, Port);
        }
        private void Connect(IPAddress ip, int port)
        {
            if (IsConnected)
            {
                Debug.Log("客户端已连接");
                return;
            }
            recvBuffer = new byte[maxBufferSize];
            dataPacker = new DataPacker();
            tcpClient = new TcpClient();
            tcpClient.BeginConnect(ip, port, ConnectResult, tcpClient);
            Debug.Log("开始连接服务器：" + ip + " 端口：" + port);
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
                PackHead head = new PackHead();
                head.MsgID = (short)msgID;
                byte[] sendData = dataPacker.Packer(head, bodyData);
                tcpClient.GetStream().BeginWrite(sendData, 0, sendData.Length, SendResult, tcpClient);
                Debug.Log("发送消息ID：" + head.MsgID + " 大小：" + sendData.Length + "字节");
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
    }
}