using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniFramework
{
    public class MsgDispatcher : MonoSingleton<MsgDispatcher>
    {
        private class MsgData : IPoolable
        {
            public bool IsRecycled { get; set; }

            public void OnRecycled()
            {
                Recv = null;
                Objs = null;
                Action = null;
            }

            public object Recv;
            public object[] Objs;
            public Action<object[]> Action;
        }

        private Dictionary<string, List<MsgData>> listeners = new Dictionary<string, List<MsgData>>();
        private Queue<MsgData> idleListeners = new Queue<MsgData>();
        private static readonly object locker = new object();
        private void Update()
        {
            while (idleListeners.Count > 0)
            {
                MsgData msg = idleListeners.Dequeue();
                msg.Action(msg.Objs);
            }
        }
        public void Regist(object recv, int msgId, Action<object[]> action)
        {
            Regist(recv, msgId.ToString(), action);
        }
        public void Regist(object recv, string msgId, Action<object[]> action)
        {
            lock (locker)
            {
                if (action == null)
                {
                    return;
                }
                List<MsgData> value;
                if (!listeners.TryGetValue(msgId, out value))
                {
                    value = new List<MsgData>();
                    listeners.Add(msgId, value);
                }
                foreach (var item in value)
                {
                    if (item.Recv == recv && item.Action == action)
                    {
                        Debug.LogWarning(recv + "重复注册!");
                        return;
                    }
                }
                MsgData msg = Pool<MsgData>.Instance.Allocate();
                msg.Recv = recv;
                msg.Action = action;
                value.Add(msg);
            }
        }
        public void UnRegist(object recv, int msgId, Action<object[]> action)
        {
            UnRegist(recv, msgId.ToString(), action);
        }
        public void UnRegist(object recv, string msgId, Action<object[]> action)
        {
            lock (locker)
            {
                List<MsgData> value;
                if (listeners.TryGetValue(msgId, out value))
                {
                    for (int i = value.Count - 1; i >= 0; i--)
                    {
                        MsgData msg = value[i];
                        if (msg.Recv == recv && msg.Action == action)
                        {
                            value.Remove(msg);
                            Pool<MsgData>.Instance.Recycle(msg);
                            break;
                        }
                    }
                }
            }
        }

        public void Dispatch(int msgId, params object[] objs)
        {
            Dispatch(msgId.ToString(), objs);
        }
        public void Dispatch(string msgId, params object[] objs)
        {
            List<MsgData> value;
            if (listeners.TryGetValue(msgId, out value))
            {
                for (int i = value.Count - 1; i >= 0; i--)
                {
                    MsgData msg = value[i];
                    if (!msg.Recv.Equals(null))
                    {
                        msg.Objs = objs;
                        idleListeners.Enqueue(msg);
                    }
                    else
                    {
                        value.Remove(msg);
                        Pool<MsgData>.Instance.Recycle(msg);
                    }
                }
            }
            else
            {
                Debug.LogError("消息ID:" + msgId + "未注册");
            }
        }

    }
}