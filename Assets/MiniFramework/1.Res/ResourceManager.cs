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
                foreach (var item in Bundles)
                {
                    name = name.Substring(name.LastIndexOf('/') + 1);
                    if (item.Contains(name))
                    {
                        asset = item.LoadAsset<T>(name);
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

        public void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(name, mode);
        }
    }
}