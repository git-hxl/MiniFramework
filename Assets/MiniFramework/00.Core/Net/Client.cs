﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace MiniFramework
{
    public class Client
    {
        public Socket Socket;
        public Client(string serverIP, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse(SocketManager.Instance.serverIP);
            IPEndPoint endPoint = new IPEndPoint(address, SocketManager.Instance.serverPort);
            Socket.BeginConnect(endPoint, ConnectCallback, Socket);
        }

        void ConnectCallback(IAsyncResult ar)
        {
            if (Socket.Connected)
            {
                Debug.Log("连接服务器成功！");
                Thread thread = new Thread(SocketManager.Instance.Receive);
                thread.IsBackground = true;
                thread.Start(Socket);
            }
            else
            {
                Debug.Log("连接服务器失败！");
            }
        }

        public void Send(string data)
        {
            if (Socket.Connected)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteString(data);
                Socket.Send(buffer.ToBytes());
            }
        }
        public void Close()
        {
            if (Socket != null)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Debug.Log("已断开连接");
            }
        }
    }
}

