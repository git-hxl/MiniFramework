using System;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class MsgDispatcher : MonoSingleton<MsgDispatcher>
    {
        private class Msg<T>
        {
            public T arg;
            public Delegate listener;
        }
        private Dictionary<int, Delegate> listeners = new Dictionary<int, Delegate>();
        private Queue<Msg> idleMsg = new Queue<Msg>();
        private static readonly object locker = new object();
        private void Update()
        {
            while (idleMsg.Count > 0)
            {
                Msg msg = idleMsg.Dequeue();

                msg.listener(msg.arg);
            }
        }
        public void ClearEventListner()
        {
            listeners.Clear();
        }
        private void AddDelegate(int id, Delegate listener)
        {
            Delegate value;
            if (!listeners.TryGetValue(id, out value))
            {
                listeners.Add(id, listener);
            }
            else
            {
                value = Delegate.Combine(value, listener);
                listeners[id] = value;
            }
        }
        private void RemoveDelegate(int id, Delegate listener)
        {
            Delegate value;
            if (listeners.TryGetValue(id, out value))
            {
                if (value != null)
                {
                    value = Delegate.Remove(value, listener);
                    listeners[id] = value;
                }
            }
        }
         public void Regist<T>(int id, Action<T> listener)
        {
            AddDelegate(id, listener);
        }
        public void UnRegist<T>(int id, Action<T> listener)
        {
            RemoveDelegate(id, listener);
        }

        public void DispatchEvent<T>(int id, T arg)
        {
            Delegate value;
            if (listeners.TryGetValue(id, out value) && value != null)
            {
                Msg<T> msg = new Msg<T>();
                msg.listener = value;
                msg.arg = arg;
                idleMsg.Enqueue(msg);
            }
        }

    }
}
