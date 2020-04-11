using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace MiniFramework.Resource
{
    public partial class ResourceManager
    {
        private class ResourceRead : IResourceRead
        {
            private string resPath;

            private List<AssetBundle> cachedBundles;

            public List<AssetBundle> CacheBundles
            {
                get { return cachedBundles; }
            }

            private ResourceManager resourceManager;
            public ResourceRead(ResourceManager resourceManager)
            {
                this.resourceManager = resourceManager;
                switch (resourceManager.readType)
                {
                    case ReadType.None:
                    case ReadType.FromStreamingAssets:
                        resPath = Application.streamingAssetsPath + "/" + PlatformUtil.GetPlatformName();
                        break;
                    case ReadType.FromPersistentPath:
                        resPath = Application.persistentDataPath + "/" + PlatformUtil.GetPlatformName();
                        break;
                }
                cachedBundles = new List<AssetBundle>();
            }
            /// <summary>
            /// 读取所有AssetBundle文件
            /// </summary>
            /// <returns></returns>
            public IEnumerator ReadAll()
            {
                if (resourceManager.readType == ReadType.None)
                {
                    yield break;
                }
                List<string> bundles = new List<string>();
                yield return FileUtil.ReadStreamingFile(resPath + "/config.txt", (data) =>
                {
                    if (data != null)
                    {
                        string txt = Encoding.UTF8.GetString(data);
                        var configDict = FileUtil.ReadConfig(txt);
                        foreach (var item in configDict)
                        {
                            if (item.Key == "version")
                            {
                                continue;
                            }
                            bundles.Add(item.Key);
                        }
                        Debug.Log("读取资源配置文件");
                    }
                });
                for (int i = 0; i < bundles.Count; i++)
                {
                    Debug.Log("加载AssetBundle：" + bundles[i]);
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(resPath + "/" + bundles[i]);
                    yield return request;
                    if (request.assetBundle == null)
                    {
                        resourceManager.readFailCallback?.Invoke();
                        yield break;
                    }
                    else
                    {
                        AddBundle(request.assetBundle);
                    }
                }
                Debug.Log("资源加载完成");
                resourceManager.readSuccessCallback?.Invoke();
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