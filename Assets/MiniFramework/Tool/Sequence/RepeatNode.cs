using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class RepeatNode
    {
        public IEnumerator Repeat(int times, float interval, Action action, Func<bool> condition = null)
        {
            while (times > 0 || times < 0)
            {
                if (condition != null && !condition())
                {
                    yield break;
                }
                times--;
                yield return new WaitForSeconds(interval);
                action();
            }
        }
    }
}