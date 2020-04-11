using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework
{
    public class RepeatNode
    {
        public IEnumerator Repeat(int times, float interval, Action action)
        {
            while (times > 0 || times < 0)
            {
                times--;
                yield return new WaitForSeconds(interval);
                action();
            }
        }
    }
}