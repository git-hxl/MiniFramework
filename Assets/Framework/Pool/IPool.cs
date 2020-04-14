namespace MiniFramework.Pool
{
    public interface IPool<T> 
    {
        /// <summary>
        /// 缓存对象数量
        /// </summary>
        int Count { get;}
        /// <summary>
        /// 分配
        /// </summary>
        /// <returns></returns>
        T Allocate();
        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Recycle(T obj);
    }
}