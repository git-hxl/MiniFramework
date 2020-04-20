
using System;

namespace MiniFramework.Network
{
    interface IUdpClient
    {
        /// <summary>
        /// ip地址或者域名
        /// </summary>
        string Address { get; }
        int Port { get; }
        bool IsActive { get; }

        event Action ConnectAbort;

        void Init(string address, int port);
        void Launch();
        void Broadcast(int msgID, byte[] data);

        void Send(int msgID, byte[] data);

        void Close();
    }
}
