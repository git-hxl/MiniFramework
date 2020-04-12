using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;

namespace MiniFramework
{
    public class UdpClientComponent : MonoSingleton<UdpClientComponent>
    {
        public string TargetAddress;
        public int TargetPort;
        public int LocalPort;
        public bool IsActive;

        public UnityEvent ConnectAbort;

        private byte[] recvBuffer;
        private UdpClient udpClient;
        private DataPacker dataPacker;
        private IPAddress targetIP;
        void Start()
        {
            Launch();
        }
        public void Launch()
        {
            if (IsActive)
            {
                Debug.Log("主机已启动!");
                return;
            }
            try
            {
                dataPacker = new DataPacker();
                targetIP = SocketUtil.ParseIP(TargetAddress);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, LocalPort);
                udpClient = new UdpClient(endPoint);
                udpClient.EnableBroadcast = true;
                udpClient.BeginReceive(ReceiveResult, udpClient);
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
            udpClient = (UdpClient)ar.AsyncState;
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            recvBuffer = udpClient.EndReceive(ar, ref remote);
            dataPacker.UnPack(recvBuffer);
            udpClient.BeginReceive(ReceiveResult, udpClient);
        }
        public void Broadcast(int msgID, byte[] bodyData)
        {
            if (IsActive)
            {
                byte[] sendData = dataPacker.Packer(msgID, bodyData);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Broadcast, TargetPort);
                udpClient.BeginSend(sendData, sendData.Length, remoteEP, SendResult, udpClient);
                Debug.Log("广播消息ID:" + msgID + " 大小:" + sendData.Length + "字节");
            }
        }
        public void Send(int msgID, byte[] bodyData)
        {
            if (IsActive)
            {
                byte[] sendData = dataPacker.Packer(msgID, bodyData);
                IPEndPoint remoteEP = new IPEndPoint(targetIP, TargetPort);
                udpClient.BeginSend(sendData, sendData.Length, remoteEP, SendResult, udpClient);
                Debug.Log("发送消息ID:" + msgID + " 大小:" + sendData.Length + "字节");
            }
        }
        private void SendResult(IAsyncResult ar)
        {
            udpClient = (UdpClient)ar.AsyncState;
            udpClient.EndSend(ar);
        }
        public void Close()
        {
            if (udpClient != null && IsActive)
            {
                udpClient.Close();
                IsActive = false;
                Debug.Log("主动断开连接");
                ConnectAbort.Invoke();
            }
        }
        void OnDestroy()
        {
            Close();
        }
    }
}