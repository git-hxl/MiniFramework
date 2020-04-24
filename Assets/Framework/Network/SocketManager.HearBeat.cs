using System;
using UnityEngine;

namespace MiniFramework.Network
{
    public partial class SocketManager
    {
        private class HearBeat
        {
            private float sendheartPackTime;
            private float recvHearPackTime;
            private float hearPackInterval = 2f;
            private SequenceNode sequenceNodeHeartPack;

            private MonoBehaviour monoBehaviour;
            private ISocket socket;
            public HearBeat(MonoBehaviour monoBehaviour, ISocket socket)
            {
                this.monoBehaviour = monoBehaviour;
                this.socket = socket;
            }
            public void Stop()
            {
                sequenceNodeHeartPack.Stop();
                MsgManager.Instance.UnRegist(MsgID.HearBeat, RecvHeartPack);
            }

            public void Start()
            {
                sendheartPackTime = 0f;
                recvHearPackTime = 0f;
                sequenceNodeHeartPack = monoBehaviour.Sequence().Repeat(-1, hearPackInterval, Send).Begin();
                MsgManager.Instance.Regist(MsgID.HearBeat, RecvHeartPack);
            }
            /// <summary>
            /// 发送心跳包
            /// </summary>
            private void Send()
            {
                if (sendheartPackTime - recvHearPackTime > hearPackInterval)
                {
                    Debug.LogError("心跳超时");
                    socket.Close();
                    return;
                }
                socket.Send(MsgID.HearBeat, null);
                sendheartPackTime = Time.time;
            }
            /// <summary>
            /// 接收心跳包
            /// </summary>
            /// <param name="data"></param>
            private void RecvHeartPack(byte[] data)
            {
                recvHearPackTime = Time.time;
            }
        }
    }
}
