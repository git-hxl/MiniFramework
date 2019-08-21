using System;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class NetMsgManager : MonoSingleton<NetMsgManager>
    {
        private class Msg
        {
            public object Recv;
            public byte[] Param;
            public Action<byte[]> Callback;
        }

        private Dictionary<int, List<Msg>> msgDict = new Dictionary<int, List<Msg>>();
        private Queue<Msg> idleMsg = new Queue<Msg>();
        private static readonly object locker = new object();
        private void Update()
        {
            while (idleMsg.Count > 0)
            {
                Msg msg = idleMsg.Dequeue();
                msg.Callback(msg.Param);
            }
        }

        public void RegisterMsg(object recv, int msgId, Action<byte[]> callback)
        {
            if (callback == null)
            {
                Debug.LogError("callback不能为null!");
                return;
            }
            lock (locker)
            {
                if (!msgDict.ContainsKey(msgId))
                {
                    msgDict.Add(msgId, new List<Msg>());
                }
                List<Msg> msgs = msgDict[msgId];
                foreach (var item in msgs)
                {
                    if (item.Recv == recv && item.Callback == callback)
                    {
                        Debug.LogWarning(item.Callback.Target + "重复注册!");
                        return;
                    }
                }
                Msg msg = new Msg();
                msg.Recv = recv;
                msg.Callback = callback;
                msgs.Add(msg);
            }
        }

        public void SendMsg(int msgId, byte[] param)
        {
            lock (locker)
            {
                List<Msg> msgs = msgDict[msgId];
                for (int i = msgs.Count - 1; i >= 0; i--)
                {
                    Msg msg = msgs[i];
                    if (!msg.Recv.Equals(null))
                    {
                        msg.Param = param;
                        idleMsg.Enqueue(msg);
                    }
                    else
                    {
                        msgs.Remove(msg);
                    }
                }
            }
        }

        public void UnRegisterMsg(int msgId)
        {
            lock (locker)
            {
                msgDict[msgId].Clear();
            }
        }
        public void UnRegisterMsg(object recv, int msgId)
        {
            lock (locker)
            {
                List<Msg> msgs = msgDict[msgId];
                for (int i = msgs.Count - 1; i >= 0; i--)
                {
                    Msg msg = msgs[i];
                    if (msg.Recv == recv)
                    {
                        msgs.Remove(msg);
                    }
                }
            }
        }
        public void UnRegisterMsg(object recv, int msgId, Action<byte[]> callback)
        {
            lock (locker)
            {
                List<Msg> msgs = msgDict[msgId];
                for (int i = msgs.Count - 1; i >= 0; i--)
                {
                    Msg msg = msgs[i];
                    if (msg.Recv == recv && msg.Callback == callback)
                    {
                        msgs.Remove(msg);
                        break;
                    }
                }
            }
        }
    }
}
