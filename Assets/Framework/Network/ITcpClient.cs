using System;

namespace MiniFramework.Network
{
    public interface ITcpClient
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
        /// 初始化
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        void Init(string address, int port);
        /// <summary>
        /// 连接
        /// </summary>
        void Connect();
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="data"></param>
        void Send(int msgID, byte[] data);
        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();
        event Action ConnectSuccess;
        event Action ConnectFailed;
        event Action ConnectAbort;
    }
}
