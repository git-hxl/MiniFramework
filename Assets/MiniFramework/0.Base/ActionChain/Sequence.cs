using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    /// <summary>
    /// 队列：控制所有节点的执行
    /// </summary>
    public class Sequence :IPoolable
    {
        public MonoBehaviour Executer { get; set; }
        public bool IsRecycled { get; set; }
        protected List<IEnumerator> Nodes = new List<IEnumerator>();
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
    }
}