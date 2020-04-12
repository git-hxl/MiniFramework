using System;
using System.Runtime.InteropServices;
using UnityEngine;
namespace MiniFramework
{
    public class DataPacker
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public class PackHead
        {
            public short PackLength;
            public int MsgID;
        }
        public byte[] LeftBytes = new byte[0];
        //创建数据包
        public byte[] Packer(int msgID, byte[] bodyData)
        {
            PackHead head = new PackHead();
            head.MsgID = msgID;
            if (bodyData == null)
            {
                bodyData = new byte[0];
            }
            head.PackLength = (short)(Marshal.SizeOf(head) + bodyData.Length);
            byte[] headData = SerializeUtil.ToPtr(head);
            byte[] packData = new byte[headData.Length + bodyData.Length];
            Array.Copy(headData, packData, headData.Length);
            Array.Copy(bodyData, 0, packData, headData.Length, bodyData.Length);
            return packData;
        }
        //解析数据包
        public void UnPack(byte[] data)
        {
            PackHead head = new PackHead();
            int headLength = SerializeUtil.ToPtr(head).Length;

            byte[] totalData = new byte[LeftBytes.Length + data.Length];
            if (LeftBytes.Length > 0)
            {
                Array.Copy(LeftBytes, totalData, LeftBytes.Length);
            }
            Array.Copy(data, 0, totalData, LeftBytes.Length, data.Length);

            if (totalData.Length < headLength)
            {
                //消息头不足
                LeftBytes = totalData;
                return;
            }
            byte[] headData = new byte[headLength];
            Array.Copy(totalData, headData, headLength);
            head = SerializeUtil.FromPtr<PackHead>(headData);
            if (totalData.Length < head.PackLength)
            {
                //接受消息大小不足
                LeftBytes = totalData;
                return;
            }
            byte[] bodyData = new byte[head.PackLength - headLength];
            Array.Copy(totalData, headLength, bodyData, 0, bodyData.Length);
            //整包发送
            SendPack(head, bodyData);
            LeftBytes = new byte[totalData.Length - head.PackLength];
            if (LeftBytes.Length > 0)
            {
                //发生粘包
                Array.Copy(totalData, head.PackLength, LeftBytes, 0, LeftBytes.Length);
                if (LeftBytes.Length >= headLength)
                {
                    //采用递归进行再次拆包
                    UnPack(new byte[0]);
                }
            }
        }
        //发送消息体
        public void SendPack(PackHead head, byte[] bodyData)
        {
            Debug.Log("接收消息ID：" + head.MsgID);
            MsgManager.Instance.Dispatch(head.MsgID, bodyData);
        }
    }
}
