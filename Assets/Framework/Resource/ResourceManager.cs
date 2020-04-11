using System.Collections;
using UnityEngine;
namespace MiniFramework.Resource
{
    public sealed partial class ResourceManager : MonoSingleton<ResourceManager>, IResourceManager
    {
        public ReadType readType;

        private ResourceRead resourceRead;

        private ResourceUpdate resourceUpdate;

        public System.Action readSuccessCallback;

        public System.Action readFailCallback;

        public System.Action updateSuccessCallback;

        public System.Action updateFailCallback;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }
        private void Init()
        {
            resourceRead = new ResourceRead(this);
            resourceUpdate = new ResourceUpdate(this);
        }
        private IEnumerator Start()
        {
            yield return resourceUpdate.CheckConfig();
            yield return resourceRead.ReadAll();
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string name) where T : Object
        {
            Object asset = null;
            asset = LoadAssetFromAssetBundle<T>(name);
#if UNITY_EDITOR
            if (asset == null)
            {
                asset = LoadAssetFromEditor<T>(name);
            }
#endif
            if (asset == null)
            {
                Debug.LogError("加载资源对象失败:" + name);
            }
            return asset as T;
        }
        /// <summary>
        /// 从编辑器模式下加载
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T LoadAssetFromEditor<T>(string name) where T : Object
        {
#if UNITY_EDITOR
            T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(name);
            if (asset != null)
            {
                return asset;
            }
#endif
            return null;
        }
        /// <summary>
        /// 从AssetBundle中加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T LoadAssetFromAssetBundle<T>(string name) where T : Object
        {
            name = name.Substring(name.LastIndexOf('/') + 1);
            foreach (var item in resourceRead.CacheBundles)
            {
                if (item.Contains(name))
                {
                    T asset = item.LoadAsset<T>(name);
                    if (asset != null)
                    {
                        return asset;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 卸载所有资源
        /// </summary>
        public void UnLoadAll()
        {
            foreach (var item in resourceRead.CacheBundles)
            {
                item.Unload(false);
            }
            resourceRead.CacheBundles.Clear();
        }

    }
}