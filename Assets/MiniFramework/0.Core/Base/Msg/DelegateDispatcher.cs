using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniFramework
{
    public class DelegateDispatcher : Singleton<DelegateDispatcher>
    {
        private DelegateDispatcher() { }
        private Dictionary<string, Delegate> listeners = new Dictionary<string, Delegate>();
        private static readonly object locker = new object();
        public void ClearEventListner()
        {
            listeners.Clear();
        }
        private void AddDelegate(string id, Delegate listener)
        {
            lock (locker)
            {
                if (listener == null)
                {
                    return;
                }
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
        }
        private void RemoveDelegate(string id, Delegate listener)
        {
            lock (locker)
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
        }

        public void Regist(string id, Action listener)
        {
            AddDelegate(id, listener);
        }
        public void UnRegist(string id, Action listener)
        {
            RemoveDelegate(id, listener);
        }

        public void Dispatch(string id)
        {
            Delegate value;
            if (listeners.TryGetValue(id, out value) && value != null)
            {
                Action act = (Action)value;
                act();
            }
            else
            {
                Debug.LogError("消息ID:" + id + "未注册");
            }
        }
        public void Regist<T>(string id, Action<T> listener)
        {
            AddDelegate(id, listener);
        }
        public void UnRegist<T>(string id, Action<T> listener)
        {
            RemoveDelegate(id, listener);
        }
        public void Dispatch<T>(string id, T arg)
        {
            Delegate value;
            if (listeners.TryGetValue(id, out value))
            {
                Action<T> act = (Action<T>)value;
                act(arg);
            }
            else
            {
                Debug.LogError("消息ID:" + id + "未注册");
            }
        }
    }
}