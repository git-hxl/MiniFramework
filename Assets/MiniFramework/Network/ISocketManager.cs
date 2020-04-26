namespace MiniFramework.Network
{
    interface ISocketManager
    {
        ITcpClient GetTcpClient { get; }
        ITcpServer GetTcpServer { get; }

        IUdpClient GetUdpClient { get; }
    }
}
