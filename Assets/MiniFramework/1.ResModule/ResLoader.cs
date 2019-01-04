﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace MiniFramework
{
    public class ResLoader : MonoSingleton<ResLoader>
    {
        protected override void OnSingletonInit(){}
        /// <summary>
        /// 获取所有AssetBundle的Hash128
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Dictionary<string, Hash128> GetAllAssetBundleHash(string path)
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
        /// <summary>
        /// 异步加载AssetBundle包
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="path"></param>
        /// <param name="loadedCallBack"></param>
        public void LoadAssetBundle(string path, string assetName,Action<UnityEngine.Object> callback)
        {
            if (File.Exists(path))
                StartCoroutine(loadAssetBundle(path, assetName, callback));
        }
        IEnumerator loadAssetBundle(string loadPath, string assetName,Action<UnityEngine.Object> callback)
        {
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(loadPath);
            yield return bundleLoadRequest;
            var loadedAssetBundle = bundleLoadRequest.assetBundle;
            if (loadedAssetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
                yield break;
            }
            AssetBundleRequest requset = loadedAssetBundle.LoadAssetAsync(assetName);
            yield return requset;
            var obj = requset.asset;
            callback(obj);
            loadedAssetBundle.Unload(false);
        }

        /// <summary>
        /// 异步加载AssetBundle包
        /// 
        /// 
        /// 所有资源
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="path"></param>
        /// <param name="loadedCallBack"></param>
        public void LoadAllAssetBundle(string path, Action<UnityEngine.Object[]> callback) 
        {
            if (File.Exists(path))
                StartCoroutine(loadAllAssetBundle(path, callback));
        }
        IEnumerator loadAllAssetBundle(string loadPath, Action<UnityEngine.Object[]> callback)
        {
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(loadPath);
            yield return bundleLoadRequest;
            var loadedAssetBundle = bundleLoadRequest.assetBundle;
            if (loadedAssetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
                yield break;
            }
            AssetBundleRequest requset = loadedAssetBundle.LoadAllAssetsAsync();
            yield return requset;
            var obj = requset.allAssets;
            callback(obj);
            loadedAssetBundle.Unload(false);
            
        }
    }
}