using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class UntilEvent
    {
        public static Coroutine Excute(MonoBehaviour monoBehaviour, Func<bool> func, Action action)
        {
            return monoBehaviour.StartCoroutine(Until(func, action));
        }
        private static IEnumerator Until(Func<bool> func, Action action)
        {
            yield return new WaitUntil(func);
            action();
        }
    }
}