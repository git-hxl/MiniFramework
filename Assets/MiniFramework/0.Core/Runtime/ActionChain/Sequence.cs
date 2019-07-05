using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    /// <summary>
    /// 队列：控制所有节点的执行
    /// </summary>
    public class Sequence : IPoolable, IChain
    {
        public MonoBehaviour Executer { get; set; }
        public bool IsRecycled { get; set; }
        public List<IEnumerator> Nodes = new List<IEnumerator>();
        public Sequence() { }
        public Sequence(MonoBehaviour executer)
        {
            Executer = executer;
        }
        public void Execute()
        {
            Executer.StartCoroutine(SequenceCoroutine());
        }
        private IEnumerator SequenceCoroutine()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                yield return Executer.StartCoroutine(Nodes[i]);
            }
            Pool<Sequence>.Instance.Recycle(this);
        }
        public void OnRecycled()
        {
            Nodes.Clear();
            Executer = null;
        }

        public IChain Delay(float seconds)
        {
            Nodes.Add(delayCoroutine(seconds));
            return this;
        }
        private IEnumerator delayCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
        public IChain Event(Action action)
        {
            Nodes.Add(delayCoroutine(action));
            return this;
        }
        private IEnumerator delayCoroutine(Action action)
        {
            action();
            yield return null;
        }
        public IChain Until(Func<bool> func)
        {
            Nodes.Add(conditionCoroutine(func));
            return this;
        }
        private IEnumerator conditionCoroutine(Func<bool> condition)
        {
            yield return new WaitUntil(condition);
        }
    }
}