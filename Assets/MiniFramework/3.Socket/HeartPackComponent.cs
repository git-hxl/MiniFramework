using UnityEngine;
namespace MiniFramework
{

    public class HeartPackComponent : MonoBehaviour
    {
        public enum HeartType
        {
            TCP,
            UDP
        }
        public HeartType Type;
        public int Timeout = 5;
        public int HeartPackLatency;

        private float sendTime;
        private float recvTime;

        // Use this for initialization
        void Start()
        {
            NetMsgDispatcher.Instance.Regist(NetMsgID.HeartPack, (data) =>
            {
                //接收心跳包 
                recvTime = Time.time;
                HeartPackLatency = (int)((recvTime - sendTime) * 1000);
            });

            switch(Type)
            {
                case HeartType.TCP:
                    TcpHeart();
                    break;
                case HeartType.UDP:
                    UdpHeart();
                    break;
            }
        }

        void TcpHeart()
        {
            RepeatAction.Excute(this, Timeout, -1, () =>
            {
                if (!TcpClientComponent.Instance.IsConnected)
                {
                    sendTime = 0;
                    recvTime = 0;
                    return;
                }
                if (recvTime - sendTime < 0)
                {
                    Debug.LogError("心跳包接收超时!");
                    TcpClientComponent.Instance.Close();
                    if (TcpClientComponent.Instance.ConnectAbort != null)
                    {
                        TcpClientComponent.Instance.ConnectAbort();
                    }
                    return;
                }
                TcpClientComponent.Instance.Send(NetMsgID.HeartPack, null);
                sendTime = Time.time;
            });
        }

        void UdpHeart()
        {
            RepeatAction.Excute(this, Timeout, -1, () =>
            {
                if (!UdpClientComponent.Instance.IsActive)
                {
                    sendTime = 0;
                    recvTime = 0;
                    return;
                }
                if (recvTime - sendTime < 0)
                {
                    Debug.LogError("心跳包接收超时!");
                    UdpClientComponent.Instance.Close();
                    if (UdpClientComponent.Instance.ConnectAbort != null)
                    {
                        UdpClientComponent.Instance.ConnectAbort();
                    }
                    return;
                }
                UdpClientComponent.Instance.Send(NetMsgID.HeartPack, null);
                sendTime = Time.time;
            });
        }
    }

}