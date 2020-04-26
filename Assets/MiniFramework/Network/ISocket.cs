 namespace MiniFramework.Network
{
    public interface ISocket
    {
        void Send(int msgID, byte[] data);
        void Close();
    }
}
