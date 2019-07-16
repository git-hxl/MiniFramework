using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework
{
    public class Host : MonoSingleton<Host>
    {
        public int Port;
        public bool IsActive { get; set; }
        private byte[] recvBuffer;
        private UdpClient udpClient;
        private DataPacker dataPacker;
        private void Start() {
            Launch(Port);
        }
        public void Launch(int port)
        {
            if (IsActive)
            {
                Debug.Log("主机已启动!");
                return;
            }
            dataPacker = new DataPacker();
            try
            {
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
        public void Send(PackHead head, byte[] bodyData, string remoteIP, int remotePort)
        {
            byte[] sendData = dataPacker.Packer(head, bodyData);
            Send(sendData, remoteIP, remotePort);
        }
        private void Send(byte[] data, string remoteIP, int remotePort)
        {
            if (IsActive)
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort);
                udpClient.BeginSend(data, data.Length, remoteEP, SendResult, udpClient);
                Debug.Log("发送数据：" + data.Length + "字节");
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
            Debug.Log("数据发送成功");
        }
        public void Close()
        {
            if (udpClient != null)
            {
                udpClient.Close();
                IsActive = false;
            }
            Debug.Log("连接已断开");
        }
    }
}