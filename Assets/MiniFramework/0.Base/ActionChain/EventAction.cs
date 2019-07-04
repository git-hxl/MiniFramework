using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class EventAction : IChain
    {
        public override IChain Event(Action action)
        {
            Nodes.Add(delayCoroutine(action));
            return this;
        }
        private IEnumerator delayCoroutine(Action action)
        {
            action();
            yield return null;
        }
    }
}