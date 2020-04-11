using System.Collections;

namespace MiniFramework.Resource
{
    public interface IResourceUpdate
    {
        /// <summary>
        /// 检查配置文件
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckConfig();
        /// <summary>
        /// 下载差异化资源
        /// </summary>
        /// <returns></returns>
        IEnumerator DownloadNewAssetBundle();
        /// <summary>
        /// 修复文件错误
        /// </summary>
        void RepairErro();
    }
}

