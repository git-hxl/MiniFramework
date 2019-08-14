using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MiniFramework
{
    public class Client : MonoSingleton<Client>
    {
        private int maxBufferSize = 1024;
        private byte[] recvBuffer;
        private TcpClient tcpClient;
        private DataPacker dataPacker;
        public void Connect(string ip, int port)
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                Debug.Log("客户端已连接");
                return;
            }
            recvBuffer = new byte[maxBufferSize];
            dataPacker = new DataPacker();
            tcpClient = new TcpClient();
            tcpClient.BeginConnect(IPAddress.Parse(ip), port, ConnectResult, tcpClient);

            MsgManager.Instance.RegisterMsg(this,MsgID.ConnectSuccess,(obj)=>{
                this.Repeat(5f,-1,()=>{
                    Send(MsgID.HeartPack,new byte[0]);
                });
            });
            MsgManager.Instance.RegisterMsg(this,MsgID.ConnectFailed,(obj)=>{

            });
            MsgManager.Instance.RegisterMsg(this,MsgID.ConnectAbort,(obj)=>{

            });
        }
        private void ConnectResult(IAsyncResult ar)
        {
            tcpClient = (TcpClient)ar.AsyncState;
            if (!tcpClient.Connected)
            {
                Debug.Log("连接服务器失败，请尝试重新连接!");
                MsgManager.Instance.SendMsg(MsgID.ConnectFailed,null);
            }
            else
            {
                tcpClient.EndConnect(ar);
                NetworkStream stream = tcpClient.GetStream();
                stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, tcpClient);
                Debug.Log("客户端连接成功");
                MsgManager.Instance.SendMsg(MsgID.ConnectSuccess,null);
            }
        }
        private void ReadResult(IAsyncResult ar)
        {
            tcpClient = (TcpClient)ar.AsyncState;
            if (tcpClient.Connected)
            {
                NetworkStream stream = tcpClient.GetStream();
                int recvLength = stream.EndRead(ar);
                if (recvLength <= 0)
                {
                    Debug.LogError("网络中断");
                     MsgManager.Instance.SendMsg(MsgID.ConnectAbort,null);
                    Close();
                    return;
                }
                byte[] recvBytes = new byte[recvLength];
                Array.Copy(recvBuffer, 0, recvBytes, 0, recvLength);
                dataPacker.UnPack(recvBytes);
                stream.BeginRead(recvBuffer, 0, recvBuffer.Length, ReadResult, tcpClient);
            }
        }
        public void Send(int msgID, byte[] bodyData)
        {
            if (tcpClient.Connected)
            {
                PackHead head = new PackHead();
                head.MsgID = (short)msgID;
                byte[] sendData = dataPacker.Packer(head, bodyData);

                tcpClient.GetStream().BeginWrite(sendData, 0, sendData.Length, SendResult, tcpClient);
                Debug.Log("发送消息ID:" + head.MsgID + " 大小:" + sendData.Length + "字节");
            }
            else
            {
                Debug.LogError("连接已断开，发送失败!");
            }
        }
        private void SendResult(IAsyncResult ar)
        {
            tcpClient = (TcpClient)ar.AsyncState;
            NetworkStream stream = tcpClient.GetStream();
            stream.EndWrite(ar);
            Debug.Log("数据发送成功");
        }
        public void Close()
        {
            if (tcpClient != null && tcpClient.Connected)
            {
                tcpClient.Close();
                Debug.Log("主动断开连接");
            }
        }
    }
}