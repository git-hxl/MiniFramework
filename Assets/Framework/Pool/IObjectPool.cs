using UnityEngine;

namespace MiniFramework.Pool
{
    public interface IObjectPool
    {
        int Count(string name);
        /// <summary>
        /// 分配
        /// </summary>
        /// <returns></returns>
        GameObject Allocate(string name);
        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Recycle(GameObject obj);
    }
}