namespace MiniFramework.Network
{
    public partial class SocketManager : MonoSingleton<SocketManager>, ISocketManager
    {
        private TcpClient tcpClient;
        public ITcpClient GetTcpClient
        {
            get
            {
                if (tcpClient == null)
                {
                    tcpClient = new TcpClient(this);
                }
                return tcpClient;
            }
        }
        private TcpServer tcpServer;
        public ITcpServer GetTcpServer
        {
            get
            {
                if (tcpServer == null)
                {
                    tcpServer = new TcpServer();
                }
                return tcpServer;
            }
        }
        private UdpClient udpClient;
        public IUdpClient GetUdpClient
        {
            get
            {
                if (udpClient == null)
                {
                    udpClient = new UdpClient();
                }
                return udpClient;
            }
        }


        private void OnDestroy()
        {
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }
    }
}