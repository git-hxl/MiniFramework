using System;
using System.Collections.Generic;
namespace MiniFramework
{
    public class EventDispatcher
    {
        public static EventDispatcher GameEvent = new EventDispatcher();
        private Dictionary<string, Delegate> listeners = new Dictionary<string, Delegate>();
        public void ClearEventListner()
        {
            listeners.Clear();
        }
        private void AddDelegate(string id, Delegate listener)
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
        private void RemoveDelegate(string id, Delegate listener)
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

        public void Regist(string id, Action listener)
        {
            AddDelegate(id, listener);
        }
        public void UnRegist(string id, Action listener)
        {
            RemoveDelegate(id, listener);
        }

        public void DispatchEvent(string id)
        {
            Delegate value;
            if (listeners.TryGetValue(id, out value) && value != null)
            {
                Action act = (Action)value;
                act();
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
        public void DispatchEvent<T>(string id, T arg)
        {
            Delegate value;
            if (listeners.TryGetValue(id, out value) && value != null)
            {
                Action<T> act = (Action<T>)value;
                act(arg);
            }
        }
    }
}