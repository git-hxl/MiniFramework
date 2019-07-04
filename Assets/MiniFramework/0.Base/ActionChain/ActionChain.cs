using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    /// <summary>
    /// 链式调用类
    /// </summary>
    public static class ActionChain
    {
        public static void Delay<T>(this T selfBehaviour, float seconds, Action action) where T : MonoBehaviour
        {
            Sequence sequence = Pool<Sequence>.Instance.Allocate();
            sequence.Executer = selfBehaviour;
            (sequence as IChain).Delay(seconds);
            (sequence as IChain).Event(action);
            (sequence as IChain).Execute();
        }
        public static IChain Sequence<T>(this T selfBehaviour) where T : MonoBehaviour
        {
            Sequence sequence = Pool<Sequence>.Instance.Allocate();
            sequence.Executer = selfBehaviour;
            return sequence as IChain;
        }
    }
}