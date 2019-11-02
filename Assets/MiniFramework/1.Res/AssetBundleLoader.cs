using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public static class AssetBundleLoader
    {
        public static IEnumerator LoadAssetBundle(string path,Action<AssetBundle> callback)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            yield return request;
            if(request.assetBundle != null)
            {
                callback(request.assetBundle);
            }
            else
            {
                Debug.LogError("AssetBundleº”‘ÿ ß∞‹");
            }
        }
        public static Dictionary<string, Hash128> LoadABManifest(string path)
        {
            Dictionary<string, Hash128> bundlesHash = new Dictionary<string, Hash128>();
            if (File.Exists(path))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
                AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                string[] bundlesName = manifest.GetAllAssetBundles();
                for (int i = 0; i < bundlesName.Length; i++)
                {
                    Hash128 hash = manifest.GetAssetBundleHash(bundlesName[i]);
                    bundlesHash.Add(bundlesName[i], hash);
                }
                assetBundle.Unload(true);
            }
            return bundlesHash;
        }
    }
}