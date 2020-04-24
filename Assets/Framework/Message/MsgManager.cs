using System;
using System.Collections.Generic;
using UnityEngine;
using MiniFramework.Pool;
namespace MiniFramework
{
    public class MsgManager : MonoSingleton<MsgManager>
    {
        private class MsgData : IPoolable
        {
            public bool IsRecycled { get; set; }

            public void OnRecycled()
            {
                data = null;
                Action = null;
            }

            public void OnAllocated()
            {

            }

            public byte[] data;
            public Action<byte[]> Action;
        }
        private Dictionary<int, List<MsgData>> msgDict = new Dictionary<int, List<MsgData>>();
        private Queue<MsgData> msgQueue = new Queue<MsgData>();
        private static readonly object locker = new object();
        private void Update()
        {
            lock (locker)
            {
                while (msgQueue.Count > 0)
                {
                    MsgData msg = msgQueue.Dequeue();
                    if (!msg.Action.Target.Equals(null))
                    {
                        msg.Action(msg.data);
                    }
                    Pool<MsgData>.Instance.Recycle(msg);
                }
            }
        }
        public void Regist(int msgId, Action<byte[]> action)
        {
            lock (locker)
            {
                if (action == null)
                {
                    return;
                }
                List<MsgData> value;
                if (!msgDict.TryGetValue(msgId, out value))
                {
                    value = new List<MsgData>();
                    msgDict.Add(msgId, value);
                }
                foreach (var item in value)
                {
                    if (item.Action == action)
                    {
                        Debug.LogWarning("重复注册!");
                        return;
                    }
                }
                MsgData msg = Pool<MsgData>.Instance.Allocate();
                msg.Action = action;
                value.Add(msg);
            }
        }
        public void UnRegist(int msgId, Action<byte[]> action)
        {
            lock (locker)
            {
                List<MsgData> values;
                if (msgDict.TryGetValue(msgId, out values))
                {
                    for (int i = values.Count - 1; i >= 0; i--)
                    {
                        MsgData msg = values[i];
                        if (msg.Action == action)
                        {
                            values.Remove(msg);
                            Pool<MsgData>.Instance.Recycle(msg);
                            break;
                        }
                    }
                }
            }
        }
        public void Clear(int msgId)
        {
            lock (locker)
            {
                List<MsgData> values;
                if (msgDict.TryGetValue(msgId, out values))
                {
                    values.Clear();
                }
            }
        }
        public void Dispatch(int msgId, byte[] data)
        {
            lock (locker)
            {
                List<MsgData> value;
                if (msgDict.TryGetValue(msgId, out value))
                {
                    for (int i = value.Count - 1; i >= 0; i--)
                    {
                        if (!value[i].Action.Target.Equals(null))
                        {
                            MsgData queueMsg = Pool<MsgData>.Instance.Allocate();
                            queueMsg.Action = value[i].Action;
                            queueMsg.data = data;
                            msgQueue.Enqueue(queueMsg);
                        }
                        else
                        {
                            value.Remove(value[i]);
                            Pool<MsgData>.Instance.Recycle(value[i]);
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
}