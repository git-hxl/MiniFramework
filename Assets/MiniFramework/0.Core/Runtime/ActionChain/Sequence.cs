using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    /// <summary>
    /// 队列：控制所有节点的执行
    /// </summary>
    public class Sequence : IPoolable
    {
        public MonoBehaviour Executer { get; set; }
        public bool IsRecycled { get; set; }
        public List<IEnumerator> Nodes = new List<IEnumerator>();
        private Coroutine coroutine;
        public Sequence() { }
        public Sequence(MonoBehaviour executer)
        {
            Executer = executer;
        }
        public Sequence Execute()
        {
            coroutine = Executer.StartCoroutine(SequenceCoroutine());
            return this;
        }
        private IEnumerator SequenceCoroutine()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                yield return Nodes[i];
            }
            Pool<Sequence>.Instance.Recycle(this);
        }
        public void OnRecycled()
        {
            Nodes.Clear();
            Executer = null;
        }
        public void Close()
        {
            if (Executer != null)
            {
                Executer.StopCoroutine(coroutine);
            }
            Pool<Sequence>.Instance.Recycle(this);
        }

        public Sequence Delay(float seconds)
        {
            Nodes.Add(delayCoroutine(seconds));
            return this;
        }
        private IEnumerator delayCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
        public Sequence Event(Action action)
        {
            Nodes.Add(eventCoroutine(action));
            return this;
        }
        private IEnumerator eventCoroutine(Action action)
        {
            action();
            yield return null;
        }
        public Sequence Until(Func<bool> func)
        {
            Nodes.Add(conditionCoroutine(func));
            return this;
        }
        private IEnumerator conditionCoroutine(Func<bool> condition)
        {
            yield return new WaitUntil(condition);
        }

        public Sequence Repeat(float interval, int times, Action action)
        {
            Nodes.Add(repeatCoroutine(interval, times, action));
            return this;
        }
        private IEnumerator repeatCoroutine(float interval, int times, Action action)
        {
            while (times > 0 || times < 0)
            {
                yield return new WaitForSeconds(interval);
                action();
                times--;
            }
        }
    }
}