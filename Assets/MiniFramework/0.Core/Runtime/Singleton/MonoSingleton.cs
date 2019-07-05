using UnityEngine;
namespace MiniFramework
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected static T mInstance = null;
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = Object.FindObjectOfType<T>();
                    if (mInstance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name);
                        mInstance = obj.AddComponent<T>();
                        mInstance.OnSingletonInit();
                    }
                }
                return mInstance;
            }
        }
        protected virtual void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        protected virtual void OnSingletonInit() { }
        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
}