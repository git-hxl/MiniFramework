using System;
using System.Runtime.InteropServices;
using UnityEngine;
namespace MiniFramework
{
    public class DataPacker
    {
        public byte[] OtherBytes = new byte[0];
        //创建数据包
        public byte[] Packer(PackHead head, byte[] bodyData)
        {
            head.PackLength  = (short)(Marshal.SizeOf(head)+bodyData.Length);
            byte[] headData = Binary.SerializeByMarshal(head);
            byte[] packData = new byte[headData.Length + bodyData.Length];
            Array.Copy(headData, packData, headData.Length);
            Array.Copy(bodyData, 0, packData, headData.Length, bodyData.Length);
            return packData;
        }
        //解析数据包
        public void UnPack(byte[] data)
        {
            PackHead head = new PackHead();
            int headLength = Binary.SerializeByMarshal(head).Length;

            byte[] totalData = new byte[OtherBytes.Length + data.Length];
            if(OtherBytes.Length>0){
                Array.Copy(OtherBytes, totalData, OtherBytes.Length);
            }           
            Array.Copy(data, 0, totalData, OtherBytes.Length, data.Length);

            if (totalData.Length < headLength)
            {
                //消息头不足
                OtherBytes = totalData;
                Debug.LogError("消息头长度不足！");
                return;
            }
            byte[] headData = new byte[headLength];
            Array.Copy(totalData, headData, headLength);
            head = Binary.DeserializeByMarshal<PackHead>(headData);
            if (totalData.Length < head.PackLength)
            {
                //消息体不足
                OtherBytes = totalData;
                return;
            }
            byte[] bodyData = new byte[head.PackLength-headLength];
            Array.Copy(totalData, headLength, bodyData, 0, bodyData.Length);
            //整包发送
            SendPack(head, bodyData);

            int leftLength = totalData.Length - head.PackLength;
            
            if (leftLength > 0)
            {
                //发生粘包
                OtherBytes = new byte[leftLength];
                Array.Copy(totalData, head.PackLength, OtherBytes, 0, leftLength);
                if (leftLength >= headLength)
                {
                    //采用递归进行再次拆包
                    UnPack(new byte[0]);
                }
            }
        }
        //发送消息体
        public void SendPack(PackHead head, byte[] bodyData)
        {
            Debug.Log("接收到消息ID："+head.MsgID);
            MsgManager.Instance.SendMsg(head.MsgID.ToString(), bodyData);
        }
    }
}
