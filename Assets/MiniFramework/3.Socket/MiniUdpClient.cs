using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework
{
    public class MiniUdpClient : MonoSingleton<MiniUdpClient>
    {
        public bool IsActive { get; set; }
        private byte[] recvBuffer;
        private UdpClient udpClient;
        private DataPacker dataPacker;
        public void Launch(int port)
        {
            if (IsActive)
            {
                Debug.Log("主机已启动!");
                return;
            }
            try
            {
                dataPacker = new DataPacker();
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
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
        public void Send(int msgID, byte[] bodyData, string remoteIP, int remotePort)
        {
            if (IsActive)
            {
                PackHead head = new PackHead();
                head.MsgID = (short)msgID;
                byte[] sendData = dataPacker.Packer(head, bodyData);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
                udpClient.BeginSend(sendData, sendData.Length, remoteEP, SendResult, udpClient);
                Debug.Log("发送消息ID:" + head.MsgID + " 大小:" + sendData.Length + "字节");
            }
            else
            {
                Debug.LogError("主机未启动，无法发送数据");
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
            }
        }
        private void OnDestroy()
        {
            Close();
        }
    }
}