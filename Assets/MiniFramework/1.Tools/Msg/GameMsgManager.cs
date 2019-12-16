using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniFramework
{
    public class GameMsgManager : Singleton<GameMsgManager>
    {
        private GameMsgManager() { }
        private Dictionary<string, List<Delegate>> listeners = new Dictionary<string, List<Delegate>>();
        public void ClearEventListner()
        {
            listeners.Clear();
        }
       
        public void Regist<T>(string id, Action<T> listener)
        {
            if(listener == null)
            {
                return;
            }
            List<Delegate> values;
            if(!listeners.TryGetValue(id,out values))
            {
                values = new List<Delegate>();
                listeners.Add(id, values);
            }
            foreach (var item in values)
            {
                if(item == (Delegate)listener)
                {
                    Debug.LogWarning("重复注册!");
                    return;
                }
            }
            values.Add(listener);
        }
        public void UnRegist<T>(string id, Action<T> listener)
        {
            List<Delegate> values;
            if (listeners.TryGetValue(id, out values))
            {
                for (int i = values.Count-1; i >=0; i--)
                {
                    if(values[i]==(Delegate)listener)
                    {
                        values.Remove(values[i]);
                        break;
                    }
                }
            }
        }
        public void Dispatch<T>(string id, T arg)
        {
            List<Delegate> values;
            if (listeners.TryGetValue(id, out values))
            {
                for (int i = values.Count - 1; i >= 0; i--)
                {
                    Delegate value = values[i];
                    if (!value.Target.Equals(null))
                    {
                        if(value.GetType() == typeof(Action<T>))
                        {
                            Action<T> act = (Action<T>)value;
                            act(arg);
                        }
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