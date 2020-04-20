using MiniFramework.WebRequest;
using System;
using System.Collections;

namespace MiniFramework.Resource
{
    public interface IResourceUpdate
    {
        /// <summary>
        /// 总计需更新数量
        /// </summary>
        int TotalUpdateCount { get; }
        /// <summary>
        /// 已更新数量
        /// </summary>
        int UpdatedCount { get; }
        /// <summary>
        /// 下载配置文件
        /// </summary>
        void DownloadConfig();
        /// <summary>
        /// 下载器
        /// </summary>
        /// <returns></returns>
        IDownloader GetDownloader { get; }
        /// <summary>
        /// 更新完成事件
        /// </summary>
        event Action onUpdateCompleted;
        /// <summary>
        /// 更新失败事件
        /// </summary>
        event Action onUpdateError;
        /// <summary>
        /// 修复文件
        /// </summary>
        void RepairFile();
    }
}

