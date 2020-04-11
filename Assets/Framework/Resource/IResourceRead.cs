using System.Collections;
using UnityEngine;
namespace MiniFramework.Resource
{
    public interface IResourceRead
    {
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