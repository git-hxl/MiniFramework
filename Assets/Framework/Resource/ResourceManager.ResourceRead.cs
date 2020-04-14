using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework.Resource
{
    public partial class ResourceManager
    {
        private class ResourceRead : IResourceRead
        {
            private string assetBundlePath;

            private Dictionary<string, string> localConfig;

            private List<AssetBundle> cachedBundles;

            public event Action onReadCompleted;

            public event Action onReadError;

            public List<AssetBundle> CacheBundles
            {
                get { return cachedBundles; }
            }

            private ResourceManager resourceManager;
            public ResourceRead(ResourceManager resourceManager)
            {
                this.resourceManager = resourceManager;
                cachedBundles = new List<AssetBundle>();
                localConfig = new Dictionary<string, string>();
            }

            /// <summary>
            /// 读取AssetBundle
            /// </summary>
            /// <returns></returns>
            public IEnumerator ReadAll()
            {
                if (resourceManager.readType == ReadType.None)
                {
                    yield break;
                }
                //从streamingAsset中读取
                if (resourceManager.readType == ReadType.FromStreamingAssets)
                {
                    assetBundlePath = Application.streamingAssetsPath + "/" + PlatformUtil.GetPlatformName();
                    yield return FileUtil.ReadStreamingFile(assetBundlePath + "/config.txt", (data) =>
                    {
                        if (data != null)
                        {
                            localConfig = FileUtil.TxtToDic(data.text);
                            Debug.Log("读取资源配置文件");
                        }
                    });
                }
                //从persistentPath中读取
                if (resourceManager.readType == ReadType.FromPersistentPath)
                {
                    assetBundlePath = Application.persistentDataPath + "/" + PlatformUtil.GetPlatformName();
                    localConfig = FileUtil.TxtToDic(File.ReadAllText(assetBundlePath + "/config.txt"));
                }

                foreach (var item in localConfig)
                {
                    if (item.Key == "version")
                    {
                        continue;
                    }
                    Debug.Log("加载AssetBundle：" + item.Key);
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath + "/" + item.Key);
                    yield return request;
                    if (request.assetBundle == null)
                    {
                        onReadError?.Invoke();
                        yield break;
                    }
                    else
                    {
                        AddBundle(request.assetBundle);
                    }
                }
                onReadCompleted?.Invoke();
            }
            /// <summary>
            /// 添加AssetBundle
            /// </summary>
            /// <param name="assetBundle"></param>
            public void AddBundle(AssetBundle assetBundle)
            {
                if (!cachedBundles.Contains(assetBundle))
                {
                    cachedBundles.Add(assetBundle);
                }
            }
        }
    }
}