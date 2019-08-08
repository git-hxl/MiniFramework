using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MiniFramework
{
    public static class AssetBundleLoader
    {
        public static AssetBundle CurAssetBundle;
        public static float Progress;
        public static IEnumerator LoadAssetBundle(string path)
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            while(!request.isDone){
                yield return null;
                Progress = request.progress;
                Debug.Log(Progress);
            }
            CurAssetBundle = request.assetBundle;
            if (CurAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                yield break;
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