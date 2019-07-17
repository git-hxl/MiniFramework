using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class ResManager : MonoSingleton<ResManager>
    {
        private List<AssetBundle> bundles = new List<AssetBundle>();
        private string assetPath;
        public IEnumerator Init(string platform)
        {
            assetPath = Application.persistentDataPath + "/" + platform;
            Dictionary<string, Hash128> files = AssetBundleLoader.LoadABManifest(assetPath + "/" + platform);
            foreach (var item in files)
            {
                Debug.Log("加载资源:" + item.Key);
                yield return AssetBundleLoader.LoadAssetBundle(assetPath + "/" + item.Key);
                bundles.Add(AssetBundleLoader.CurAssetBundle);
            }
            AssetBundleLoader.CurAssetBundle = null;
        }
        public Object Load(string name)
        {
            Object asset = Resources.Load(name);
            if (asset == null)
            {
                foreach (var item in bundles)
                {
                    name = name.Substring(name.LastIndexOf('/') + 1);
                    if (item.Contains(name))
                    {
                        asset = item.LoadAsset(name);
                        break;
                    }
                }
            }
            if (asset == null)
            {
                Debug.LogError("加载失败:" + name);
            }
            return asset;
        }
    }
}

