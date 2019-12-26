using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniFramework
{
    public class EventManager : Singleton<EventManager>
    {
        private EventManager() { }
        private Dictionary<string, List<Action<object[]>>> listeners = new Dictionary<string, List<Action<object[]>>>();
        public void ClearAll()
        {
            listeners.Clear();
        }
        public void Regist(string id, Action<object[]> listener)
        {
            if(listener == null)
            {
                return;
            }
            List<Action<object[]>> values;
            if(!listeners.TryGetValue(id,out values))
            {
                values = new List<Action<object[]>>();
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
        public void UnRegist(string id, Action<object[]> listener)
        {
            List<Action<object[]>> values;
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
            List<Action<object[]>> values;
            if (listeners.TryGetValue(id, out values))
            {
                values.Clear();
            }
        }
        public void Dispatch(string id, params object[] arg)
        {
            List<Action<object[]>> values;
            if (listeners.TryGetValue(id, out values))
            {
                for (int i = values.Count - 1; i >= 0; i--)
                {
                    Action<object[]> value = values[i];
                    if (!value.Target.Equals(null))
                    {
                        value(arg);
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