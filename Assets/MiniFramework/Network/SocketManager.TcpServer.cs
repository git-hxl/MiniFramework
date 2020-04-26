using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework.Network
{
    public sealed partial class SocketManager
    {
        public class TcpServer : ITcpServer
        {
            public bool IsActive { get; set; }
            public int Port { get; set; }
            public int MaxConnections { get; set; }
            public int MaxBufferSize { get; set; }
            private byte[] recvBuffer;
            private List<System.Net.Sockets.TcpClient> remoteClients;
            private TcpListener tcpListener;
            private DataPacker dataPacker;

            public TcpServer()
            {
                MaxConnections = 12;
                MaxBufferSize = 1024;
            }
            public void SetPort(int port)
            {
                Port = port;
            }
            [ContextMenu("开启服务器")]
            public void Launch()
            {
                if (IsActive)
                { return; }
                recvBuffer = new byte[MaxBufferSize];
                remoteClients = new List<System.Net.Sockets.TcpClient>();
                dataPacker = new DataPacker();
                try
                {
                    tcpListener = new TcpListener(IPAddress.Any, Port);
                    tcpListener.Start(MaxConnections);
                    tcpListener.BeginAcceptTcpClient(AcceptResult, tcpListener);
                    IsActive = true;
                    Debug.Log("服务端启动成功:" + Port);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            private void AcceptResult(IAsyncResult ar)
            {
                tcpListener = (TcpListener)ar.AsyncState;
                System.Net.Sockets.TcpClient remoteClient = tcpListener.EndAcceptTcpClient(ar);
                remoteClients.Add(remoteClient);
                NetworkStream networkStream = remoteClient.GetStream();
                networkStream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, remoteClient);
                tcpListener.BeginAcceptTcpClient(AcceptResult, tcpListener);
                Debug.Log("远程客户端:" + remoteClient.Client.RemoteEndPoint + "接入成功");
            }

            private void ReadResult(IAsyncResult ar)
            {
                System.Net.Sockets.TcpClient tcpClient = (System.Net.Sockets.TcpClient)ar.AsyncState;
                NetworkStream stream = tcpClient.GetStream();
                int recvLength = stream.EndRead(ar);
                if (recvLength <= 0)
                {
                    Debug.Log("远程客户端:" + tcpClient.Client.RemoteEndPoint + "已经断开");
                    remoteClients.Remove(tcpClient);
                    tcpClient.Close();
                    return;
                }
                byte[] recvBytes = new byte[recvLength];
                Array.Copy(recvBuffer, 0, recvBytes, 0, recvLength);
                dataPacker.UnPack(recvBytes);
                stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, tcpClient);
            }
            public void Send(int msgID, byte[] bodyData)
            {
                if (IsActive)
                {
                    byte[] sendData = dataPacker.Packer(msgID, bodyData);
                    for (int i = 0; i < remoteClients.Count; i++)
                    {
                        System.Net.Sockets.TcpClient client = remoteClients[i];
                        if (client.Connected)
                        {
                            client.GetStream().BeginWrite(sendData, 0, sendData.Length, SendResult, client);
                        }
                    }
                    Debug.Log("发送消息ID：" + msgID + " 大小：" + sendData.Length + "字节");
                }
            }
            private void SendResult(IAsyncResult ar)
            {
                System.Net.Sockets.TcpClient tcpClient = (System.Net.Sockets.TcpClient)ar.AsyncState;
                NetworkStream stream = tcpClient.GetStream();
                stream.EndWrite(ar);
            }
            public void Close()
            {
                if (remoteClients != null)
                {
                    foreach (var item in remoteClients)
                    {
                        if (item.Connected)
                        {
                            item.Close();
                        }
                    }
                    remoteClients.Clear();
                    Debug.Log("已断开远程客户端");
                }
                if (tcpListener != null && IsActive)
                {
                    tcpListener.Stop();
                    IsActive = false;
                    Debug.Log("已关闭服务器监听");
                }
            }
        }
    }
}