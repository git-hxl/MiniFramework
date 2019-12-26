using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class UntilAction
    {
        public static Coroutine Excute(MonoBehaviour monoBehaviour, Func<bool> func, Action action)
        {
            return monoBehaviour.StartCoroutine(Until(func, action));
        }
        public static IEnumerator Until(Func<bool> func, Action action)
        {
            yield return new WaitUntil(func);
            action();
        }
    }
}