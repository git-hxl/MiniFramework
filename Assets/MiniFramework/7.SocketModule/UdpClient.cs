using System;
using System.Net;
using UnityEngine;
namespace MiniFramework
{
    public class UdpClient : ISocket
    {
        private string address;
        private int port;
        public bool IsActive { get; set; }
        private IPAddress ip;
        private IPEndPoint iPEndPoint;
        private byte[] recvBuffer;
        private System.Net.Sockets.UdpClient socket;
        private DataPacker dataPacker;
        public UdpClient(string address, int port)
        {
            this.address = address;
            this.port = port;
            ip = SocketUtil.ParseIP(address);
            iPEndPoint = new IPEndPoint(ip, port);
        }
        public void Launch()
        {
            if (socket != null)
            { return; }
            try
            {
                dataPacker = new DataPacker();
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                socket = new System.Net.Sockets.UdpClient(endPoint);
                socket.EnableBroadcast = true;
                socket.BeginReceive(ReceiveResult, socket);
                IsActive = true;
                Debug.Log("主机初始化成功");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        private void ReceiveResult(IAsyncResult ar)
        {
            socket = (System.Net.Sockets.UdpClient)ar.AsyncState;
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            recvBuffer = socket.EndReceive(ar, ref remote);
            dataPacker.UnPack(recvBuffer);
            socket.BeginReceive(ReceiveResult, socket);
        }
        public void Broadcast(int msgID, byte[] data)
        {
            if (IsActive)
            {
                byte[] sendData = dataPacker.Packer(msgID, data);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Broadcast, port);
                socket.BeginSend(sendData, sendData.Length, remoteEP, SendResult, socket);
                Debug.Log("广播消息ID:" + msgID + " 大小:" + sendData.Length + "字节");
            }
        }
        public void Send(int msgID, byte[] data)
        {
            if (IsActive)
            {
                byte[] sendData = dataPacker.Packer(msgID, data);
                socket.BeginSend(sendData, sendData.Length, iPEndPoint, SendResult, socket);
                Debug.Log("发送消息ID:" + msgID + " 大小:" + sendData.Length + "字节");
            }
        }
        private void SendResult(IAsyncResult ar)
        {
            socket = (System.Net.Sockets.UdpClient)ar.AsyncState;
            socket.EndSend(ar);
        }

        public void Close()
        {
            if (socket != null && IsActive)
            {
                socket.Close();
                socket = null;
                IsActive = false;
                Debug.LogError("网络中断");
                MsgManager.Instance.Enqueue(MsgID.ConnectFailed, "网络中断");
            }
        }

    }
}