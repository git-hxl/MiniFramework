
using System;

namespace MiniFramework.Network
{
    public interface IUdpClient:ISocket
    {
        /// <summary>
        /// ip地址或者域名
        /// </summary>
        string Address { get; }
        int Port { get; }
        bool IsActive { get; }
        /// <summary>
        /// 设置Ip和端口号
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        void SetIPEndPoint(string address, int port);
        void Launch();
        void Broadcast(int msgID, byte[] data);
        event Action ConnectAbort;
    }
}
