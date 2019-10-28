using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class ActionChain
    {
        public ActionChain(MonoBehaviour mono)
        {
            this.mono = mono;
        }
        private List<IEnumerator> iEnumerators = new List<IEnumerator>();
        private MonoBehaviour mono;
        private Coroutine curCoroutine;
        public ActionChain Delay(float seconds, Action action)
        {
            iEnumerators.Add(DelayAction.Delay(seconds, action));
            return this;
        }

        public ActionChain Until(Func<bool> func, Action action)
        {
            iEnumerators.Add(UntilAction.Until(func, action));
            return this;
        }
        public ActionChain Repeat(float interval, int times, Action action)
        {
            iEnumerators.Add(RepeatAction.Repeat(interval,times,action));
            return this;
        }
        public void Excute()
        {
            curCoroutine = mono.StartCoroutine(ActionChainCoroutine());
        }
        public void Stop()
        {
            if (curCoroutine != null)
            {
                mono.StopCoroutine(curCoroutine);
                curCoroutine = null;
            }
        }
        private IEnumerator ActionChainCoroutine()
        {
            foreach (var iEnumerator in iEnumerators)
            {
                yield return iEnumerator;
            }
        }
    }
}