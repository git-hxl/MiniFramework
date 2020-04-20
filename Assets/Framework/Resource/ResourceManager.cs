using System.Collections;
using UnityEngine;
using System;
using MiniFramework.WebRequest;

namespace MiniFramework.Resource
{
    public sealed partial class ResourceManager : MonoSingleton<ResourceManager>, IResourceManager
    {
        public ReadType readType;
        private ResourceUpdate resourceUpdate;
        private ResourceRead resourceRead;

        /// <summary>
        /// 获取更新信息
        /// </summary>
        /// <returns></returns>
        public IResourceUpdate GetResourceUpdate
        {
            get
            {
                return resourceUpdate;
            }
            
        }
        /// <summary>
        /// 获取加载信息
        /// </summary>
        /// <returns></returns>
        public IResourceRead GetResourceRead
        {
            get
            {
                return resourceRead;
            }
        }

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
        private void Start()
        {
            switch (readType)
            {
                case ReadType.None: break;
                case ReadType.FromStreamingAssets:
                    StartCoroutine(resourceRead.ReadAll()); break;
                case ReadType.FromPersistentPath:
                    resourceUpdate.onUpdateCompleted += () => StartCoroutine(resourceRead.ReadAll());
                    resourceUpdate.DownloadConfig();
                    break;
            }

        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string name) where T : UnityEngine.Object
        {
            UnityEngine.Object asset = null;
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
        public T LoadAssetFromEditor<T>(string name) where T : UnityEngine.Object
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
        public T LoadAssetFromAssetBundle<T>(string name) where T : UnityEngine.Object
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