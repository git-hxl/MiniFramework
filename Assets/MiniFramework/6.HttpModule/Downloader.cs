using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniFramework
{
    public class Downloader
    {
        public string fileName;
        public string fileDir;
        public string filePath;
        public float progress;
        private string url;
        public event Action<string> onDownloadCompleted;
        public event Action<string> onDownloadError;
        public Downloader(string saveDir, string url)
        {
            this.url = url;
            fileDir = saveDir;
            string[] txts = url.Split('?');
            fileName = url.GetFileName();
            filePath = fileDir + "/" + fileName;
        }

        public IEnumerator Download()
        {
            string tempFilePath = filePath + ".tmp";
            //using (UnityWebRequest headRequest = UnityWebRequest.Head(url))
            //{
            //    headRequest.certificateHandler = new BypassCertificate();
            //    yield return headRequest.SendWebRequest();
            //    if (headRequest.isHttpError || headRequest.isNetworkError)
            //    {
            //        Debug.LogError("下载文件：" + downloadFileName + " " + headRequest.error);
            //        onDownloadError?.Invoke();
            //        isError = true;
            //        yield break;
            //    }
            //    string contentLength = headRequest.GetResponseHeader("Content-Length");
            //    Debug.Log(contentLength);
            //if (!string.IsNullOrEmpty(contentLength))
            //{
            //    totalLength = long.Parse(contentLength);
            //}
            //else
            //{
            //    Debug.LogError("无法获取资源大小");
            //    onDownloadError?.Invoke();
            //    isError = true;
            //    yield break;
            //}
            //}
            Debug.Log("开始下载:" + fileName);
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.downloadHandler = new DownloadHandlerFile(tempFilePath);
                request.SendWebRequest();
                while (!request.isDone)
                {
                    progress = request.downloadProgress;
                    yield return null;
                }
                if (request.isHttpError || request.isNetworkError)
                {
                    Debug.LogError(request.error);
                    onDownloadError?.Invoke(fileName);
                    yield break;
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                File.Move(tempFilePath, filePath);
                long fileLength = FileUtil.GetFileLength(filePath);
                Debug.Log("下载成功:" + fileName + "大小：" + fileLength + "; md5: " + FileUtil.GetMD5(filePath));
                onDownloadCompleted?.Invoke(fileName);
            }
        }
    }
}