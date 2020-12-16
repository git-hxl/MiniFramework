using UnityEngine;
namespace MiniFramework
{
    /// <summary>
    /// 可缓存对象标记组件
    /// </summary>
    public class Poolable : MonoBehaviour
    {
        public string poolKey;
        public bool IsRecycled;
    }
}
