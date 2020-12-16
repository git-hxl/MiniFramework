using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public class ResourceLoad
    {
        public enum LoadType
        {
            FromEditor,//从编辑器目录直接读取
            FromStreamingAssets,//从流文件夹中读取
            FromPersistentPath,//从外部目录读取
        }
        private LoadType loadType;//加载方式
        private string resDir;//资源目录
        private string configPath;//配置文件路径
        public Dictionary<string, string> configInfo = new Dictionary<string, string>();
        public Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();
        public float progress;
        public ResourceLoad(LoadType loadType)
        {
            this.loadType = loadType;
            switch (loadType)
            {
                case LoadType.FromEditor:
                    break;
                case LoadType.FromStreamingAssets:
                    resDir = Application.streamingAssetsPath;
                    break;
                case LoadType.FromPersistentPath:
                    resDir = Application.persistentDataPath;
                    break;
            }
            resDir += "/" + PlatformUtil.GetPlatformName();
            configPath = resDir + "/config.txt";
        }

        /// <summary>
        /// 读取资源配置信息
        /// </summary>
        /// <returns></returns>
        public IEnumerator ReadConfig()
        {
            Debug.Log("读取资源配置信息...");
            //从streamingAsset中读取
            if (loadType == LoadType.FromStreamingAssets)
            {
                yield return FileUtil.ReadStreamingFile(configPath, (handler) =>
                {
                    if (handler != null && handler.data.Length > 0)
                    {
                        configInfo = FileUtil.TxtToDic(handler.text);
                    }
                    else
                    {
                        Debug.LogError("配置文件不存在");
                    }
                });
            }
            //从persistentPath中读取
            if (loadType == LoadType.FromPersistentPath)
            {
                if (File.Exists(configPath))
                {
                    configInfo = FileUtil.TxtToDic(File.ReadAllText(configPath));
                }
                else
                {
                    Debug.LogError("配置文件不存在");
                    yield break;
                }
            }

            yield return LoadAllBundles();
        }

        private IEnumerator LoadAllBundles()
        {
            progress = 0f;
            //过滤版本号信息
            foreach (var item in configInfo)
            {
                if (item.Key == "version")
                    continue;
                yield return LoadAssetBundle(item.Key);
                progress += 1f / (configInfo.Count - 1);
            }
        }
        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <param name="bundleName">bundle名称</param>
        /// <returns></returns>
        private IEnumerator LoadAssetBundle(string bundleName)
        {
            Debug.Log("加载AB资源：" + bundleName);
            string path = resDir + "/" + bundleName;
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            yield return request;
            if (request.assetBundle == null)
            {
                Debug.LogError(bundleName + "加载失败");
                yield break;
            }
            if (bundles.ContainsKey(bundleName))
            {
                Debug.LogError(bundleName + "资源重复");
                yield break;
            }
            bundles.Add(bundleName, request.assetBundle);
        }

        /// <summary>
        /// 卸载AssetBunde
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="unloadAllLoadedObjects"></param>
        public void UnloadAssetBundle(string bundleName, bool unloadAllLoadedObjects)
        {
            Debug.Log("卸载AB资源：" + bundleName);
            AssetBundle assetBundle = null;
            bundles.TryGetValue(bundleName, out assetBundle);
            if (assetBundle != null)
            {
                assetBundle.Unload(unloadAllLoadedObjects);
                bundles.Remove(bundleName);
            }
        }

    }
}