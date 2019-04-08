using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniFramework
{
    public class AssetBundleHelper : MonoSingleton<AssetBundleHelper>
    {
        protected override void OnSingletonInit(){}
        public void LoadAssetBundle(string assetBundlePath, string assetName, Action<UnityEngine.Object> loadCallback)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
            StartCoroutine(requestIEnumerator(request, assetName, loadCallback));
        }
        public void LoadAssetBundles(string assetBundlePath, Action<UnityEngine.Object[]> loadCallback)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
            StartCoroutine(requestIEnumerator(request, loadCallback));
        }
        public void UnloadAsset(UnityEngine.Object asset)
        {
            Resources.UnloadAsset(asset);
        }
        public void UnloadUnUsedAsset()
        {
            Resources.UnloadUnusedAssets();
        }
        IEnumerator requestIEnumerator(AssetBundleCreateRequest request, string assetName, Action<UnityEngine.Object> loadCallback)
        {
            yield return request;
            if (request.isDone)
            {
                AssetBundleRequest requset = request.assetBundle.LoadAssetAsync(assetName);
                yield return requset;
                loadCallback(requset.asset);
            }
        }
        IEnumerator requestIEnumerator(AssetBundleCreateRequest request, Action<UnityEngine.Object[]> loadCallback)
        {
            yield return request;
            if (request.isDone)
            {
                AssetBundleRequest requset = request.assetBundle.LoadAllAssetsAsync();
                yield return requset;
                loadCallback(requset.allAssets);
            }
        }
        public Dictionary<string, Hash128> LoadABManifest(string path)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] bundlesName = manifest.GetAllAssetBundles();
            Dictionary<string, Hash128> bundlesHash = new Dictionary<string, Hash128>();
            for (int i = 0; i < bundlesName.Length; i++)
            {
                Hash128 hash = manifest.GetAssetBundleHash(bundlesName[i]);
                bundlesHash.Add(bundlesName[i], hash);
            }
            return bundlesHash;
        }
    }
}

