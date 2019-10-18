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
        private void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this as T;
                Init();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public virtual void Init()
        {

        }
        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
}