namespace MiniFramework.Network
{
    interface ISocketManager
    {
        ITcpClient GetTcpClient { get; }
    }
}
