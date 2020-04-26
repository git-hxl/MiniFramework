using System;
using System.Collections;

namespace MiniFramework.WebRequest
{
    public interface IDownloader
    {
        /// <summary>
        /// 下载进度
        /// </summary>
        float downloadProgress { get; }
        /// <summary>
        /// 下载速度 单位（kb/s）
        /// </summary>
        int downloadSpeed { get; }
        /// <summary>
        /// 文件名
        /// </summary>
        string downloadFileName { get; }
        /// <summary>
        /// 文件路径
        /// </summary>
        string downloadFilePath { get; }
        /// <summary>
        /// 文件下载保存路径
        /// </summary>
        string downloadSaveDir { get; }
        /// <summary>
        /// 当前已下载大小
        /// </summary>
        long curlength { get; }
        /// <summary>
        /// 文件总大小
        /// </summary>
        long totalLength { get; }
        /// <summary>
        /// 是否出错
        /// </summary>
        bool isError { get; }
        /// <summary>
        /// 是否完成
        /// </summary>
        bool isCompleted { get; }
        /// <summary>
        /// 下载完成
        /// </summary>
        event Action onDownloadCompleted;
        /// <summary>
        /// 下载失败
        /// </summary>
        event Action onDownloadError;
    }
}