using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    /// <summary>
    /// 队列：控制所有节点的执行
    /// </summary>
    public class Sequence : IPoolable, IDelay, IUntil, IEvent
    {
        public MonoBehaviour Executer { get; set; }
        public bool IsRecycled { get; set; }
        private List<IEnumerator> nodes = new List<IEnumerator>();
        public Sequence() { }
        public Sequence(MonoBehaviour executer)
        {
            Executer = executer;
        }
        public Sequence Delay(float seconds)
        {
            nodes.Add(delayCoroutine(seconds));
            return this;
        }
        public Sequence Event(Action handler)
        {
            nodes.Add(actionCoroutine(handler));
            return this;
        }
        public Sequence Until(Func<bool> condition)
        {
            nodes.Add(conditionCoroutine(condition));
            return this;
        }

        public void Execute()
        {
            Executer.StartCoroutine(SequenceCoroutine());
        }
        private IEnumerator SequenceCoroutine()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                yield return Executer.StartCoroutine(nodes[i]);
            }
            Pool<Sequence>.Instance.Recycle(this);
        }
        private IEnumerator delayCoroutine(float time)
        {
            float cur = 0;
            while ((cur += Time.deltaTime) < time)
            {
                yield return null;
            }
        }
        private IEnumerator actionCoroutine(Action handler)
        {
            handler();
            yield return null;
        }
        private IEnumerator conditionCoroutine(Func<bool> condition)
        {
            while (!condition())
            {
                yield return null;
            }
        }
        public void OnRecycled()
        {
            nodes.Clear();
            Executer = null;
        }
    }
}