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
        public int CheckInterval = 5;
        public float PackLatency;

        private float sendTime;
        private float recvTime;

        // Use this for initialization
        void Start()
        {
            NetMsgDispatcher.Instance.Regist(NetMsgID.HeartPack, (data) =>
            {
                //接收心跳包 
                recvTime = Time.time;
                PackLatency = recvTime - sendTime;
            });

            switch (Type)
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
            RepeatAction.Excute(this, CheckInterval, -1, () =>
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
                    TcpClientComponent.Instance.ConnectAbort.Invoke();
                    return;
                }
                TcpClientComponent.Instance.Send(NetMsgID.HeartPack, null);
                sendTime = Time.time;
            });
        }

        void UdpHeart()
        {
            RepeatAction.Excute(this, CheckInterval, -1, () =>
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
                    UdpClientComponent.Instance.ConnectAbort.Invoke();
                    return;
                }
                UdpClientComponent.Instance.Send(NetMsgID.HeartPack, null);
                sendTime = Time.time;
            });
        }
    }

}