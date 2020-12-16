using System;
using System.Collections.Generic;
namespace MiniFramework
{
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<int, Action<object>> listeners = new Dictionary<int, Action<object>>();
        public void Regist(int id, Action<object> action)
        {
            Action<object> listener;
            if (!listeners.TryGetValue(id, out listener))
            {
                listeners.Add(id, listener);
            }
            listener += action;
            listeners[id] = listener;
        }
        public void UnRegist(int id, Action<object> action)
        {
            Action<object> listener;
            if (listeners.TryGetValue(id, out listener))
            {
                listener -= action;
                if (listener == null)
                    listeners.Remove(id);
                else
                    listeners[id] = listener;
            }
        }
        public void Dispatch(int id, object param)
        {
            Action<object> listener;
            if (listeners.TryGetValue(id, out listener))
            {
                listener.Invoke(param);
            }
        }

        public void Clear()
        {
            listeners.Clear();
        }
    }
}