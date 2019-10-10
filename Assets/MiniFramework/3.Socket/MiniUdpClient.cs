using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework
{
    public class MiniUdpClient : Singleton<MiniUdpClient>
    {
        public bool IsActive;
        private byte[] recvBuffer;
        private UdpClient udpClient;
        private DataPacker dataPacker;
        private IPAddress ip;
        private int port;
        private MiniUdpClient() { }
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
        public void SetDefaultIPEndPoint(string address, int port)
        {
            if (!IPAddress.TryParse(address, out ip))
            {
                IPHostEntry hostInfo = Dns.GetHostEntry(address);
                ip = hostInfo.AddressList[0];
            }
            this.port = port;
        }
        private void ReceiveResult(IAsyncResult ar)
        {
            udpClient = (UdpClient)ar.AsyncState;
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            recvBuffer = udpClient.EndReceive(ar, ref remote);
            dataPacker.UnPack(recvBuffer);
            udpClient.BeginReceive(ReceiveResult, udpClient);
        }
        public void Send(int msgID, byte[] bodyData)
        {
            if (udpClient != null && IsActive)
            {
                PackHead head = new PackHead();
                head.MsgID = (short)msgID;
                byte[] sendData = dataPacker.Packer(head, bodyData);
                IPEndPoint remoteEP = new IPEndPoint(ip, port);
                udpClient.BeginSend(sendData, sendData.Length, remoteEP, SendResult, udpClient);
                Debug.Log("发送消息ID:" + head.MsgID + " 大小:" + sendData.Length + "字节");
            }
        }
        private void SendResult(IAsyncResult ar)
        {
            udpClient = (UdpClient)ar.AsyncState;
            udpClient.EndSend(ar);
        }
        public void Close()
        {
            if (udpClient != null)
            {
                udpClient.Close();
                udpClient = null;
                IsActive = false;
                Debug.Log("主动断开连接");
            }
        }
    }
}