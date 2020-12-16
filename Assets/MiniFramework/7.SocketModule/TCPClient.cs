using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
namespace MiniFramework
{
    public class TcpClient
    {
        private string address;
        private int port;
        private float timeout = 5f;
        private IPAddress ip;
        private byte[] recvBuffer;
        private int maxBufferSize = 1024;
        private DataPacker dataPacker;
        private System.Net.Sockets.TcpClient socket;
        public bool IsConnected { get { return socket == null ? false : socket.Connected; } }
        public TcpClient(string address, int port)
        {
            this.address = address;
            this.port = port;
            ip = SocketUtil.ParseIP(address);

        }
        public void Connect()
        {
            if (socket != null)
            {
                return;
            }
            recvBuffer = new byte[maxBufferSize];
            dataPacker = new DataPacker();
            Debug.Log("开始连接服务器：" + ip + " 端口：" + port);
            socket = new System.Net.Sockets.TcpClient();
            socket.BeginConnect(ip, port, ConnectResult, socket);
            CoroutineUtil.Instance.Create().AppendDelay(timeout).AppendEvent(() =>
            {
                if (socket != null && !socket.Connected)
                {
                    Close();
                }
            }).Start();
        }
        private void ConnectResult(IAsyncResult ar)
        {
            System.Net.Sockets.TcpClient socket = (System.Net.Sockets.TcpClient)ar.AsyncState;
            if (socket.Client != null && socket.Connected)
            {
                socket.EndConnect(ar);
                NetworkStream stream = socket.GetStream();
                stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, socket);
                Debug.Log("客户端连接成功!");
                MsgManager.Instance.Enqueue(MsgID.ConnectSuccess, "客户端连接成功");
            }
        }

        private void ReadResult(IAsyncResult ar)
        {
            System.Net.Sockets.TcpClient socket = (System.Net.Sockets.TcpClient)ar.AsyncState;
            if (socket.Client != null && socket.Connected)
            {
                NetworkStream stream = socket.GetStream();
                int recvLength = stream.EndRead(ar);
                if (recvLength > 0)
                {
                    byte[] recvBytes = new byte[recvLength];
                    Array.Copy(recvBuffer, 0, recvBytes, 0, recvLength);
                    DataPacker dataPacker = new DataPacker();
                    dataPacker.UnPack(recvBytes);
                    stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, socket);
                }
            }
        }
        public void Send(int msgID, byte[] data)
        {
            if (socket != null && socket.Connected)
            {
                byte[] sendData = dataPacker.Packer(msgID, data);
                socket.GetStream().BeginWrite(sendData, 0, sendData.Length, SendResult, socket);
            }
        }
        private void SendResult(IAsyncResult ar)
        {
            System.Net.Sockets.TcpClient socket = (System.Net.Sockets.TcpClient)ar.AsyncState;
            if (socket.Client != null && socket.Connected)
            {
                NetworkStream stream = socket.GetStream();
                stream.EndWrite(ar);
            }
        }

        public void Close()
        {
            if (socket != null)
            {
                if (socket.Connected)
                {
                    Debug.LogError("网络中断");
                    MsgManager.Instance.Enqueue(MsgID.ConnectFailed, "网络中断");
                }
                else
                {

                    Debug.LogError("连接失败");
                    MsgManager.Instance.Enqueue(MsgID.ConnectFailed, "连接失败");
                }
                socket.Close();
                socket = null;
            }
        }
    }
}