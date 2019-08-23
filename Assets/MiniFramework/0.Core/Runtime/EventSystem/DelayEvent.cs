using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class DelayEvent
    {
        public static Coroutine Excute(MonoBehaviour monoBehaviour,float seconds,Action action)
        {
            return monoBehaviour.StartCoroutine(Delay(seconds,action));
        }
        private static IEnumerator Delay(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }
    }
}