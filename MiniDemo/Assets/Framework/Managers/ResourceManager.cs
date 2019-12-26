using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniFramework
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public List<AssetBundle> Bundles = new List<AssetBundle>();
        
        /// <summary>
        /// 从Resources、Assets和AssetBundle中依次加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Load<T>(string name) where T : Object
        {
            T asset = Resources.Load<T>(name);
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
            name = name.Substring(name.LastIndexOf('/') + 1);
            string[] guids = UnityEditor.AssetDatabase.FindAssets(name + " t:" + typeof(T).Name);
            foreach (string guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                if (name == System.IO.Path.GetFileNameWithoutExtension(path))
                {
                    T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                    if (asset != null)
                    {
                        return asset;
                    }
                }
            }
            return null;
        }
#endif
        public T LoadFromAB<T>(string name) where T : Object
        {
            name = name.Substring(name.LastIndexOf('/') + 1);
            foreach (var item in Bundles)
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