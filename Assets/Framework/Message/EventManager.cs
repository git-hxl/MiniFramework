using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniFramework
{
    public class EventManager : Singleton<EventManager>
    {
        private EventManager() { }
        private Dictionary<string, List<Action>> listeners = new Dictionary<string, List<Action>>();
        public void ClearAll()
        {
            listeners.Clear();
        }
        public void Regist(string id, Action listener)
        {
            if(listener == null)
            {
                return;
            }
            List<Action> values;
            if(!listeners.TryGetValue(id,out values))
            {
                values = new List<Action>();
                listeners.Add(id, values);
            }
            foreach (var item in values)
            {
                if(item == listener)
                {
                    Debug.LogWarning("重复注册!");
                    return;
                }
            }
            values.Add(listener);
        }
        public void UnRegist(string id, Action listener)
        {
            List<Action> values;
            if (listeners.TryGetValue(id, out values))
            {
                for (int i = values.Count-1; i >=0; i--)
                {
                    if(values[i]==listener)
                    {
                        values.Remove(values[i]);
                        break;
                    }
                }
            }
        }
        public void Clear(string id)
        {
            List<Action> values;
            if (listeners.TryGetValue(id, out values))
            {
                values.Clear();
            }
        }
        public void Dispatch(string id)
        {
            List<Action> values;
            if (listeners.TryGetValue(id, out values))
            {
                for (int i = values.Count - 1; i >= 0; i--)
                {
                    Action value = values[i];
                    if (!value.Target.Equals(null))
                    {
                        value();
                    }
                    else
                    {
                        values.Remove(value);
                    }
                }
            }
            else
            {
                Debug.LogError("消息ID:" + id + "未注册");
            }
        }
    }
}