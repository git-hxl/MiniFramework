using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;

namespace MiniFramework.Network
{
    public sealed partial class SocketManager
    {
        public class UdpClient : IUdpClient
        {

            public string Address { get; set; }

            public int Port { get; set; }

            public bool IsActive { get; set; }

            public event Action ConnectAbort;

            private IPAddress iP;
            private IPEndPoint iPEndPoint;
            private byte[] recvBuffer;
            private System.Net.Sockets.UdpClient socket;
            private DataPacker dataPacker;
            public void Init(string address, int port)
            {
                Address = address;
                Port = port;
                iP = SocketUtil.ParseIP(Address);
                iPEndPoint = new IPEndPoint(iP, Port);
            }

            public void Launch()
            {
                if (IsActive)
                { return; }
                try
                {
                    dataPacker = new DataPacker();

                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, Port);
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
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Broadcast, Port);
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
                    IsActive = false;
                    Debug.Log("主动关闭连接");
                    ConnectAbort?.Invoke();
                }
            }

        }
    }

}