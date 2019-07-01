using System;
using System.Collections.Generic;
namespace MiniFramework
{
    /// <summary>
    /// 用于非GameObject对象的对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> : Singleton<Pool<T>> where T : IPoolable, new()
    {
        private Pool() { }
        private uint mMaxCount=10;//缓存池最大个数

        protected readonly Stack<T> mCacheStack = new Stack<T>();//缓存池
        //当前缓存池中对象个数
        public int CurCount
        {
            get { return mCacheStack.Count; }
        }
        /// <summary>
        /// 初始化缓存池
        /// </summary>
        /// <param name="maxCount">最大数量</param>
        /// <param name="minCount">最小数量</param>
        public void Init(uint maxCount, uint minCount)
        {
            mMaxCount = maxCount;
            uint initCount = Math.Min(maxCount, minCount);
            if (CurCount < initCount)
            {
                for (int i = CurCount; i < initCount; i++)
                {
                    Recycle(new T());
                }
            }
        }
        /// <summary>
        /// 分配
        /// </summary>
        /// <returns></returns>
        public T Allocate()
        {
            var result = mCacheStack.Count == 0 ? new T() : mCacheStack.Pop();
            result.IsRecycled = false;
            return result;
        }
        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Recycle(T obj)
        {
            if (obj == null||obj.IsRecycled)
            {
                return false;
            }
            if (mCacheStack.Count >= mMaxCount)
            {
                return false;
            }
            obj.IsRecycled = true;
            obj.OnRecycled();
            mCacheStack.Push(obj);
            return true;
        }
    }
}