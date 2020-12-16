using System;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class MsgManager : MonoSingleton<MsgManager>
    {
        [Header("模拟消息延迟")]
        [Range(0, 1)]
        public float analogDelay;
        private float countTime;
        private class MsgData : IPoolable
        {
            public int msgID;
            public object data;
            public bool IsRecycled { get; set; }

            public void OnAllocated()
            {

            }
            public void OnRecycled()
            {

            }
        }
        private Queue<MsgData> msgQueue = new Queue<MsgData>();
        private void Update()
        {
            while (msgQueue.Count > 0)
            {
                countTime += Time.deltaTime;
                if (countTime < analogDelay)
                {
                    return;
                }
                countTime = 0;
                MsgData msg = msgQueue.Dequeue();
                EventManager.Instance.Dispatch(msg.msgID, msg.data);
                Pool<MsgData>.Instance.Recycle(msg);
            }
        }

        public void Enqueue(int msgID, object param)
        {
            MsgData queueMsg = Pool<MsgData>.Instance.Allocate();
            queueMsg.msgID = msgID;
            queueMsg.data = param;
            msgQueue.Enqueue(queueMsg);
        }
    }
}