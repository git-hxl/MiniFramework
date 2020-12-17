namespace MiniFramework
{
    public interface ISocket
    {

        void Launch();
        void Send(int msgID, byte[] data);
        void Close();
    }
}
