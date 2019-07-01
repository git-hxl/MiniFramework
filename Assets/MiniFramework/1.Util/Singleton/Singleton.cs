using System;
using System.Reflection;
namespace MiniFramework
{
    public abstract class Singleton<T> where T:Singleton<T>
    {
        protected static T mInstance;
        static object mLock = new object();
        public static T Instance
        {
            get
            {
                lock(mLock)
                {
                    if(mInstance == null)
                    {
                        // 获取私有构造函数
                        var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                        // 获取无参构造函数
                        var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                        if (ctor == null)
                        {
                            throw new Exception("Non-Public Constructor() not found! in " + typeof(T));
                        }
                        mInstance = ctor.Invoke(null) as T;
                        mInstance.OnSingletonInit();
                    }
                }
                return mInstance;
            }
        }
        protected virtual void OnSingletonInit()
        {

        }
        public virtual void Dispose()
        {
            mInstance = null;
        }
    }

}