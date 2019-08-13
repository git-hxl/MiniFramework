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
        /// <summary>
        /// 延迟执行逻辑
        /// </summary>
        /// <param name="selfBehaviour"></param>
        /// <param name="seconds"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Sequence Delay<T>(this T selfBehaviour, float seconds, Action action) where T : MonoBehaviour
        {
            Sequence sequence = Pool<Sequence>.Instance.Allocate();
            sequence.Executer = selfBehaviour;
            sequence.Delay(seconds);
            sequence.Event(action);
            return sequence.Execute();
        }
        /// <summary>
        /// 重新执行逻辑
        /// </summary>
        /// <param name="selfBehaviour"></param>
        /// <param name="interval">间隔</param>
        /// <param name="times">-1为无限次</param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Sequence Repeat<T>(this T selfBehaviour, float interval,int times, Action action) where T : MonoBehaviour
        {
            Sequence sequence = Pool<Sequence>.Instance.Allocate();
            sequence.Executer = selfBehaviour;
            sequence.Repeat(interval,times,action);
            return sequence.Execute();
        }
        public static Sequence Sequence<T>(this T selfBehaviour) where T : MonoBehaviour
        {
            Sequence sequence = Pool<Sequence>.Instance.Allocate();
            sequence.Executer = selfBehaviour;
            return sequence;
        }
    }
}