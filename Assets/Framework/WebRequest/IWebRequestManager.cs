using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniFramework.WebRequest
{
    public interface IWebRequestManager 
    {
        /// <summary>
        /// 获取下载接口
        /// </summary>
        /// <param name="dir">保存路径</param>
        /// <returns></returns>
        IDownload Downloader(string dir);
        /// <summary>
        /// UnityWebRequest Get封装
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        void Get(string url, Action<DownloadHandler> callback);

        /// <summary>
        /// UnityWebRequest Put封装
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        void Put(string url,byte[] data, Action<DownloadHandler> callback);

        /// <summary>
        /// UnityWebRequest Post封装
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        void Post(string url, WWWForm form, Action<DownloadHandler> callback);
    }
}