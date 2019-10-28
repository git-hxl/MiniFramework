using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniFramework
{
    public class ResManager : MonoSingleton<ResManager>
    {
        public List<AssetBundle> bundles = new List<AssetBundle>();
        private string assetPath;
        [SerializeField]
        public CheckUpdate CheckUpdate = new CheckUpdate();
        //private IEnumerator Start()
        //{
        //    yield return CheckUpdate.Check();
        //    yield return Init(CheckUpdate.Platform);
        //}
        //private IEnumerator Init(string platform)
        //{
        //    assetPath = Application.persistentDataPath + "/" + platform;
        //    Dictionary<string, Hash128> files = AssetBundleLoader.LoadABManifest(assetPath + "/" + platform);
        //    foreach (var item in files)
        //    {
        //        Debug.Log("正在加载资源:" + item.Key);
        //        yield return AssetBundleLoader.LoadAssetBundle(assetPath + "/" + item.Key);
        //        if (AssetBundleLoader.CurAssetBundle != null)
        //        {
        //            bundles.Add(AssetBundleLoader.CurAssetBundle);
        //        }
        //    }
        //    AssetBundleLoader.CurAssetBundle = null;
        //}
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
                Debug.LogError("加载资源对象失败:" + name);
            }
            return asset;
        }
        //public T LoadFromPath<T>(string path) where T : Object
        //{
        //    return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        //}

        public void LoadScene(string name, LoadSceneMode mode)
        {
            SceneManager.LoadScene(name, mode);
        }
    }
}