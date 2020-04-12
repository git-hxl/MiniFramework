using System;
using System.Collections;

namespace MiniFramework.WebRequest
{
    public interface IDownload
    {
        /// <summary>
        /// 下载进度
        /// </summary>
        float downloadProgress { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        string downloadFileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        string downloadFilePath { get; set; }
        /// <summary>
        /// 文件下载保存路径
        /// </summary>
        string downloadSaveDir { get; set; }
        /// <summary>
        /// 当前已下载大小
        /// </summary>
        long curlength { get; set; }
        /// <summary>
        /// 文件总大小
        /// </summary>
        long totalLength { get; set; }
        /// <summary>
        /// 是否出错
        /// </summary>
        bool isError { get; set; }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEnumerator Get(string url);
        /// <summary>
        /// 下载完成
        /// </summary>
        event Action<IDownload> onDownloadCompleted;
        /// <summary>
        /// 下载失败
        /// </summary>
        event Action onDownloadError;
    }
}