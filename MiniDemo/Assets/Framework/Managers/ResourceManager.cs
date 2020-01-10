using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

namespace MiniFramework
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public string ABPath;
        private List<AssetBundle> bundles = new List<AssetBundle>();

        private IEnumerator Start()
        {
            if (!string.IsNullOrEmpty(ABPath))
                yield return LoadAssetBundle(ABPath);
        }
        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle(string resPath)
        {
            string[] files = Directory.GetFiles(resPath);
            foreach (var item in files)
            {
                string fileName = Path.GetFileName(item);
                string extension = Path.GetExtension(item);
                if (extension != "")
                {
                    continue;
                }
                Debug.Log("加载AssetBundle：" + fileName);
                yield return AssetBundleLoader.LoadAssetBundle(item, (assetBundle) =>
                {
                    bundles.Add(assetBundle);
                });
            }
        }
        /// <summary>
        /// 从本地或者AssetBundle中加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Load<T>(string name) where T : Object
        {
            T asset = null;
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
            return asset;
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
    }
}