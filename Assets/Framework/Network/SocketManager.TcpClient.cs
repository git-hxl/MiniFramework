
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework.Network
{
    public partial class SocketManager
    {
        class TcpClient : ITcpClient
        {
            public string Address { get; set; }
            public int Port { get; set; }

            public bool IsConnected { get; set; }
            public bool IsConnecting { get; set; }
            public float ConnectTimeout { get; set; }

            public event Action ConnectSuccess;
            public event Action ConnectFailed;
            public event Action ConnectAbort;

            private IPAddress ip;
            private byte[] recvBuffer;
            private int maxBufferSize;
            private DataPacker dataPacker;
            private System.Net.Sockets.TcpClient socket;
            private SocketManager socketManager;
            private SequenceNode sequenceNodeConnect;

            private HearBeat hearBeat;
            public TcpClient(SocketManager socketManager)
            {
                maxBufferSize = 1024;
                ConnectTimeout = 5f;
                this.socketManager = socketManager;
                this.hearBeat = new HearBeat(socketManager,this);

                ConnectSuccess += hearBeat.Start;
                ConnectAbort += hearBeat.Stop;

                MsgManager.Instance.Regist(MsgID.ConnectSuccess, (data) => ConnectSuccess?.Invoke());
                MsgManager.Instance.Regist(MsgID.ConnectFailed, (data) => ConnectFailed?.Invoke());
                MsgManager.Instance.Regist(MsgID.ConnectAbort, (data) => ConnectAbort?.Invoke());
            }
            public void SetIPEndPoint(string address, int port)
            {
                Address = address;
                Port = port;
                ip = SocketUtil.ParseIP(Address);
            }

            public void Connect()
            {
                if (IsConnecting || IsConnected)
                { return; }
                IsConnecting = true;
                recvBuffer = new byte[maxBufferSize];
                dataPacker = new DataPacker();
                Debug.Log("开始连接服务器：" + ip + " 端口：" + Port);
                socket = new System.Net.Sockets.TcpClient();
                socket.BeginConnect(ip, Port, ConnectResult, socket);
                //连接超时检测
                sequenceNodeConnect = socketManager.Sequence().Delay(ConnectTimeout).Event(() =>
                {
                    if (IsConnecting && !IsConnected)
                    {
                        socket.Close();
                    }
                }).Begin();
            }
            private void ConnectResult(IAsyncResult ar)
            {
                socket = (System.Net.Sockets.TcpClient)ar.AsyncState;
                if (socket.Client != null && socket.Connected)
                {
                    socket.EndConnect(ar);
                    NetworkStream stream = socket.GetStream();
                    stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, socket);
                    Debug.Log("客户端连接成功!");
                    IsConnected = true;
                    MsgManager.Instance.Dispatch(MsgID.ConnectSuccess, null);
                }
                else
                {
                    Debug.LogError("客户端连接失败!");
                    IsConnected = false;
                    MsgManager.Instance.Dispatch(MsgID.ConnectFailed, null);
                }
                IsConnecting = false;
            }

            private void ReadResult(IAsyncResult ar)
            {
                socket = (System.Net.Sockets.TcpClient)ar.AsyncState;
                if (socket.Connected)
                {
                    NetworkStream stream = socket.GetStream();
                    int recvLength = stream.EndRead(ar);
                    if (recvLength > 0)
                    {
                        byte[] recvBytes = new byte[recvLength];
                        Array.Copy(recvBuffer, 0, recvBytes, 0, recvLength);
                        dataPacker.UnPack(recvBytes);
                        stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, socket);
                    }
                    else
                    {
                        Debug.LogError("网络中断");
                        Close();
                    }
                }
            }
            public void Send(int msgID, byte[] data)
            {
                if (socket.Client != null && socket.Connected)
                {
                    byte[] sendData = dataPacker.Packer(msgID, data);
                    socket.GetStream().BeginWrite(sendData, 0, sendData.Length, SendResult, socket);
                    Debug.Log("发送消息ID：" + msgID + " 大小：" + sendData.Length + "字节");
                }
            }
            private void SendResult(IAsyncResult ar)
            {
                socket = (System.Net.Sockets.TcpClient)ar.AsyncState;
                if (socket.Connected)
                {
                    NetworkStream stream = socket.GetStream();
                    stream.EndWrite(ar);
                }
            }

            public void Close()
            {
                if (socket.Client != null && socket.Connected)
                {
                    socket.Close();
                    IsConnected = false;
                    Debug.LogError("主动关闭连接");
                    MsgManager.Instance.Dispatch(MsgID.ConnectAbort, null);
                }
            }

        }
    }
}
