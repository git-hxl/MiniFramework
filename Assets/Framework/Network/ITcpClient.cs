using System;

namespace MiniFramework.Network
{
    public interface ITcpClient:ISocket
    {
        /// <summary>
        /// ip地址或者域名
        /// </summary>
        string Address { get; }
        /// <summary>
        /// 端口号
        /// </summary>
        int Port { get; }
        /// <summary>
        /// 连接状态
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// 连接状态
        /// </summary>
        bool IsConnecting { get; }
        /// <summary>
        /// 连接超时
        /// </summary>
        float ConnectTimeout { get; set; }
        /// <summary>
        /// 设置Ip和端口号
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        void SetIPEndPoint(string address, int port);
        /// <summary>
        /// 连接
        /// </summary>
        void Connect();
 
        event Action ConnectSuccess;
        event Action ConnectFailed;
        event Action ConnectAbort;
    }
}
