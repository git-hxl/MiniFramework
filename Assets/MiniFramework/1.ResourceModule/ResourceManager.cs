using System;
using System.Collections;
using UnityEngine;

namespace MiniFramework
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public ResourceLoad.LoadType loadType;
        public ResourceUpdate resourceUpdate;
        public ResourceLoad resourceLoad;
        public event Action onLoadComplete;
        protected override void Awake()
        {
            base.Awake();
            resourceUpdate = new ResourceUpdate();
            resourceLoad = new ResourceLoad(loadType);
        }
        private IEnumerator Start()
        {
            if (loadType == ResourceLoad.LoadType.FromPersistentPath)
            {
                yield return resourceUpdate.DownloadConfig();
            }
            yield return resourceLoad.ReadConfig();
            Debug.Log("资源读取完成");
            onLoadComplete?.Invoke();
        }

        /// <summary>
        /// 自动选择从AB中加载还是直接加载
        /// </summary>
        /// <param name="assetPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (loadType == ResourceLoad.LoadType.FromEditor)
            {
                return LoadAssetFromEditor<T>(assetPath);
            }
#endif
            return LoadAssetFromAB<T>(assetPath);
        }

        /// <summary>
        /// 从AB包中加载资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T LoadAssetFromAB<T>(string assetPath) where T : UnityEngine.Object
        {
            foreach (var item in resourceLoad.bundles)
            {
                AssetBundle assetBundle = item.Value;
                if (assetBundle.Contains(assetPath))
                {
                    T asset = assetBundle.LoadAsset<T>(assetPath);
                    if (asset != null)
                    {
                        return asset;
                    }
                }
            }
            Debug.LogError("AB加载资源失败");
            return null;
        }

        /// <summary>
        /// 从编辑器目录下加载
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        private T LoadAssetFromEditor<T>(string assetPath) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                return asset;
            }
#endif
            Debug.LogError("Editor加载资源失败");
            return null;
        }
    }
}