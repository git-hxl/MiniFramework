using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public class AssetBundleManager : MonoSingleton<AssetBundleManager>
    {
        protected override void OnSingletonInit() { }

        public void LoadAsyncFromAssetBundle(string assetBundlePath, string assetName, Action<UnityEngine.Object> loadCallback)
        {
            StartCoroutine(requestIEnumerator(assetBundlePath, assetName, loadCallback));
        }
        public void LoadAllAsyncFromAssetBundle(string assetBundlePath, Action<UnityEngine.Object[]> loadCallback)
        {
            StartCoroutine(requestIEnumerator(assetBundlePath, loadCallback));
        }
        public void LoadAsyncFromResource(string assetPath, Action<UnityEngine.Object> loadCallback)
        {
            StartCoroutine(requestIEnumerator(assetPath, loadCallback));
        }
        IEnumerator requestIEnumerator(string assetPath, Action<UnityEngine.Object> loadCallback)
        {
            ResourceRequest request = Resources.LoadAsync(assetPath);
            yield return request;
            if (request.asset == null)
            {
                Debug.Log("Failed to load Asset!");
                yield break;
            }
            loadCallback(request.asset);
        }
        IEnumerator requestIEnumerator(string assetBundlePath, string assetName, Action<UnityEngine.Object> loadCallback)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
            yield return request;
            AssetBundle myLoadedAssetBundle = request.assetBundle;
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                yield break;
            }
            AssetBundleRequest assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync(assetName);
            yield return assetLoadRequest;
            loadCallback(assetLoadRequest.asset);
            myLoadedAssetBundle.Unload(false);
        }
        IEnumerator requestIEnumerator(string assetBundlePath, Action<UnityEngine.Object[]> loadCallback)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
            yield return request;
            AssetBundle myLoadedAssetBundle = request.assetBundle;
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                yield break;
            }
            AssetBundleRequest assetLoadRequest = myLoadedAssetBundle.LoadAllAssetsAsync();
            yield return assetLoadRequest;
            loadCallback(assetLoadRequest.allAssets);
            myLoadedAssetBundle.Unload(false);
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
            assetBundle.Unload(true);
            return bundlesHash;
        }
    }
}