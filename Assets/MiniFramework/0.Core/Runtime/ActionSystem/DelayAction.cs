using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class DelayAction
    {
        public static Coroutine Excute(MonoBehaviour monoBehaviour,float seconds,Action action)
        {
            return monoBehaviour.StartCoroutine(Delay(seconds,action));
        }
        public static IEnumerator Delay(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }
    }
}