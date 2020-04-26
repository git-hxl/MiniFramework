using System;
using System.Collections;
using UnityEngine;
namespace MiniFramework.Resource
{
    public interface IResourceRead
    {
        /// <summary>
        /// 资源读取完成事件
        /// </summary>
        event Action onReadCompleted;
        /// <summary>
        /// 资源读取失败
        /// </summary>
        event Action onReadError;
        /// <summary>
        /// 读取所有AssetBundle文件
        /// </summary>
        /// <returns></returns>
        IEnumerator ReadAll();
        /// <summary>
        /// 添加AssetBundle
        /// </summary>
        /// <param name="assetBundle"></param>
        void AddBundle(AssetBundle assetBundle);
    }
}