using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class SequenceNode
    {
        private MonoBehaviour mono;
        private List<IEnumerator> coroutines = new List<IEnumerator>();
        private Coroutine coroutine;
        public SequenceNode(MonoBehaviour mono)
        {
            this.mono = mono;
        }
        public void Append(float delay)
        {
            DelayNode delayNode = new DelayNode();
            coroutines.Add(delayNode.Delay(delay));
        }
        public void Append(Func<bool> condition)
        {
            UntilNode untilNode = new UntilNode();
            coroutines.Add(untilNode.Until(condition));
        }
        public void Append(Action action)
        {
            EventNode eventNode = new EventNode();
            coroutines.Add(eventNode.Event(action));
        }
        public void Append(int times, float interval, Action action)
        {
            RepeatNode repeatNode = new RepeatNode();
            coroutines.Add(repeatNode.Repeat(times, interval, action));
        }
        public SequenceNode Begin()
        {
            coroutine = mono.StartCoroutine(begin());
            return this;
        }

        IEnumerator begin()
        {
            foreach (var cor in coroutines)
            {
                yield return cor;
            }
        }

        public void Stop()
        {
            if(coroutine!=null)
            {
                mono.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
