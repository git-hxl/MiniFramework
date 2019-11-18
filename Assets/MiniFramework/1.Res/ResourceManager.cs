using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniFramework
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public List<AssetBundle> Bundles = new List<AssetBundle>();
        /// 加载资源
        /// </summary>
        /// <param name="name">资源名</param>
        /// <returns></returns>
        public T Load<T>(string name) where T : Object
        {
            T asset = Resources.Load<T>(name);
            if (asset == null)
            {
                asset = LoadFromEditor<T>(name);
            }
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
        public T LoadFromEditor<T>(string name) where T : Object
        {
#if UNITY_EDITOR
            name = name.Substring(name.LastIndexOf('/') + 1);
            string type = typeof(T).ToString();
            type = type.Substring(type.LastIndexOf('.') + 1);
            string[] guids = UnityEditor.AssetDatabase.FindAssets(name + " t:" + type);
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
#endif
            return null;
        }
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