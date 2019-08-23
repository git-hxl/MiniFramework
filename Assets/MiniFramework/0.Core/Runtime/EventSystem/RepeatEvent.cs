using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class RepeatEvent
    {
        public static Coroutine Excute(MonoBehaviour monoBehaviour, float interval, int times, Action action)
        {
            return monoBehaviour.StartCoroutine(Repeat(interval, times, action));
        }
        private static IEnumerator Repeat(float interval, int times, Action action)
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