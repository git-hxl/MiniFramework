using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniFramework
{
    public class AssetLoader
    {
        private MonoBehaviour monoBehaviour;
        public AssetLoader(MonoBehaviour mono)
        {
            monoBehaviour = mono;
        }
        public void LoadAsset(string assetPath, Action<UnityEngine.Object> loadCallback)
        {
            ResourceRequest request = Resources.LoadAsync(assetPath);
            monoBehaviour.StartCoroutine(requestIEnumerator(request, loadCallback));
        }
        public void LoadAsset(string assetPath,Type type,Action<UnityEngine.Object> loadCallback)
        {
            ResourceRequest request = Resources.LoadAsync(assetPath,type);
            monoBehaviour.StartCoroutine(requestIEnumerator(request, loadCallback));
        }
        public void LoadAllAsset(string assetPath,Action<UnityEngine.Object[]> loadCallback){
            loadCallback(Resources.LoadAll(assetPath));
        }
        public void LoadAssetBundle(string assetBundlePath, string assetName, Action<UnityEngine.Object> loadCallback)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
            monoBehaviour.StartCoroutine(requestIEnumerator(request, assetName, loadCallback));
        }

        public void LoadAssetBundles(string assetBundlePath, Action<UnityEngine.Object[]> loadCallback)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
            monoBehaviour.StartCoroutine(requestIEnumerator(request, loadCallback));
        }
        public void UnloadAsset(UnityEngine.Object asset)
        {
            Resources.UnloadAsset(asset);
        }
        public void UnloadUnUsedAsset()
        {
            Resources.UnloadUnusedAssets();
        }
        IEnumerator requestIEnumerator(ResourceRequest request, Action<UnityEngine.Object> loadCallback)
        {
            yield return request;
            if (request.isDone && request.asset != null)
            {
                loadCallback(request.asset);
            }
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

