namespace MiniFramework.Network
{
    public interface ITcpServer
    {
        int Port { get; }
        int MaxConnections { get; }
        int MaxBufferSize { get; }
        bool IsActive { get; }
        void Init(int port);
        void Launch();

        void Send(int msgID, byte[] data);
        void Close();
    }
}