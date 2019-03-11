using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniFramework
{
    public class MsgManager : MonoSingleton<MsgManager>
    {
        private class MsgHandler
        {
            public readonly object Receiver;
            public readonly Action<object> Callback;
            public MsgHandler(object receiver, Action<object> callback)
            {
                Receiver = receiver;
                Callback = callback;
            }
        }
        private class IdleMsg
        {
            public readonly object Param;
            public readonly Action<object> Callback;
            public IdleMsg(object param, Action<object> callback)
            {
                Param = param;
                Callback = callback;
            }
        }
        private readonly Dictionary<string, List<MsgHandler>> msgHandlerDict = new Dictionary<string, List<MsgHandler>>();

        protected override void OnSingletonInit() { }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="receiverSelf">接收方</param>
        /// <param name="msgName"></param>
        /// <param name="callback"></param>
        public void RegisterMsg(object receiverSelf, string msgName, Action<object> callback)
        {
            if (callback == null)
            {
                Debug.LogError("callback不能为null!");
                return;
            }
            //确保一个消息名只有一组消息列表
            if (!msgHandlerDict.ContainsKey(msgName))
            {
                msgHandlerDict[msgName] = new List<MsgHandler>();
            }
            var handlers = msgHandlerDict[msgName];
            foreach (var item in handlers)
            {
                //防止重复注册
                if (receiverSelf == item.Receiver && callback == item.Callback)
                {
                    return;
                }
            }
            handlers.Add(new MsgHandler(receiverSelf, callback));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="paramList"></param>
        public void SendMsg(string msgName, object param)
        {
            if (!msgHandlerDict.ContainsKey(msgName))
            {
                Debug.LogError("该消息名没有被注册:" + msgName);
                return;
            }
            var handlers = msgHandlerDict[msgName];
            //从后向前遍历，删除item后前面item的索引不会变化
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                MsgHandler handler = handlers[i];
                if (handler.Receiver != null)
                {
                    handler.Callback(param);
                }
                else
                {
                    handlers.Remove(handler);
                }
            }
        }

        /// <summary>
        /// 撤销注册消息
        /// </summary>
        /// <param name="receiverSelf"></param>
        /// <param name="msgName"></param>
        /// <param name="callback"></param>
        public void UnRegisterMsg(object receiver, string msgName, Action<object> callback)
        {
            if (callback == null)
            {
                return;
            }
            var handlers = msgHandlerDict[msgName];
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                var handler = handlers[i];
                if (handler.Receiver == receiver && handler.Callback == callback)
                {
                    handlers.Remove(handler);
                    break;
                }
            }
        }
    }


}