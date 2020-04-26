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
                    }
                }
                return mInstance;
            }
        }
        protected virtual void Awake()
        {
            if (mInstance != null)
            {
                Destroy(gameObject);
                return;
            }
            mInstance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
}