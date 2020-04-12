using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniFramework.WebRequest
{
    public sealed partial class WebRequestManager : MonoSingleton<WebRequestManager>,IWebRequestManager
    {
        /// <summary>
        /// 获取下载接口
        /// </summary>
        /// <param name="dir">保存路径</param>
        /// <returns></returns>
        public IDownload Downloader(string dir)
        {
            return new Download(dir);
        }
        /// <summary>
        /// UnityWebRequest Get封装
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        public void Get(string url, Action<DownloadHandler> callback)
        {
            StartCoroutine(GetEnumerator(url,callback));
        }
        private IEnumerator GetEnumerator(string url, Action<DownloadHandler> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    Debug.Log("Get complete!");
                    callback?.Invoke(www.downloadHandler);
                }
            }
        }
        /// <summary>
        /// UnityWebRequest Post封装
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        public void Post(string url, WWWForm form, Action<DownloadHandler> callback)
        {
            StartCoroutine(PostEnumerator(url, form, callback));
        }
        private IEnumerator PostEnumerator(string url, WWWForm form, Action<DownloadHandler> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                    callback?.Invoke(www.downloadHandler);
                }
            }
        }
        /// <summary>
        /// UnityWebRequest Put封装
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        public void Put(string url, byte[] data, Action<DownloadHandler> callback)
        {
            StartCoroutine(PutEnumerator(url, data, callback));
        }
        private IEnumerator PutEnumerator(string url, byte[] data, Action<DownloadHandler> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Put(url, data))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    Debug.Log("Upload complete!");
                    callback?.Invoke(www.downloadHandler);
                }
            }
        }
    }
}