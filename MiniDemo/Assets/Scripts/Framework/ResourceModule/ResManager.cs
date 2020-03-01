using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
namespace MiniFramework
{
    public class ResManager : MonoSingleton<ResManager>
    {
        public bool isLocal;
        private string bundlePath;
        private List<AssetBundle> bundles = new List<AssetBundle>();
        public bool isReady;

        private IEnumerator Start()
        {
            if (isLocal)
            {
                bundlePath = Application.streamingAssetsPath + "/" + PlatformUtil.GetPlatformName();
            }
            else
            {
                bundlePath = Application.persistentDataPath + "/" + PlatformUtil.GetPlatformName();
            }
            //从配置文件中获得bundle名
            string config = "";
            yield return FileUtil.ReadStreamingFile(bundlePath + "/config.txt", (data) =>
            {
                if (data != null)
                {
                    config = Encoding.UTF8.GetString(data);
                    Debug.Log("读取资源配置文件");
                }
                    
            });
            string[] bundleConfig = config.Split('\n');
            //加载assetbundle
            for (int i = 1; i < bundleConfig.Length; i++)
            {
                string bundleName = bundleConfig[i].Split(':')[0];
                Debug.Log("加载AssetBundle：" +bundleName);
                yield return AssetBundleLoader.LoadAssetBundle(bundlePath + "/" + bundleName, AddBundle);
            }
            isReady = true;
        }
        void AddBundle(AssetBundle assetBundle)
        {
            bundles.Add(assetBundle);
        }

        /// <summary>
        /// 从本地或者AssetBundle中加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Load<T>(string name) where T : Object
        {

            Object asset = GetFromCache(name);
#if UNITY_EDITOR
            if (asset == null)
            {
                asset = LoadFromEditor<T>(name);
            }
#endif
            if (asset == null)
            {
                asset = LoadFromAB<T>(name);
            }
            if (asset == null)
            {
                Debug.LogError("加载资源对象失败:" + name);
            }
            else
            {
                CacheAsset(name, asset);
            }
            return asset as T;
        }
#if UNITY_EDITOR
        private T LoadFromEditor<T>(string name) where T : Object
        {
            T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(name);
            if (asset != null)
            {
                return asset;
            }
            return null;
        }
#endif
        public T LoadFromAB<T>(string name) where T : Object
        {
            name = name.Substring(name.LastIndexOf('/') + 1);
            foreach (var item in bundles)
            {
                if (item.Contains(name))
                {
                    T asset = item.LoadAsset<T>(name);
                    if (asset != null)
                    {
                        return asset;
                    }
                }
            }
            return null;
        }
        public void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(name, mode);
        }

        private Dictionary<string, Object> cacheAssets = new Dictionary<string, Object>();
       
        private void CacheAsset(string name, Object asset)
        {
            if(asset!=null&&!cacheAssets.ContainsKey(name))
            {
                cacheAssets.Add(name, asset);
            }
        }
        private Object GetFromCache(string name)
        {
            Object asset = null;
            cacheAssets.TryGetValue(name, out asset);
            return asset;
        }
    }
}