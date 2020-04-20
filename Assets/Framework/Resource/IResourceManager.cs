using MiniFramework.WebRequest;
using UnityEngine;
namespace MiniFramework.Resource
{
    public interface IResourceManager
    {
        /// <summary>
        /// 获取更新信息
        /// </summary>
        /// <returns></returns>
        IResourceUpdate GetResourceUpdate { get; }
        /// <summary>
        /// 获取加载信息
        /// </summary>
        /// <returns></returns>
        IResourceRead GetResourceRead { get; }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>

        T LoadAsset<T>(string name) where T : Object;
        /// <summary>
        /// 从编辑器模式下加载
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        T LoadAssetFromEditor<T>(string name) where T : Object;
        /// <summary>
        /// 从AssetBundle中加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>

        T LoadAssetFromAssetBundle<T>(string name) where T : Object;
        /// <summary>
        /// 卸载所有资源
        /// </summary>
        void UnLoadAll();
    }
}