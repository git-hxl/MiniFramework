using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace MiniFramework
{
    public class HttpDownload
    {
        public string FileName;
        public string SaveDir;
        public string FilePath;
        public float Progress;
        public long CurLength;
        public long TotalLength;
        public bool isError;
        public HttpDownload(string saveDir)
        {
            SaveDir = saveDir;
            Directory.CreateDirectory(saveDir);
        }
        public IEnumerator Download(string url, Action<bool> callback = null)
        {
            FileName = url.Substring(url.LastIndexOf('/') + 1);
            FilePath = SaveDir + "/" + FileName;
            string tempFilePath = FilePath + ".tmp";
            CurLength = FileUtil.GetFileLength(tempFilePath);
            using (UnityWebRequest headRequest = UnityWebRequest.Head(url))
            {
                yield return headRequest.SendWebRequest();
                if (headRequest.isHttpError || headRequest.isNetworkError)
                {
                    Debug.LogError("文件："+FileName+" "+headRequest.error);
                    isError = true;
                    if (callback != null)
                        callback(false);
                    yield break;
                }
                string contentLength = headRequest.GetResponseHeader("Content-Length");
                if (!string.IsNullOrEmpty(contentLength))
                {
                    TotalLength = long.Parse(contentLength);
                }
                else
                {
                    Debug.LogError("无法获取资源大小");
                    isError = true;
                    if (callback != null)
                        callback(false);
                    yield break;
                }
            }
            Debug.Log("开始下载:" + FileName + " 大小:" + ByteConvert.ByteAutoConvert(TotalLength) + " 已下载:" + ByteConvert.ByteAutoConvert(CurLength));
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetRequestHeader("Range", "bytes=" + CurLength + "-");
                request.SendWebRequest();
                using (FileStream fileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fileStream.Seek(CurLength, SeekOrigin.Begin);
                    int index = 0;
                    while (!request.isDone)
                    {
                        yield return null;
                        byte[] data = request.downloadHandler.data;
                        int writeLength = data.Length - index;
                        fileStream.Write(data, index, writeLength);
                        index = data.Length;
                        CurLength += writeLength;
                        Progress = CurLength * 1.0f / TotalLength;
                    }

                    if (request.isHttpError || request.isNetworkError)
                    {
                        Debug.LogError(request.error);
                        isError = true;
                        if (callback != null)
                            callback(false);
                        yield break;
                    }
                }
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
                File.Move(tempFilePath, FilePath);
                Debug.Log("下载成功:" + FileName);
                isError = false;
                if (callback != null)
                    callback(true);
            }
        }
    }
}