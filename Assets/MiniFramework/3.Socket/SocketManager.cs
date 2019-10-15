namespace MiniFramework
{
    public class SocketManager : Singleton<SocketManager>
    {
        private SocketManager() { }

        private MiniTcpClient miniTcpClient;
        private MiniTcpServer miniTcpServer;
        private MiniUdpClient miniUdpClient;
        public MiniTcpClient MiniTcpClient
        {
            get
            {
                if (miniTcpClient == null)
                {
                    miniTcpClient = new MiniTcpClient();
                }
                return miniTcpClient;
            }
        }

        public MiniTcpServer MiniTcpServer
        {
            get
            {
                if (miniTcpServer == null)
                {
                    miniTcpServer = new MiniTcpServer();
                }
                return miniTcpServer;
            }
        }

        public MiniUdpClient MiniUdpClient
        {
            get
            {
                if (miniUdpClient == null)
                {
                    miniUdpClient = new MiniUdpClient();
                }
                return miniUdpClient;
            }
        }
    }
}