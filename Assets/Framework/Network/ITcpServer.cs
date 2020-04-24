namespace MiniFramework.Network
{
    public interface ITcpServer:ISocket
    {
        int Port { get; }
        int MaxConnections { get; }
        int MaxBufferSize { get; }
        bool IsActive { get; }
        void SetPort(int port);
        void Launch();
    }
}