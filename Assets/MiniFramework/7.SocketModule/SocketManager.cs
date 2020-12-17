using UnityEngine;

namespace MiniFramework
{
    public class SocketManager : MonoSingleton<SocketManager>
    {
        public enum SocketType
        {
            TcpClient,
            TcpServer,
            UdpClient,
        }
        public SocketType socketType;
        public string address;
        public int port;

        public bool openHearBeat;
        public ISocket socket;

        private HearBeatComponent hearBeat;
        private void Start()
        {
            switch (socketType)
            {
                case SocketType.TcpClient: socket = new TcpClient(address, port); break;
                case SocketType.TcpServer: socket = new TcpServer(port); break;
                case SocketType.UdpClient: socket = new UdpClient(address, port); break;
            }
            socket.Launch();
            if (openHearBeat)
            {
                hearBeat = gameObject.AddComponent<HearBeatComponent>();
                hearBeat.Init(socket);
            }
            EventManager.Instance.Regist(MsgID.ConnectSuccess, ConnectSuccess);
            EventManager.Instance.Regist(MsgID.ConnectFailed, ConnectFailed);
            Application.runInBackground = true;
        }

        [ContextMenu("重启")]
        public void Launch()
        {
            socket.Launch();
        }
        void ConnectSuccess(object data)
        {
            socket.Send(MsgID.Ping, null);
        }

        void ConnectFailed(object data)
        {

        }
    }

}
