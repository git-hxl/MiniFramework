using UnityEngine;
namespace MiniFramework.Pool
{
    /// <summary>
    /// 可缓存对象标记组件
    /// </summary>
    class ObjectPoolable : MonoBehaviour
    {
        /// <summary>
        /// 用作与缓存的key
        /// </summary>
        public string PoolKey { get; set; }

        public bool IsRecycled { get; set; }
    }
}
